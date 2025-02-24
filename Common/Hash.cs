using System.Security.Cryptography;

namespace Common;

public class Hash {

    public static byte[] Sha256Hash(byte[] data) {
        SHA256 manager = SHA256.Create();
        return manager.ComputeHash(data);
    }

    public static byte[] TagHash(byte[] data, byte[] tag) {
        byte[] tag_hash = Sha256Hash(tag);
        byte[] buffer = Utility.Concat3Bytes(tag_hash, tag_hash, Sha256Hash(data));
        return Sha256Hash(buffer);
    }
}
