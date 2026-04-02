namespace DevelopmentTask.Core.Models.Exceptions
{
    internal sealed class TreeNotFoundException : SecureException
    {
        public TreeNotFoundException(long nodeId)
            : base($"Tree with id '{nodeId}' was not found.")
        {
        }

        public TreeNotFoundException(string treeName)
            : base($"Tree '{treeName}' was not found.")
        {
        }
    }
}
