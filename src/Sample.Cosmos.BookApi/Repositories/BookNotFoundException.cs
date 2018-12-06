using System;

namespace Sample.Cosmos.BookApi.Repositories
{
    [Serializable]
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() { }
        public BookNotFoundException(string message) : base(message) { }
        public BookNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected BookNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
