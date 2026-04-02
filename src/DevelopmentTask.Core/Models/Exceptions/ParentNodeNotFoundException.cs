namespace DevelopmentTask.Core.Models.Exceptions
{
    internal sealed class ParentNodeNotFoundException : SecureException
    {
        public ParentNodeNotFoundException(long parentId)
            : base($"Parent node with ID '{parentId}' was not found in the tree.")
        {
        }
    }
}
