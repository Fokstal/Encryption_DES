using System;

namespace DES
{
    public class SDES
    {
        private static string[,] blockS1 = new string[,] { { "0", "00", "01", "10", "11", },
                                                          { "00", "1", "0", "3", "2", },
                                                          { "01", "3", "2", "1", "0", },
                                                          { "10", "0", "2", "1", "3", },
                                                          { "11", "3", "1", "3", "1", },};

        private static string[,] blockS2 = new string[,] { { "0", "00", "01", "10", "11", },
                                                          { "00", "1", "1", "2", "3", },
                                                          { "01", "2", "0", "1", "3", },
                                                          { "10", "3", "0", "1", "0", },
                                                          { "11", "2", "1", "0", "3", },};

        private static int[] AdditionalBinary(int[] firstCodeBinary, int[] secondCodeBinary)
        {
            int[] result = new int[firstCodeBinary.Length];

            for (int i = result.Length - 1; i > -1; i--)
            {
                int sum = firstCodeBinary[i] + secondCodeBinary[i];

                switch (sum)
                {
                    case 2:
                        sum = 0;
                        break;
                }

                result[i] = sum;
            }

            return result;
        }

        private static int[] ConvertToBinary(int numbers)
        {
            int[] result;

            switch (numbers)
            {
                case 0:
                    result = new int[] { 0, 0 };
                    break;
                case 1:
                    result = new int[] { 0, 1 };
                    break;
                case 2:
                    result = new int[] { 1, 0 };
                    break;
                case 3:
                    result = new int[] { 1, 1 };
                    break;
                default:
                    result = new int[] { 0 };
                    break;
            }

            return result;
        }

        private static int[] SwapIP(int[] input, int numberSwap)
        {
            int[] swapMas1 = new int[] { 2, 6, 3, 1, 4, 8, 5, 7 };
            int[] swapMas2 = new int[] { 4, 1, 3, 5, 7, 2, 8, 6 };
            int[] swapMasF = new int[] { 4, 1, 2, 3, 2, 3, 4, 1 };
            int[] swapF = new int[] { 2, 4, 3, 1 };
            int len;

            if (numberSwap == 4) len = 4;
            else len = 8;

            int[] output = new int[len];

            switch (numberSwap)
            {
                case 1:
                    for (int i = 0; i < input.Length; i++)
                    {
                        for (int j = 0; j < swapMas1.Length; j++)
                        {
                            if (i + 1 == swapMas1[j])
                            {
                                output[j] = input[i];
                            }
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < input.Length; i++)
                    {
                        for (int j = 0; j < swapMas2.Length; j++)
                        {
                            if (i + 1 == swapMas2[j])
                            {
                                output[j] = input[i];
                            }
                        }
                    }
                    break;

                case 3:
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < swapMasF.Length; j++)
                        {
                            if (i < 4)
                            {
                                if (i + 1 == swapMasF[j])
                                {
                                    output[j] = input[i];
                                }
                            }
                            else
                            {
                                if (i + 1 - 4 == swapMasF[j])
                                {
                                    output[j] = input[i - 4];
                                }
                            }
                        }
                    }
                    break;

                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < swapF.Length; j++)
                        {
                            if (i < 4)
                            {
                                if (i + 1 == swapF[j])
                                {
                                    output[j] = input[i];
                                }
                            }
                            else
                            {
                                if (i + 1 - 4 == swapF[j])
                                {
                                    output[j] = input[i - 4];
                                }
                            }
                        }
                    }
                    break;
            }



            return output;
        }

        private static int[] SwapBlock(int[] inputFourBit, string[,] typeBlockS)
        {
            int pozLine = 0;
            int pozColumn = 0;
            string line = Convert.ToString(inputFourBit[0]) + Convert.ToString(inputFourBit[3]);
            string column = Convert.ToString(inputFourBit[1]) + Convert.ToString(inputFourBit[2]);

            for (int j = 0; j < typeBlockS.GetLength(0); j++)
            {
                if (line == typeBlockS[0, j])
                {
                    pozLine = j;
                    for (int i = 0; i < typeBlockS.GetLength(1); i++)
                    {
                        if (column == typeBlockS[i, 0])
                        {
                            pozColumn = i;
                            break;
                        }
                    }
                }

            }

            return ConvertToBinary(Convert.ToInt32(typeBlockS[pozLine, pozColumn]));
        }

        public static int[,] Work(int[,] input, int[] k1, int[] k2)
        {
            int[,] resultOutput = new int[input.GetLength(0), input.GetLength(1)];

            for (int l = 0; l < input.GetLength(0); l++)
            {
                int[] work = new int[input.GetLength(1)];

                for (int i = 0; i < work.Length; i++)
                {
                    work[i] = input[l, i];
                }


                int[] result = SwapIP(work, 1);
                int len = work.Length / 2;
                int[] leftPart = new int[len];
                int[] rightPart = new int[len];

                for (int i = 0; i < len; i++)
                {
                    leftPart[i] = result[i];
                    rightPart[i] = result[i + 4];
                }

                int[] rightStart = rightPart;
                int[] leftStart = leftPart;
                rightPart = FunctionSDES(k1, rightPart, blockS1);

                leftPart = AdditionalBinary(leftPart, rightPart);
                leftPart = FunctionSDES(k2, leftPart, blockS2);

                leftPart = AdditionalBinary(leftPart, rightStart);
                rightPart = AdditionalBinary(rightPart, leftStart);

                for (int i = 0; i < result.Length; i++)
                {
                    if (i < 4)
                    {
                        result[i] = leftPart[i];
                    }
                    else
                    {
                        result[i] = rightPart[i - 4];
                    }

                }

                result = SwapIP(result, 2);

                for (int i = 0; i < resultOutput.GetLength(1); i++)
                {
                    resultOutput[l, i] = result[i];
                }
            }

            return resultOutput;
        }

        private static int[] FunctionSDES(int[] key, int[] input, string[,] typeBlockS)
        {
            int[] workEightBit = SwapIP(input, 3);
            int[] summaryBit = AdditionalBinary(key, workEightBit);
            int[] result = new int[4];
            int len = summaryBit.Length / 2;
            int[] left = new int[len];
            int[] right = new int[len];

            for (int i = 0; i < len; i++)
            {
                left[i] = summaryBit[i];
                right[i] = summaryBit[i + 4];
            }

            for (int i = 0; i < result.Length; i++)
            {
                if (i < 2)
                {
                    result[i] = SwapBlock(left, blockS1)[i];
                }
                else
                {
                    result[i] = SwapBlock(right, blockS2)[i - 2];
                }

            }

            result = SwapIP(result, 4);

            return result;
        }
    }
}