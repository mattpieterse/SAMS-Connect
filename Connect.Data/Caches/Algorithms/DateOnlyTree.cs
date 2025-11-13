using Connect.Data.Models;

namespace Connect.Data.Caches.Algorithms;

/// <summary>
/// AVL Binary Search Tree (BST)
/// </summary>
/// <remarks>
/// It is specifically a multimap variant.
/// </remarks>
public sealed class DateOnlyTree
{
#region Variables

    private Node? _root;

#endregion

#region Functions

    public void Insert(Ticket instance) {
        var key = DateOnly.FromDateTime(instance.CreatedAt);
        _root = InsertInternal(_root, key, instance);
    }


    public IEnumerable<Ticket> Search(
        DateOnly earliestDateInclusive,
        DateOnly furthestDateInclusive
    ) {
        if (_root == null) return [];

        var results = new List<Ticket>();
        TraverseRanges(_root, earliestDateInclusive, furthestDateInclusive, results);
        return results;
    }

#endregion

#region Internals

    /// <summary>
    /// AVL method for balancing operations.
    /// </summary>
    private static Node Balance(Node node) {
        var balance = node.BalanceFactor;

        // If node is LSide Heavy
        if (balance > 1) {
            if (node.LSide is { BalanceFactor: < 0 }) {
                node.LSide = RotateL(node.LSide);
            }
            return RotateR(node);
        }

        // If node is RSide Heavy
        if (balance < -1) {
            if (node.RSide is { BalanceFactor: > 0 }) {
                node.RSide = RotateR(node.RSide);
            }
            return RotateL(node);
        }

        // Else tree is balanced
        return node;
    }


    /// <summary>
    /// AVL method for balancing operations.
    /// </summary>
    private static Node RotateR(Node y) {
        var x = y.LSide!;
        var t2 = x.RSide;

        x.RSide = y;
        y.LSide = t2;

        y.UpdateHeight();
        x.UpdateHeight();

        return x;
    }


    /// <summary>
    /// AVL method for balancing operations.
    /// </summary>
    private static Node RotateL(Node x) {
        var y = x.RSide!;
        var t2 = y.LSide;

        y.LSide = x;
        x.RSide = t2;

        x.UpdateHeight();
        y.UpdateHeight();

        return y;
    }


    private static void TraverseRanges(
        Node? node,
        DateOnly earliestDateInclusive,
        DateOnly furthestDateInclusive,
        ICollection<Ticket> result
    ) {
        if (node == null) return;

        if (earliestDateInclusive < node.Key) {
            TraverseRanges(node.LSide, earliestDateInclusive, furthestDateInclusive, result);
        }

        if ((node.Key >= earliestDateInclusive) && (node.Key <= furthestDateInclusive)) {
            foreach (var item in node.Items) {
                result.Add(item);
            }
        }

        if (node.Key < furthestDateInclusive) {
            TraverseRanges(node.RSide, earliestDateInclusive, furthestDateInclusive, result);
        }
    }


    private static Node InsertInternal(
        Node? node,
        DateOnly key,
        Ticket value
    ) {
        if (node == null)
            return new Node(key, value);

        var compared = key.CompareTo(node.Key);
        switch (compared) {
        case < 0: {
            node.LSide = InsertInternal(node.LSide, key, value);
            break;
        }
        case > 0: {
            node.RSide = InsertInternal(node.RSide, key, value);
            break;
        }
        default: {
            node.Items.Add(value);
            return node;
        }
        }

        node.UpdateHeight();
        return Balance(node);
    }


    public sealed class Node
    {
        public DateOnly Key { get; }
        public List<Ticket> Items { get; } = [];
        public Node? LSide { get; set; } = null;
        public Node? RSide { get; set; } = null;
        private int Height { get; set; } = 1;

        public Node(
            DateOnly key,
            Ticket value
        ) {
            Key = key;
            Items.Add(value);
        }

        public void UpdateHeight() {
            var lSideHeight = LSide?.Height ?? 0;
            var rSideHeight = RSide?.Height ?? 0;
            Height = Math.Max(lSideHeight, rSideHeight) + 1;
        }


        public int BalanceFactor => (
            (LSide?.Height ?? 0) -
            (RSide?.Height ?? 0)
        );
    }

#endregion
}
