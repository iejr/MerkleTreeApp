namespace Common;

public class Utility {

    public static byte[] ConcatBytes(byte[] array1, byte[] array2) {
        byte[] buffer = new byte[array1.Length + array2.Length];
        System.Buffer.BlockCopy(array1, 0, buffer, 0, array1.Length);
        System.Buffer.BlockCopy(array2, 0, buffer, array1.Length, array2.Length);
        return buffer;
    }

    public static byte[] Concat3Bytes(byte[] array1, byte[] array2, byte[] array3) {
        byte[] buffer = new byte[array1.Length + array2.Length + array3.Length];
        System.Buffer.BlockCopy(array1, 0, buffer, 0, array1.Length);
        System.Buffer.BlockCopy(array2, 0, buffer, array1.Length, array2.Length);
        System.Buffer.BlockCopy(array3, 0, buffer, array1.Length + array2.Length, array3.Length);
        return buffer;
    }

}