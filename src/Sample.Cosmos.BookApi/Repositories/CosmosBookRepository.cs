using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Sample.Cosmos.BookApi.Models;

namespace Sample.Cosmos.BookApi.Repositories
{
    internal class CosmosBookRepository : IBookRepository, IDisposable
    {
        private readonly DocumentClient _client;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public CosmosBookRepository(IOptions<CosmosConfiguration> config)
        {
            _databaseName = "library";
            _collectionName = "books";

            _client = new DocumentClient(new Uri(config.Value.Endpoint), config.Value.Key);
        }

        public async Task<Book> CreateBookAsync(string name, string description, string ISBN)
        {
            try
            {
                var collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
                var document = await _client.CreateDocumentAsync(collectionLink, new BookEntity { CreatedOn = DateTime.Now, Name = name, Description = description, ISBN = ISBN });
                var book = (BookEntity)(dynamic)document.Resource;

                return new Book { Id = book.Id, Name = book.Name, Description = book.Description, ISBN = book.ISBN };
            }
            catch (DocumentClientException exception)
            {
                if (exception.StatusCode == System.Net.HttpStatusCode.Conflict)
                    throw new PersistenceException("Cosmos returned Conflict which means a document with this ID already exists");

                if (exception.StatusCode == System.Net.HttpStatusCode.RequestEntityTooLarge)
                    throw new InvalidBookException("Book is too big for the database");

                if (exception.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new PersistenceException("Cosmos returned Forbidden which usually means the database is full");

                throw;
            }
        }

        public async Task<Book> GetBookAsync(string id)
        {
            try
            {
                var docLink = UriFactory.CreateDocumentUri(_databaseName, _collectionName, id);
                var document = await _client.ReadDocumentAsync(docLink);
                var book = (BookEntity)(dynamic)document.Resource;

                return new Book { Id = book.Id, Name = book.Name, Description = book.Description, ISBN = book.ISBN };
            }
            catch (DocumentClientException exception)
            {
                if (exception.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new BookNotFoundException($"Book with id {id} was not found in the database");

                throw;
            }
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            try
            { 
                var collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
                var existingDocument = _client.CreateDocumentQuery(collectionLink)
                            .Where(doc => doc.Id == book.Id)
                            .AsEnumerable()
                            .Single();
                existingDocument.SetPropertyValue("Name", book.Name);
                existingDocument.SetPropertyValue("Description", book.Description);
                existingDocument.SetPropertyValue("ISBN", book.ISBN);

                var updatedDocument = await _client.ReplaceDocumentAsync(existingDocument,
                    options: new RequestOptions
                    {
                        AccessCondition = new AccessCondition
                        {
                            Condition = existingDocument.ETag,
                            Type = AccessConditionType.IfMatch
                        }
                    });
                var updatedBook = (BookEntity)(dynamic)updatedDocument.Resource;

                return new Book { Id = updatedBook.Id, Name = updatedBook.Name, Description = updatedBook.Description, ISBN = updatedBook.ISBN };
            }
            catch (DocumentClientException exception)
            {
                if (exception.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new BookNotFoundException($"Book with id {book.Id} was not found in the database");

                throw;
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private class BookEntity : Resource
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ISBN { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
        }
    }
}
