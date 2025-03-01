using Common;

namespace MerkleTree;

/*
 *  Define a MerkleTree node, used by MerkleTree
 */
public class MerkleTreeNode {
    // Whether the node is a left child or right child. No apply to Root which
    // is set to left by default
    public enum ChildPosition {
        Left,
        Right,
    }

    // Store the hash val in bytes
    public byte[] hash;
    // Pointer to parrent, null for root node
    public MerkleTreeNode? parent;
    // Pointer to sbiling
    public MerkleTreeNode? sibling;

    public ChildPosition pos;

    // Constructor given hash val
    public MerkleTreeNode (byte[] hash) {
        this.hash = hash;
        this.parent = null;
        this.sibling = null;
    }
}

/*
 *  Define a Merkle Proof node, used by Merkle Proof
 */
public class MerkleProofNode {
    // Node hash in hex string format
    public string hex_hash { get; set; }
    public MerkleTreeNode.ChildPosition pos { get; set; }
}

/*
 *  Define a MerkleTree data structure
 */
public class MerkleTree {
    // Define root node
    public MerkleTreeNode? root = null;
    // Used by leaf node hash tag
    public byte[] LeafHashTag = Array.Empty<byte>();
    // Used by branch node hash tag
    public byte[] BranchHashTag = Array.Empty<byte>();
    // Map the leaf node plain text to itself, helpful for generating merkle proofs
    Dictionary<string, MerkleTreeNode> entrances = new Dictionary<string, MerkleTreeNode>();

    /*
     *  Hash function for leaf node. Using tagged hash
     */
    byte[] LeafHashFun(byte[] data) {
        return Common.Hash.TagHash(data, this.LeafHashTag);
    }

    /*
     *  Hash function for branch node. Using tagged hash
     */
    byte[] BranchHashFun(byte[] data) {
        return Common.Hash.TagHash(data, this.BranchHashTag);
    }

    /*
     *  Build a new merkle tree given a list of strings and return its root hash
     *  @param payloads: string array. Each element is one leaf
     *  @param leaf_tag: string. Used for hashing in the leaf node
     *  @param branch_tag: string. Used for hashing in the branch (non-leaf) node
     *  @return: string. Denote hash of root node in hex string format
     */
    public string Build(string[] payloads, string leaf_tag, string branch_tag) {
        // Reset state
        this.Clear();

        this.LeafHashTag = Common.Serializer.Utf8Decoding(leaf_tag);
        this.BranchHashTag = Common.Serializer.Utf8Decoding(branch_tag);

        // Used for level order traversal when building the merkle tree
        Queue<MerkleTreeNode> order = new Queue<MerkleTreeNode>();

        // Generate all leaf nodes
        byte[] leaf_hash = Array.Empty<byte>();
        foreach (string payload in payloads) {
            leaf_hash = this.LeafHashFun(Common.Serializer.Utf8Decoding(payload));
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
            this.entrances.Add(payload, node);
        }

        // Edge case when there is odd number of leaf nodes, we duplicate the
        // last node to pair them together
        if (order.Count > 1 && (order.Count & 1) == 1) {
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
        }

        // Generate all branch node from bottom to top, level by level
        // At each level, pair two concecutive nodes from the front of the queue
        // If there are odd number of nodes, just leave the last node and scan
        // the next level
        while (order.Count > 1) {
            int level_size = order.Count;
            while (level_size > 1) {
                MerkleTreeNode left_child = order.Dequeue();
                MerkleTreeNode right_child = order.Dequeue();
                level_size -= 2;

                left_child.pos = MerkleTreeNode.ChildPosition.Left;
                right_child.pos = MerkleTreeNode.ChildPosition.Right;

                byte[] branch_hash = this.BranchHashFun(Common.Utility.ConcatBytes(left_child.hash, right_child.hash));
                MerkleTreeNode node = new MerkleTreeNode(branch_hash);

                left_child.parent = node;
                left_child.sibling = right_child;

                right_child.parent = node;
                right_child.sibling = left_child;

                order.Enqueue(node);
            }

            // Handle the odd nodes case
            if (level_size == 1) {
                order.Enqueue(order.Dequeue());
            }
        }

        // Return root hash
        if (order.Count == 1) {
            this.root = order.Dequeue();
            return Common.Serializer.HexEncoding(root.hash);
        }

        // Indicate something wrong happens
        return "";
    }

    /*
     *  Generate merkle proof given a query.
     *  If the query matches any leaf node plain text, return an array of
     *    pair(hash, pos) from closest to the matching leaf node up to the
     *    root where
     *      hash is hex string of the node
     *      pos indicate whether is a 0 (left child) or 1 (right child)
     *  @param query: string
     *  @return: MerkleProofNode[], format as the described above
     */
    public MerkleProofNode[] Proof(string query) {
        if (!this.entrances.ContainsKey(query)) {
            // No match found, return empty result
            return [];
        }

        List<MerkleProofNode> path = new();
        MerkleTreeNode node = this.entrances[query];
        while (node != null) {
            if (node.sibling == null || node.parent == null) {
                // Already reach root, leave loop
                break;
            }
            MerkleTreeNode proof_node = node.sibling;
            path.Add(
                new MerkleProofNode {
                    hex_hash = Serializer.HexEncoding(proof_node.hash),
                    pos = proof_node.pos
                }
            );
            node = node.parent;
        }

        return path.ToArray();
    }

    /*
     *  Reset the states
     */
    public void Clear() {
        this.root = null;
        this.LeafHashTag = Array.Empty<byte>();
        this.BranchHashTag = Array.Empty<byte>();
        this.entrances.Clear();
    }

    /*
     *  Show the tree structure, for debug purpose
     */
    public void Show() {
        Console.WriteLine("============================================================");
        foreach (KeyValuePair<string, MerkleTreeNode> kv in this.entrances) {
            MerkleTreeNode node = kv.Value;
            Console.Write("{0}", Common.Serializer.HexEncoding(node.hash));
            while (node.parent != null) {
                Console.Write(" -> {0}", Common.Serializer.HexEncoding(node.parent.hash));
                node = node.parent;
            }
            Console.WriteLine();
        }
        Console.WriteLine("============================================================");
    }
}