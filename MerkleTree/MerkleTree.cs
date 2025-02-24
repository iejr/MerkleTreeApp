namespace MerkleTree;

public class MerkleTreeNode {
    public byte[] hash;
    public MerkleTreeNode? parent;
    public MerkleTreeNode? sibling;

    public MerkleTreeNode (byte[] hash) {
        this.hash = hash;
        this.parent = null;
        this.sibling = null;
    }
}

public class MerkleTree {
    public MerkleTreeNode? root = null;
    public byte[] LeafHashTag = Array.Empty<byte>();
    public byte[] BranchHashTag = Array.Empty<byte>();
    Dictionary<string, MerkleTreeNode> entrances = new Dictionary<string, MerkleTreeNode>();

    byte[] LeafHashFun(byte[] data) {
        return Common.Hash.TagHash(data, this.LeafHashTag);
    }

    byte[] BranchHashFun(byte[] data) {
        return Common.Hash.TagHash(data, this.BranchHashTag);
    }

    public string Build(string[] payloads, string leaf_tag, string branch_tag) {
        this.Clear();

        this.LeafHashTag = Common.Serializer.Utf8Decoding(leaf_tag);
        this.BranchHashTag = Common.Serializer.Utf8Decoding(branch_tag);

        Queue<MerkleTreeNode> order = new Queue<MerkleTreeNode>();

        byte[] leaf_hash = Array.Empty<byte>();
        foreach (string payload in payloads) {
            leaf_hash = this.LeafHashFun(Common.Serializer.Utf8Decoding(payload));
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
            this.entrances.Add(payload, node);
        }

        if (order.Count > 1 && (order.Count & 1) == 1) {
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
        }

        while (order.Count > 1) {
            int level_size = order.Count;
            while (level_size > 1) {
                MerkleTreeNode left_child = order.Dequeue();
                MerkleTreeNode right_child = order.Dequeue();
                level_size -= 2;

                byte[] branch_hash = this.BranchHashFun(Common.Utility.ConcatBytes(left_child.hash, right_child.hash));
                MerkleTreeNode node = new MerkleTreeNode(branch_hash);

                left_child.parent = node;
                left_child.sibling = right_child;

                right_child.parent = node;
                right_child.sibling = left_child;

                order.Enqueue(node);
            }

            if (level_size == 1) {
                order.Enqueue(order.Dequeue());
            }
        }

        if (order.Count == 1) {
            this.root = order.Dequeue();
            return Common.Serializer.HexEncoding(root.hash);
        }
        return "";
    }

    public void Clear() {
        this.root = null;
        this.LeafHashTag = Array.Empty<byte>();
        this.BranchHashTag = Array.Empty<byte>();
        this.entrances.Clear();
    }

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