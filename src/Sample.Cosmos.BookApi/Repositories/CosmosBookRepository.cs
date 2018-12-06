﻿using System;
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
            var collectionLink = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
            var document = await _client.CreateDocumentAsync(collectionLink, new BookEntity { CreatedOn = DateTime.Now, Name = name, Description = description, ISBN = ISBN });
            var book = (BookEntity)(dynamic)document.Resource;

            return new Book { Id = book.Id, Name = book.Name, Description = book.Description, ISBN = book.ISBN };
        }

        public async Task<Book> GetBookAsync(string id)
        {
            var docLink = UriFactory.CreateDocumentUri(_databaseName, _collectionName, id);
            var document = await _client.ReadDocumentAsync(docLink);
            var book = (BookEntity)(dynamic)document.Resource;

            return new Book { Id = book.Id, Name = book.Name, Description = book.Description, ISBN = book.ISBN };
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
