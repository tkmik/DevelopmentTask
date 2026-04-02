namespace DevelopmentTask.Core.Models.Exceptions
{
    internal sealed class DuplicateNodeNameException : SecureException
    {
        public DuplicateNodeNameException() : base("Node name must be unique among siblings.")
        { }
    }
}
