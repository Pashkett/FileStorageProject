using System;

namespace FileStorage.Domain.Exceptions
{
    public class StorageItemExeedLimitException : Exception
    {
        public StorageItemExeedLimitException() { }
        public StorageItemExeedLimitException(string message) : base(message) { }
        public StorageItemExeedLimitException(string message, Exception inner) : base(message, inner) { }
        protected StorageItemExeedLimitException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
