using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.Exceptions
{
    public class StorageItemEmptyException : Exception
    {
        public StorageItemEmptyException() { }
        public StorageItemEmptyException(string message) : base(message) { }
        public StorageItemEmptyException(string message, Exception inner) : base(message, inner) { }
        protected StorageItemEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
