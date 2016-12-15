using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Complex;
using SubstationLSE.Measurements;
using SubstationLSE.Topology;

namespace SubstationLSE.Algorithm
{
    public static class LSECalculation
    {
        public static HashSet<int> CalculateLSE(DenseMatrix H, DenseMatrix W, DenseMatrix Z, out DenseMatrix X, double threshold, bool doBadDataDetection)
        {
            HashSet<int> badDataList = new HashSet<int>();
            int bad_Data = -1;
            int length_Z = Z.RowCount;
            int length_X = H.ColumnCount;
            int eliminate = 0;
            IList<Tuple<int, int, Complex>> H_Changed = new List<Tuple<int, int, Complex>>();
            X = new DenseMatrix(length_X, 1); ;

            if (length_Z < length_X)
            {
                Console.WriteLine("unsolveable island");
                return badDataList; 
            }

            while(true)
            {
                if (bad_Data != -1)
                {
                    int index = bad_Data;
                    eliminate++;
                    Z[index, 0] = 0;
                    Vector<Complex> H_badRow = H.Row(index);
                    foreach (Tuple<int, Complex> badElement in H_badRow.GetIndexedEnumerator())
                    {
                        Tuple<int, int, Complex> changeMeasurement = Tuple.Create(index, badElement.Item1, badElement.Item2);
                        H_Changed.Add(changeMeasurement);
                    }
                    foreach (Tuple<int, int, Complex> change in H_Changed)
                    {
                        H[change.Item1, change.Item2] = 0;
                    }
                }
                if (badDataList.Count == 0)
                {
                    //System.Diagnostics.Stopwatch m_stopwatch = new System.Diagnostics.Stopwatch();
                    DenseMatrix H_Transpose = H.Transpose() as DenseMatrix;
                    DenseMatrix P = (((H_Transpose * W * H).Inverse()) * H_Transpose * W) as DenseMatrix;
                    X = P * Z;

                    if (doBadDataDetection)
                    {
                        bad_Data = Bad_Data_Detection(H, W, Z, threshold, X, eliminate, P);
                    }
                }
                else
                {
                    DenseMatrix HTranspose = (DenseMatrix)H.Transpose();
                    DenseMatrix PsuedoInverseOfMatrix = (((HTranspose * W * H).Inverse()) * HTranspose * W) as DenseMatrix;
                    X = PsuedoInverseOfMatrix * Z;
                    bad_Data = Bad_Data_Detection(H, W, Z, threshold, X, eliminate, PsuedoInverseOfMatrix);
                }
                if (bad_Data == -1)
                {
                    foreach (Tuple<int, int, Complex> change in H_Changed)
                    {
                        H[change.Item1, change.Item2] = change.Item3;
                    }
                    break;
                }
                else
                {
                    if (!badDataList.Contains(bad_Data))
                    {
                        badDataList.Add(bad_Data);
                    }
                }
            }

            return badDataList;
        }

        public static int Bad_Data_Detection(DenseMatrix H, DenseMatrix W, DenseMatrix Z, double threshold, DenseMatrix X, int eliminate, DenseMatrix PsuedoInverse)
        {
            int X_length = H.ColumnCount;
            int Z_length = Z.RowCount;

            DenseMatrix Z_Estimated = H * X;
            DenseMatrix residue = Z_Estimated - Z;
            //DenseMatrix temp = (H.Transpose() * R * H).Inverse() * H.Transpose() * R as DenseMatrix;
            Complex[] K = new Complex[Z_length];
            Complex[] K1 = new Complex[Z_length];
            foreach (Tuple<int, int, Complex> element in H.IndexedEnumerator())
            {
                if (element.Item3 != 0)
                {
                    int row = element.Item1;
                    int column = element.Item2;
                    K[row] += element.Item3 * PsuedoInverse[column, row];
                    //K1[row] += element.Item3 * temp[column, row];
                }
            }
            double[] Omiga = new double[Z_length];
            double[] Omiga1 = new double[Z_length];
            for (int i = 0; i < Z_length; i++)
            {
                Omiga[i] = ((1 - K[i]) / W[i, i]).Magnitude;
                //Omiga[i] = (1 - K1[i].Magnitude) / R[i, i].Magnitude;
            }
            int bad_data = -1;
            double[] standardErrorEstimated = new double[Z_length];
            for (int m = 0; m < Z_length; m++)
            {
                if (Math.Abs(Omiga[m]) < 0.00000001)
                {
                    standardErrorEstimated[m] = 0;
                }
                else
                {
                    standardErrorEstimated[m] = residue[m, 0].Magnitude / Math.Sqrt(Omiga[m]);
                }
            }
            double standardErrorEstimatedTemp;
            int counter = 0;
            if (Z_length - X_length >= eliminate)
            {
                standardErrorEstimatedTemp = 0;
                for (int n = 0; n < Z_length; n++)
                {
                    if (standardErrorEstimated[n] > standardErrorEstimatedTemp)
                    {
                        standardErrorEstimatedTemp = standardErrorEstimated[n];
                        counter = n;
                    }
                }
                bad_data = counter;
                if (standardErrorEstimated[counter] < threshold)
                    bad_data = -1;
            }
            else
            {
                Console.WriteLine("Stop bad data detection");
            }
            return bad_data;
        }
    }
}
