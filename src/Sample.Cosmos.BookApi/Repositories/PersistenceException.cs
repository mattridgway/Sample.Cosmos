using System;

namespace Sample.Cosmos.BookApi.Repositories
{
    [Serializable]
    public class PersistenceException : Exception
    {
        public PersistenceException() { }
        public PersistenceException(string message) : base(message) { }
        public PersistenceException(string message, Exception inner) : base(message, inner) { }
        protected PersistenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
