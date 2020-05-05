using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.Exceptions
{
    public class FileEmptyException : Exception
    {
        public FileEmptyException() { }
        public FileEmptyException(string message) : base(message) { }
        public FileEmptyException(string message, Exception inner) : base(message, inner) { }
        protected FileEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
