using System;

namespace FileStorage.Domain.Exceptions
{
    public class FileExeedLimitException : Exception
    {
        public FileExeedLimitException() { }
        public FileExeedLimitException(string message) : base(message) { }
        public FileExeedLimitException(string message, Exception inner) : base(message, inner) { }
        protected FileExeedLimitException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
