# Cosmos Sample

This repo shows an example of how to handle partial updates to a doucment in Cosmos. The code will run locally against the [Cosmos emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator).

## Partial Updates

Although they have been working on it since March 2018, [Cosmos doesn't support partial updates](https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/6693091-be-able-to-do-partial-updates-on-document) out-of-the-box. The code in this repo works by first reading the existing whole document out of the database, updating it with the values passed in to the repository and then it replaces the entire document with the updated version. 

```c#
var existingDocument = _client.CreateDocumentQuery(collectionLink)
                            .Where(doc => doc.Id == book.Id)
                            .AsEnumerable()
                            .Single();
existingDocument.SetPropertyValue("Name", book.Name);
existingDocument.SetPropertyValue("Description", book.Description);
existingDocument.SetPropertyValue("ISBN", book.ISBN);

var updatedDocument = await _client.ReplaceDocumentAsync(existingDocument);
```

The subtlety here is that rather than construct a new object with the values passed into the repository and simply replace the existing document with that new object, this flow ensures that should there be a property that isn't sent to the client but needs to remain in the document  (such as `CreatedOn` in this repo) it's value won't be lost, because only the required fields are updated on the original document, keeping hidden properties intact.

The one danger still remaining here is that between the document being read and being replaced another thread could have made a change and now the version in the current thread is stale. To counter this Cosmos can check the ETag of the document from the original read and if it doesn't match at the time of the update, reject the change:

```c#
var updatedDocument = await _client.ReplaceDocumentAsync(
    existingDocument,
    options: new RequestOptions {
        AccessCondition = new AccessCondition {
            Condition = existingDocument.ETag,
            Type = AccessConditionType.IfMatch
        }
    });
```
