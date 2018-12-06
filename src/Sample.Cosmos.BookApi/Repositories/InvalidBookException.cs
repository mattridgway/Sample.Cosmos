using System;

namespace Sample.Cosmos.BookApi.Repositories
{
    [Serializable]
    public class InvalidBookException : Exception
    {
        public InvalidBookException() { }
        public InvalidBookException(string message) : base(message) { }
        public InvalidBookException(string message, Exception inner) : base(message, inner) { }
        protected InvalidBookException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
