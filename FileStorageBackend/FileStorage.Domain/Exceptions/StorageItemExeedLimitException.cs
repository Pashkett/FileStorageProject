using System;

namespace FileStorage.Domain.Exceptions
{
    public class StorageItemExceedLimitException : Exception
    {
        public StorageItemExceedLimitException() { }
        public StorageItemExceedLimitException(string message) : base(message) { }
        public StorageItemExceedLimitException(string message, Exception inner) : base(message, inner) { }
        protected StorageItemExceedLimitException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
