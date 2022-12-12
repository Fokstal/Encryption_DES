using System;
using System.Text;

namespace DES
{
    public class Encryptions
    {
        private int[] Key = new int[] { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0 };
        private static int[] K1;
        private static int[] K2;

        private static int[] TransformByteToBinary(int number)
        {
            int[] binary = new int[8];
            int work = number;

            if (number < 64)
            {
                binary[0] = 0;
                binary[1] = 0;
                binary[2] = 1;
            }
            else
            {
                binary[0] = 0;
                binary[1] = 1;
            }

            for (int i = binary.Length - 1; i > -1; i--)
            {
                int particle = work / 2;
                int particleBinary = work - particle * 2;

                binary[i] = particleBinary;
                work = particle;
            }

            return binary;
        }

        private static byte TransformBinaryToNumber(int[] codeBinary)
        {
            byte number = 0;
            int pow = codeBinary.Length;

            for (int i = 0; i < codeBinary.Length; i++)
            {
                pow--;
                number += Convert.ToByte(codeBinary[i] * Convert.ToInt32(Math.Pow(2, pow)));
            }

            return number;
        }

        private static int[,] Create_CodeBinary(byte[] code)
        {
            int[,] binaryCode = new int[code.Length, 8];

            for (int i = 0; i < binaryCode.GetLength(0); i++)
            {
                for (int j = 0; j < binaryCode.GetLength(1); j++)
                {
                    int[] work = TransformByteToBinary(code[i]);

                    binaryCode[i, j] = work[j];
                }
            }

            return binaryCode;
        }

        private static byte[] Create_CodeByte(int[,] code)
        {
            byte[] byteCode = new byte[code.GetLength(0)];

            for (int i = 0; i < code.GetLength(0); i++)
            {
                int[] work = new int[code.GetLength(1)];

                for (int j = 0; j < code.GetLength(1); j++)
                {
                    work[j] = code[i, j];
                }

                byteCode[i] = TransformBinaryToNumber(work);
            }

            return byteCode;
        }

        private static string Encryption(string word)
        {
            UnicodeEncoding ue = new UnicodeEncoding();

            byte[] byte_Word = ue.GetBytes(word);
            int[,] binary_Word = Create_CodeBinary(byte_Word);

            int[,] binary_Encryption = SDES.Work(binary_Word, K1, K2);
            byte[] byte_Encryption = Create_CodeByte(binary_Encryption);
            string encryption = ue.GetString(byte_Encryption);

            return encryption;
        }

        private static string Decryption(string encryption)
        {
            UnicodeEncoding ue = new UnicodeEncoding();

            byte[] byte_Encryption = ue.GetBytes(encryption);
            int[,] binary_Encryption = Create_CodeBinary(byte_Encryption);

            int[,] binary_Decryption = SDES.Work(binary_Encryption, K2, K1);
            byte[] byte_Decryption = Create_CodeByte(binary_Decryption);
            string decryption = ue.GetString(byte_Decryption);

            return decryption;
        }

        public string Encryption_Word { get; private set; }
        public string Decryption_Word { get; private set; }

        public Encryptions(string word)
        {
            Key_SDES.Start(Key);
            K1 = Key_SDES.K1;
            K2 = Key_SDES.K2;

            Encryption_Word = Encryption(word);
            Decryption_Word = Decryption(Encryption_Word);
        }
    }
}
