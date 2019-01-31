namespace DDDSample.Application
{
    using System;

    public class ApplicationException : Exception
    {
        public ApplicationException(string message) : base(message) { }
    }
}
