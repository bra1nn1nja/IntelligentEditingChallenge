using System;

namespace Intelligent_Editing.Exceptions
{
    public class InvalidLanguageException : Exception
    {
        public InvalidLanguageException()
        {
        }

        public InvalidLanguageException(string message)
            : base(message)
        {
        }

        public InvalidLanguageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
