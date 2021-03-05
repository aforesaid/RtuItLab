using System;

namespace RtuItLab.Infrastructure.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        { }
    }
}
