using System;

namespace FileStorage.Domain.Exceptions
{
    public class StorageItemNotFoundException : Exception
    {
        public StorageItemNotFoundException() { }
        public StorageItemNotFoundException(string message) : base(message) { }
        public StorageItemNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected StorageItemNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
