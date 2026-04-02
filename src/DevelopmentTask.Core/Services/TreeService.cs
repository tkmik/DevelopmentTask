using System.Linq.Expressions;
using DevelopmentTask.Core.Database.Repositories.Interfaces;
using DevelopmentTask.Core.Models.Dto.TreeNode;
using DevelopmentTask.Core.Models.Entities;
using DevelopmentTask.Core.Models.Exceptions;
using DevelopmentTask.Core.Services.Interfaces;

namespace DevelopmentTask.Core.Services
{
    internal sealed class TreeService : ITreeService
    {
        private readonly ITreeRepository _treeRepository;
        private readonly ITreeNodeRepository _treeNodeRepository;

        public TreeService(ITreeRepository treeRepository, ITreeNodeRepository treeNodeRepository)
        {
            _treeRepository = treeRepository;
            _treeNodeRepository = treeNodeRepository;
        }

        public async Task CreateNodeAsync(CreateTreeNode node, CancellationToken cancellationToken = default)
        {
            Expression<Func<Tree, bool>> filter = x => x.Name == node.TreeName;
            IEnumerable<string> include = [nameof(Tree.Nodes)];

            var tree = await _treeRepository.FirstOrDefaultAsync(filter, include, cancellationToken: cancellationToken);

            if (tree is null)
            {
                throw new TreeNotFoundException(node.TreeName);
            }

            TreeNode? parent = null;

            if (node.ParentId is not null)
            {
                parent = tree.Nodes.FirstOrDefault(n => n.Id == node.ParentId.Value);

                if (parent is null)
                {
                    throw new ParentNodeNotFoundException(node.ParentId.Value);
                }
            }
            else
            {
                parent = tree.Nodes.First(x => x.ParentId == null);
            }

            var siblings = tree.Nodes
               .Where(n => n.ParentId == parent.Id)
               .ToList();

            if (siblings.Any(n => n.Name == node.NodeName))
                throw new DuplicateNodeNameException();

            string path = parent is null
                ? node.NodeName
                : $"{parent.Path}/{node.NodeName}";

            var newNode = new TreeNode
            {
                TreeId = tree.Id,
                ParentId = parent!.Id,
                Name = node.NodeName,
                Path = path
            };

            tree.Nodes.Add(newNode);

            await _treeRepository.UpdateAsync(tree, cancellationToken);
        }

        public async Task DeleteNodeAsync(long nodeId, CancellationToken cancellationToken = default)
        {
            var node = await _treeNodeRepository.GetAsync(nodeId, cancellationToken);

            if (node is null)
            {
                throw new TreeNotFoundException(nodeId);
            }

            var descendants = await _treeNodeRepository.GetDescendantsAsync(nodeId, cancellationToken);

            await using var transaction = await _treeNodeRepository.BeginTransactionAsync(cancellationToken);

            await _treeNodeRepository.DeleteAsync(descendants, cancellationToken);
            await _treeNodeRepository.DeleteAsync(node, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        public async Task<TreeNode> GetOrCreateAsync(string treeName, CancellationToken cancellationToken = default)
        {
            Expression<Func<Tree, bool>> filter = x => x.Name == treeName;
            IEnumerable<string> include = [nameof(Tree.Nodes)];

            var tree = await _treeRepository.FirstOrDefaultAsync(filter, include, cancellationToken: cancellationToken);

            if (tree is null)
            {
                tree = new Tree { Id = Guid.NewGuid(), Name = treeName };

                const string root = "root";

                var treeNode = new TreeNode
                {
                    Name = root,
                    Path = root,
                };

                tree.Nodes.Add(treeNode);

                await _treeRepository.AddAsync(tree, cancellationToken);

                return BuildTreeHierarchy(tree.Nodes);
            }

            return BuildTreeHierarchy(tree.Nodes);
        }

        public async Task UpdateNodeAsync(RenameTreeNode node, CancellationToken cancellationToken = default)
        {
            var treeNode = await _treeNodeRepository.GetAsync(node.Id, cancellationToken);

            if (treeNode is null)
            {
                throw new TreeNotFoundException(node.Id);
            }

            Expression<Func<TreeNode, bool>> filter = x => x.TreeId == treeNode.TreeId
                && x.ParentId == treeNode.ParentId
                && x.Name == treeNode.Name
                && x.Id != node.Id;

            bool exists = await _treeNodeRepository.AnyAsync(filter, cancellationToken);

            if (exists)
            {
                throw new TreeNodeAlreadyExistsException(node.NewName);
            }

            await using var transaction = await _treeNodeRepository.BeginTransactionAsync(cancellationToken);

            string oldName = treeNode.Name;
            string oldPath = treeNode.Path;

            string newPath = oldPath.Replace(oldName, node.NewName);

            treeNode.Name = node.NewName;
            treeNode.Path = newPath;

            var descendants = await _treeNodeRepository.GetDescendantsAsync(treeNode.Id, cancellationToken);

            foreach (var descendant in descendants)
            {
                descendant.Path = descendant.Path.Replace(oldPath, newPath);
            }

            await _treeNodeRepository.UpdateAsync(descendants, cancellationToken);
            await _treeNodeRepository.UpdateAsync(treeNode, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }

        private TreeNode BuildTreeHierarchy(IEnumerable<TreeNode> nodes)
        {
            var lookup = nodes.ToDictionary(n => n.Id);
            TreeNode? root = null;

            foreach (var node in nodes)
            {
                if (node.ParentId is null)
                {
                    root = node;
                    continue;
                }

                if (lookup.TryGetValue(node.ParentId.Value, out var parent))
                {
                    parent.Children ??= new List<TreeNode>();
                    parent.Children.Add(node);
                }
            }

            return root!;
        }
    }
}
