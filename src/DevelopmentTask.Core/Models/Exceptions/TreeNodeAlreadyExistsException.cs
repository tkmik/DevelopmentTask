namespace DevelopmentTask.Core.Models.Exceptions
{
    internal sealed class TreeNodeAlreadyExistsException : SecureException
    {
        public TreeNodeAlreadyExistsException(string treeName)
            : base($"The tree node with name {treeName} already exists.")
        {
        }
    }
}
