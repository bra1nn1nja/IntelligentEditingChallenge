using System;

namespace Intelligent_Editing.Exceptions
{
    public class InvalidLengthException : Exception
    {
        public InvalidLengthException()
        {
        }

        public InvalidLengthException(string message)
            : base(message)
        {
        }

        public InvalidLengthException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
