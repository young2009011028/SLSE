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
    class CurrentEstimator
    {
        #region [ Private Members ]

        private Dictionary<string, Dictionary<string, BreakerCurrentPhasorGroup>> m_islandBreakerCurrentMeasurements = new Dictionary<string, Dictionary<string, BreakerCurrentPhasorGroup>>();
        private Dictionary<string, Dictionary<string, CurrentPhasorGroup>> m_islandCurrentMeasurements = new Dictionary<string, Dictionary<string, CurrentPhasorGroup>>();

        private Dictionary<string, DenseMatrix> m_island_CB = new Dictionary<string, DenseMatrix>();
        private Dictionary<string, DenseMatrix> m_island_EQ = new Dictionary<string, DenseMatrix>();
        private Dictionary<string, DenseMatrix> m_island_W = new Dictionary<string, DenseMatrix>();
        private Dictionary<string, DenseMatrix> m_island_Z = new Dictionary<string, DenseMatrix>();

        //private DenseMatrix m_H = DenseMatrix.OfArray(new Complex[1, 1]);
        //private DenseMatrix m_I = DenseMatrix.OfArray(new Complex[1, 1]);
        //private DenseMatrix m_W = DenseMatrix.OfArray(new Complex[1, 1]);
        private Dictionary<string, HashSet<int>> m_badDataList = new Dictionary<string, HashSet<int>>();
        private Dictionary<string, Dictionary<int, string>> m_measurementList = new Dictionary<string, Dictionary<int, string>>();

        private Dictionary<string, BreakerCurrentPhasorGroup> m_activeBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();
        private Dictionary<string, CurrentPhasorGroup> m_activeCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
        private TopologyProcessor m_topologyProcessor = new TopologyProcessor();

        #endregion

        #region [ Constructor ]

        public CurrentEstimator(Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements, Dictionary<string, CurrentPhasorGroup> activeCurrentMeasurements, TopologyProcessor topologyProcessor)
        {
            m_activeBreakerCurrentMeasurements = activeBreakerCurrentMeasurements;
            m_activeCurrentMeasurements = activeCurrentMeasurements;
            m_topologyProcessor = topologyProcessor;
        }

        #endregion

        #region [ Properties ]

        public Dictionary<string, Dictionary<string, BreakerCurrentPhasorGroup>> IslandBreakerCurrentMeasurements
        {
            get
            {
                return m_islandBreakerCurrentMeasurements;
            }
            set
            {
                m_islandBreakerCurrentMeasurements = value;
            }
        }

        public Dictionary<string, Dictionary<string, CurrentPhasorGroup>> IslandCurrentMeasurements
        {
            get
            {
                return m_islandCurrentMeasurements;
            }
            set
            {
                m_islandCurrentMeasurements = value;
            }
        }

        public Dictionary<string, HashSet<int>> BadDataList
        {
            get
            {
                return m_badDataList;
            }
            set
            {
                m_badDataList = value;
            }
        }

        public Dictionary<string, Dictionary<int, string>> MeasurementList
        {
            get
            {
                return m_measurementList;
            }
            set
            {
                m_measurementList = value;
            }
        }

        #endregion

        #region [ Private Methods ]

        private void DistributeMeasurements()
        {
            m_islandBreakerCurrentMeasurements.Clear();
            m_islandCurrentMeasurements.Clear();

            foreach(KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                Dictionary<string, BreakerCurrentPhasorGroup> islandActiveBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();
                foreach(KeyValuePair<string, BreakerCurrentPhasorGroup> kw in m_activeBreakerCurrentMeasurements)
                {
                    if (kv.Value.m_Edge_Vertex.ContainsKey(kw.Key))
                    {
                        islandActiveBreakerCurrentMeasurements.Add(kw.Key, kw.Value);
                    }
                }
                m_islandBreakerCurrentMeasurements.Add(kv.Key, islandActiveBreakerCurrentMeasurements);
                
                Dictionary<string, CurrentPhasorGroup> islandActiveCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
                foreach (KeyValuePair<string, CurrentPhasorGroup> ku in m_activeCurrentMeasurements)
                {
                    if (kv.Value.ContainsKey(ku.Key))
                    {
                        islandActiveCurrentMeasurements.Add(ku.Key, ku.Value);
                    }
                }
                m_islandCurrentMeasurements.Add(kv.Key, islandActiveCurrentMeasurements);
            }
        }

        private void CurrentLSEFormulation()
        {
            foreach (KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_islandBreakerCurrentMeasurements[kv.Key] == null || m_islandBreakerCurrentMeasurements[kv.Key].Count < 1)
                {
                    Console.WriteLine("Current State Estimator: unobservable island");
                    continue;
                }

                Dictionary<string, BreakerCurrentPhasorGroup> islandActiveBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();
                if (m_islandBreakerCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveBreakerCurrentMeasurements = m_islandBreakerCurrentMeasurements[kv.Key];
                }
                DenseMatrix m_CB_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                List<BreakerCurrentPhasorGroup> m_islandActiveBreakerCurrentMeasurements = islandActiveBreakerCurrentMeasurements.Values.ToList();
                m_CB_temp = DenseMatrix.OfArray(new Complex[m_islandActiveBreakerCurrentMeasurements.Count, m_islandActiveBreakerCurrentMeasurements.Count]);
                foreach (BreakerCurrentPhasorGroup breakerCurrentPhasorGroup in m_islandActiveBreakerCurrentMeasurements)
                {
                    m_CB_temp[m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup), m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup)] = new Complex(1, 0);
                }
                m_island_CB.Add(kv.Key, m_CB_temp);

                Dictionary<string, CurrentPhasorGroup> islandActiveCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
                if (m_islandCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveCurrentMeasurements = m_islandCurrentMeasurements[kv.Key];
                }

                foreach(KeyValuePair<string, CurrentPhasorGroup> kw in islandActiveCurrentMeasurements)
                {
                    //kw.Value.m
                    if (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerOne) || (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerTwo)))
                    {
                        islandActiveCurrentMeasurements.Remove(kw.Key);
                        m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
                    }

                    //if (kv.Value.ContainsKey(kw.Key))
                    //{
                    //    HashSet<string> edges = new HashSet<string>();
                    //    edges = kv.Value[kw.Key];       
                    //    foreach (string ku in edges)
                    //    {
                    //        if (!islandActiveBreakerCurrentMeasurements.ContainsKey(ku))
                    //        {
                    //            islandActiveCurrentMeasurements.Remove(kw.Key);
                    //            m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
                    //        }
                    //    }
                    //}
                }

                DenseMatrix m_EQ_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_EQ_temp = DenseMatrix.OfArray(new Complex[islandActiveCurrentMeasurements.Count, islandActiveBreakerCurrentMeasurements.Count]);

                List<CurrentPhasorGroup> m_islandActiveCurrentMeasurements = islandActiveCurrentMeasurements.Values.ToList();

                foreach (CurrentPhasorGroup EQ_Current in m_islandActiveCurrentMeasurements)
                {
                    if (islandActiveBreakerCurrentMeasurements.ContainsKey(EQ_Current.MeasuredBreakerOne))
                    {
                        if (EQ_Current.MeasuredBreakerOneDirection.Equals("Positive"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerOneDirection.Equals("Negative"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(-1, 0);
                        }
                        else
                        {
                            Console.WriteLine("No breaker current direction assigned:     " + EQ_Current.MeasuredBreakerOne);
                        }
                    }

                    if (islandActiveBreakerCurrentMeasurements.ContainsKey(EQ_Current.MeasuredBreakerTwo))
                    {
                        if (EQ_Current.MeasuredBreakerTwoDirection.Equals("Positive"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerTwoDirection.Equals("Negative"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(-1, 0);
                        }
                        else
                        {
                            Console.WriteLine("No breaker current direction assigned:      " + EQ_Current.MeasuredBreakerTwo);
                        }
                    }

                    //HashSet<string> edges = new HashSet<string>();
                    //edges = kv.Value[EQ_Current.Key];  
                    //foreach(string edge in edges)
                    //{
                    //    if (islandActiveBreakerCurrentMeasurements.ContainsKey(edge))
                    //    {
                    //        if (islandActiveBreakerCurrentMeasurements[edge].FromNodeID.Equals(EQ_Current.Key))
                    //        {
                    //            // "-"
                    //            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(-1, 0);
                    //        }
                    //        if (islandActiveBreakerCurrentMeasurements[edge].ToNodeID.Equals(EQ_Current.Key))
                    //        {
                    //            // "+"
                    //            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(1, 0);
                    //        }
                    //    }
                    //}
                }
                m_island_EQ.Add(kv.Key, m_EQ_temp);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count, m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count]);
                for (int i=0; i<m_W_temp.RowCount; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        m_W_temp[i, i] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.MeasurementCovariance, 0);
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        m_W_temp[i, i] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.MeasurementCovariance), 0);
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count, 1]);

                for (int i = 0; i < m_Z_temp.RowCount; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        measurementList_temp.Add(i, m_islandActiveBreakerCurrentMeasurements[i].MeasuredBreakerID);
                        m_Z_temp[i, 0] = m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        measurementList_temp.Add(i, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].MeasuredNodeID);
                        m_Z_temp[i, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_measurementList.Add(kv.Key, measurementList_temp);
                m_island_Z.Add(kv.Key, m_Z_temp);
            }
        }

        private void SolveLSE()
        {
            m_badDataList.Clear();
            foreach(KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_island_CB == null || !m_island_CB.ContainsKey(kv.Key))
                {
                    Console.WriteLine("not enough breaker current");
                    continue;
                }
                HashSet<int> badDataList = new HashSet<int>();
                DenseMatrix X;
                DenseMatrix Z;
                DenseMatrix H;
                DenseMatrix W;
                DenseMatrix P;

                Z = m_island_Z[kv.Key];
                H = MatrixCalculationExtensions.VerticallyConcatenate(m_island_CB[kv.Key], m_island_EQ[kv.Key]);
                W = m_island_W[kv.Key];
                badDataList = LSECalculation.CalculateLSE(H, W, Z, out X, 10, true);
                m_badDataList.Add(kv.Key, badDataList);

                if (X.RowCount>0)
                {
                    List<BreakerCurrentPhasorGroup> islandBreakerCurrentMeasurements = m_islandBreakerCurrentMeasurements[kv.Key].Values.ToList();
                    for(int i=0; i<islandBreakerCurrentMeasurements.Count;i++)
                    {
                        islandBreakerCurrentMeasurements[i].PositiveSequence.Estimate.PerUnitComplexPhasor = X[i, 0];
                    }

                    Dictionary<string, CurrentPhasorGroup> islandCurrentMeasurements = m_islandCurrentMeasurements[kv.Key];

                    foreach (KeyValuePair<string, CurrentPhasorGroup> EQ_Current in islandCurrentMeasurements)
                    {
                        Complex I1 = new Complex();
                        Complex I2 = new Complex();
                        CurrentPhasorGroup currentMeasurement = EQ_Current.Value;
                        if (m_islandBreakerCurrentMeasurements[kv.Key].ContainsKey(currentMeasurement.MeasuredBreakerOne))
                        {
                            if (currentMeasurement.MeasuredBreakerOneDirection.Equals("Positive"))
                            {
                                I1 = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else if (currentMeasurement.MeasuredBreakerOneDirection.Equals("Negative"))
                            {
                                I1 = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else
                            {
                                Console.WriteLine("breaker current mapping error at:     " + EQ_Current.Key);
                            }
                        }
                        if (m_islandBreakerCurrentMeasurements[kv.Key].ContainsKey(currentMeasurement.MeasuredBreakerTwo))
                        {
                            if (currentMeasurement.MeasuredBreakerTwoDirection.Equals("Positive"))
                            {
                                I1 = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else if (currentMeasurement.MeasuredBreakerTwoDirection.Equals("Negative"))
                            {
                                I1 = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else
                            {
                                Console.WriteLine("breaker current mapping error at:     " + EQ_Current.Key);
                            }
                        }
                        //HashSet<string> edges = new HashSet<string>();
                        //edges = kv.Value[EQ_Current.Key];
                        //foreach (string edge in edges)
                        //{
                        //    if (m_islandBreakerCurrentMeasurements[kv.Key].ContainsKey(edge))
                        //    {
                        //        if (m_islandBreakerCurrentMeasurements[kv.Key][edge].FromNodeID.Equals(EQ_Current.Key))
                        //        {
                        //            // "-"
                        //            I1 = (-1) * (m_islandBreakerCurrentMeasurements[kv.Key][edge].PositiveSequence.Estimate.PerUnitComplexPhasor);
                        //            // m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(-1, 0);
                        //        }
                        //        if (m_islandBreakerCurrentMeasurements[kv.Key][edge].ToNodeID.Equals(EQ_Current.Key))
                        //        {
                        //            // "+"
                        //            I2 = m_islandBreakerCurrentMeasurements[kv.Key][edge].PositiveSequence.Estimate.PerUnitComplexPhasor;
                        //            // m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(1, 0);
                        //        }
                        //    }
                        //}
                        EQ_Current.Value.PositiveSequence.Estimate.PerUnitComplexPhasor = I1 + I2;
                    }
                }
            }
        }

        private void ThreePhaseCurrentLSEFormulation()
        {
            foreach (KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_islandBreakerCurrentMeasurements[kv.Key] == null || m_islandBreakerCurrentMeasurements[kv.Key].Count < 1)
                {
                    Console.WriteLine("Current State Estimator: unobservable island");
                    continue;
                }

                Dictionary<string, BreakerCurrentPhasorGroup> islandActiveBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();
                if (m_islandBreakerCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveBreakerCurrentMeasurements = m_islandBreakerCurrentMeasurements[kv.Key];
                }
                DenseMatrix m_CB_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                List<BreakerCurrentPhasorGroup> m_islandActiveBreakerCurrentMeasurements = islandActiveBreakerCurrentMeasurements.Values.ToList();
                m_CB_temp = DenseMatrix.OfArray(new Complex[3 * m_islandActiveBreakerCurrentMeasurements.Count, 3 * m_islandActiveBreakerCurrentMeasurements.Count]);
                foreach (BreakerCurrentPhasorGroup breakerCurrentPhasorGroup in m_islandActiveBreakerCurrentMeasurements)
                {
                    m_CB_temp[m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup), m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup)] = new Complex(1, 0);
                }
                m_island_CB.Add(kv.Key, m_CB_temp);

                Dictionary<string, CurrentPhasorGroup> islandActiveCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
                if (m_islandCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveCurrentMeasurements = m_islandCurrentMeasurements[kv.Key];
                }

                foreach (KeyValuePair<string, CurrentPhasorGroup> kw in islandActiveCurrentMeasurements)
                {
                    //kw.Value.m
                    if (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerOne) || (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerTwo)))
                    {
                        islandActiveCurrentMeasurements.Remove(kw.Key);
                        m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
                    }

                    //if (kv.Value.ContainsKey(kw.Key))
                    //{
                    //    HashSet<string> edges = new HashSet<string>();
                    //    edges = kv.Value[kw.Key];       
                    //    foreach (string ku in edges)
                    //    {
                    //        if (!islandActiveBreakerCurrentMeasurements.ContainsKey(ku))
                    //        {
                    //            islandActiveCurrentMeasurements.Remove(kw.Key);
                    //            m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
                    //        }
                    //    }
                    //}
                }

                DenseMatrix m_EQ_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_EQ_temp = DenseMatrix.OfArray(new Complex[islandActiveCurrentMeasurements.Count, islandActiveBreakerCurrentMeasurements.Count]);

                List<CurrentPhasorGroup> m_islandActiveCurrentMeasurements = islandActiveCurrentMeasurements.Values.ToList();

                foreach (CurrentPhasorGroup EQ_Current in m_islandActiveCurrentMeasurements)
                {
                    if (islandActiveBreakerCurrentMeasurements.ContainsKey(EQ_Current.MeasuredBreakerOne))
                    {
                        if (EQ_Current.MeasuredBreakerOneDirection.Equals("Positive"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerOneDirection.Equals("Negative"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(-1, 0);
                        }
                        else
                        {
                            Console.WriteLine("No breaker current direction assigned:     " + EQ_Current.MeasuredBreakerOne);
                        }
                    }

                    if (islandActiveBreakerCurrentMeasurements.ContainsKey(EQ_Current.MeasuredBreakerTwo))
                    {
                        if (EQ_Current.MeasuredBreakerTwoDirection.Equals("Positive"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerTwoDirection.Equals("Negative"))
                        {
                            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(-1, 0);
                        }
                        else
                        {
                            Console.WriteLine("No breaker current direction assigned:      " + EQ_Current.MeasuredBreakerTwo);
                        }
                    }

                    //HashSet<string> edges = new HashSet<string>();
                    //edges = kv.Value[EQ_Current.Key];  
                    //foreach(string edge in edges)
                    //{
                    //    if (islandActiveBreakerCurrentMeasurements.ContainsKey(edge))
                    //    {
                    //        if (islandActiveBreakerCurrentMeasurements[edge].FromNodeID.Equals(EQ_Current.Key))
                    //        {
                    //            // "-"
                    //            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(-1, 0);
                    //        }
                    //        if (islandActiveBreakerCurrentMeasurements[edge].ToNodeID.Equals(EQ_Current.Key))
                    //        {
                    //            // "+"
                    //            m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current.Value), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[edge])] = new Complex(1, 0);
                    //        }
                    //    }
                    //}
                }
                m_island_EQ.Add(kv.Key, m_EQ_temp);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count, m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count]);
                for (int i = 0; i < m_W_temp.RowCount; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        m_W_temp[i, i] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.MeasurementCovariance, 0);
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        m_W_temp[i, i] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.MeasurementCovariance), 0);
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count, 1]);

                for (int i = 0; i < m_Z_temp.RowCount; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        measurementList_temp.Add(i, m_islandActiveBreakerCurrentMeasurements[i].MeasuredBreakerID);
                        m_Z_temp[i, 0] = m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        measurementList_temp.Add(i, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].MeasuredNodeID);
                        m_Z_temp[i, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_measurementList.Add(kv.Key, measurementList_temp);
                m_island_Z.Add(kv.Key, m_Z_temp);
            }
        }

        private void SolveThreePhaseLSE()
        {

        }

        private DenseMatrix GetPositiveSequenceMeasurementVector(List<BreakerCurrentPhasorGroup> islandActiveBreakerCurrentMeasurements, List<CurrentPhasorGroup> islandActiveCurrentMeasurements)
        {

            return null;
        }

        private void BuildMatrices(Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements, Dictionary<string, CurrentPhasorGroup> activeCurrentMeasurements, TopologyProcessor topologyProcessor)
        {
            BuildCBMatrix(activeBreakerCurrentMeasurements, topologyProcessor);
            BuildEQMatrix(activeBreakerCurrentMeasurements, activeCurrentMeasurements, topologyProcessor);
        }

        private void BuildCBMatrix(Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements, TopologyProcessor topologyProcessor)
        {
            foreach(KeyValuePair<string, Tree> kv in topologyProcessor)
            {
                Dictionary<string, BreakerCurrentPhasorGroup> islandActiveBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();
                if (m_islandBreakerCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveBreakerCurrentMeasurements = m_islandBreakerCurrentMeasurements[kv.Key];
                }
                DenseMatrix m_CB_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                List<BreakerCurrentPhasorGroup> m_islandActiveBreakerCurrentMeasurements = islandActiveBreakerCurrentMeasurements.Values.ToList();
                m_CB_temp = DenseMatrix.OfArray(new Complex[m_islandActiveBreakerCurrentMeasurements.Count, m_islandActiveBreakerCurrentMeasurements.Count]);
                foreach (BreakerCurrentPhasorGroup breakerCurrentPhasorGroup in m_islandActiveBreakerCurrentMeasurements)
                {
                    m_CB_temp[m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup), m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup)] = new Complex(1, 0);
                }
                m_island_CB.Add(kv.Key, m_CB_temp);
            }
        }

        private void BuildEQMatrix(Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements, Dictionary<string, CurrentPhasorGroup> activeCurrentMeasurements, TopologyProcessor topologyProcessor)
        {
            
        }

        #endregion

        #region [ Public Methods ]

        public void CompleteCurrentLSE()
        {
            try
            {
                DistributeMeasurements();
                //BuildMatrices(m_activeBreakerCurrentMeasurements, m_activeCurrentMeasurements, m_topologyProcessor);
                CurrentLSEFormulation();
                SolveLSE();
            }
            catch (Exception ex)
            {
                var i = ex.Message;
            }
        }

        public void CompleteThreePhaseCurrentLSE()
        {
            DistributeMeasurements();
            ThreePhaseCurrentLSEFormulation();
            SolveThreePhaseLSE();
        }

        public void FastCurrentLSE(Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements, Dictionary<string, CurrentPhasorGroup> activeCurrentMeasurements)
        {
            m_activeBreakerCurrentMeasurements.Clear();
            m_activeCurrentMeasurements.Clear();

            m_activeBreakerCurrentMeasurements = activeBreakerCurrentMeasurements;
            m_activeCurrentMeasurements = activeCurrentMeasurements;

            DistributeMeasurements();
            SolveLSE();
        }

        #endregion
    }
}
