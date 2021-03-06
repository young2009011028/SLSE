﻿using System;
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
                HashSet<string> removedActiveCurrentMeasurements = new HashSet<string>();
                try
                {
                    foreach (KeyValuePair<string, CurrentPhasorGroup> kw in islandActiveCurrentMeasurements)
                    {
                        //kw.Value.m
                        if (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerOne) || (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerTwo)))
                        {
                            removedActiveCurrentMeasurements.Add(kw.Key);
                            //islandActiveCurrentMeasurements.Remove(kw.Key);
                            //m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
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
                    foreach(string kn in removedActiveCurrentMeasurements)
                    {
                        if (islandActiveCurrentMeasurements.ContainsKey(kn))
                        {
                            islandActiveCurrentMeasurements.Remove(kn);
                        }
                    }

                }
                catch(Exception ex)
                {
                    int i = 1;
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
                

                // main bus current KCL check
                // EAST BUS: with breaker currents: 4730, 4724, 4731
                // WEST BUS: with breaker currents: 4594, 4586, 4732
                DenseMatrix m_MainBus_temp = DenseMatrix.OfArray(new Complex[2, m_islandActiveBreakerCurrentMeasurements.Count]);
                foreach(BreakerCurrentPhasorGroup breakerCurrent in m_islandActiveBreakerCurrentMeasurements)
                {
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4730")
                    {
                        m_MainBus_temp[0, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4724")
                    {
                        m_MainBus_temp[0, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4731")
                    {
                        m_MainBus_temp[0, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4594")
                    {
                        m_MainBus_temp[1, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4586")
                    {
                        m_MainBus_temp[1, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4732")
                    {
                        m_MainBus_temp[1, m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                    }
                }
                //m_MainBus_temp.L2Norm
                //add main bus temp to 
                DenseMatrix m_EQ_temp_AddMainBus = MatrixCalculationExtensions.VerticallyConcatenate(m_EQ_temp, m_MainBus_temp);

                m_island_EQ.Add(kv.Key, m_EQ_temp_AddMainBus);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2, m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2]);
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
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count)+2)
                    {
                        m_W_temp[i, i] = m_W_temp[0, 0];                       
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2, 1]);

                for (int i = 0; i < m_Z_temp.RowCount; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        measurementList_temp.Add(i, m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.MagnitudeKey);
                        m_Z_temp[i, 0] = m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        measurementList_temp.Add(i, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.MagnitudeKey);
                        m_Z_temp[i, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count)+2)
                    {
                        measurementList_temp.Add(i, "main bus zero injection:  "+i.ToString());
                        m_Z_temp[i, 0] = 0;
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
                                I2 = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else if (currentMeasurement.MeasuredBreakerTwoDirection.Equals("Negative"))
                            {
                                I2 = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
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
                for (int i = 0; i < m_islandActiveBreakerCurrentMeasurements.Count;i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        m_CB_temp[3 * i, 3 * i] = new Complex(1, 0);
                        m_CB_temp[3 * i + 1, 3 * i + 1] = new Complex(1, 0);
                        m_CB_temp[3 * i + 2, 3 * i + 2] = new Complex(1, 0);
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                
                //foreach (BreakerCurrentPhasorGroup breakerCurrentPhasorGroup in m_islandActiveBreakerCurrentMeasurements)
                //{
                //    m_CB_temp[m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup), m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrentPhasorGroup)] = new Complex(1, 0);
                //}
                m_island_CB.Add(kv.Key, m_CB_temp);

                Dictionary<string, CurrentPhasorGroup> islandActiveCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
                if (m_islandCurrentMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveCurrentMeasurements = m_islandCurrentMeasurements[kv.Key];
                }

                HashSet<string> removedActiveCurrentMeasurements = new HashSet<string>();
                try
                {
                    foreach (KeyValuePair<string, CurrentPhasorGroup> kw in islandActiveCurrentMeasurements)
                    {
                        //kw.Value.m
                        if (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerOne) || (!islandActiveBreakerCurrentMeasurements.ContainsKey(kw.Value.MeasuredBreakerTwo)))
                        {
                            removedActiveCurrentMeasurements.Add(kw.Key);
                            //islandActiveCurrentMeasurements.Remove(kw.Key);
                            //m_islandCurrentMeasurements[kv.Key].Remove(kw.Key);
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
                    foreach (string kn in removedActiveCurrentMeasurements)
                    {
                        if (islandActiveCurrentMeasurements.ContainsKey(kn))
                        {
                            islandActiveCurrentMeasurements.Remove(kn);
                        }
                    }

                }
                catch (Exception ex)
                {
                    int i = 1;
                }

                DenseMatrix m_EQ_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_EQ_temp = DenseMatrix.OfArray(new Complex[3 * islandActiveCurrentMeasurements.Count, 3 * islandActiveBreakerCurrentMeasurements.Count]);

                List<CurrentPhasorGroup> m_islandActiveCurrentMeasurements = islandActiveCurrentMeasurements.Values.ToList();

                foreach (CurrentPhasorGroup EQ_Current in m_islandActiveCurrentMeasurements)
                {
                    if (islandActiveBreakerCurrentMeasurements.ContainsKey(EQ_Current.MeasuredBreakerOne))
                    {
                        if (EQ_Current.MeasuredBreakerOneDirection.Equals("Positive"))
                        {
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne]) + 1] = new Complex(1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne]) + 2] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerOneDirection.Equals("Negative"))
                        {
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne])] = new Complex(-1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne]) + 1] = new Complex(-1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerOne]) + 2] = new Complex(-1, 0);
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
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo]) + 1] = new Complex(1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo]) + 2] = new Complex(1, 0);
                            //m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(1, 0);
                        }
                        else if (EQ_Current.MeasuredBreakerTwoDirection.Equals("Negative"))
                        {
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(-1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo]) + 1] = new Complex(-1, 0);
                            m_EQ_temp[3 * m_islandActiveCurrentMeasurements.IndexOf(EQ_Current) + 2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo]) + 2] = new Complex(-1, 0);
                            //m_EQ_temp[m_islandActiveCurrentMeasurements.IndexOf(EQ_Current), m_islandActiveBreakerCurrentMeasurements.IndexOf(islandActiveBreakerCurrentMeasurements[EQ_Current.MeasuredBreakerTwo])] = new Complex(-1, 0);
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
                // main bus current KCL check
                // EAST BUS: with breaker currents: 4730, 4724, 4731
                // WEST BUS: with breaker currents: 4594, 4586, 4732
                DenseMatrix m_MainBus_temp = DenseMatrix.OfArray(new Complex[6, 3 * m_islandActiveBreakerCurrentMeasurements.Count]);
                foreach (BreakerCurrentPhasorGroup breakerCurrent in m_islandActiveBreakerCurrentMeasurements)
                {
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4730")
                    {
                        m_MainBus_temp[0, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4724")
                    {
                        m_MainBus_temp[0, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4731")
                    {
                        m_MainBus_temp[0, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[1, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[2, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4594")
                    {
                        m_MainBus_temp[3, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[4, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[5, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4586")
                    {
                        m_MainBus_temp[3, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[4, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[5, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                    if (breakerCurrent.MeasuredBreakerID == "SUBSTATION_PCB_500_4732")
                    {
                        m_MainBus_temp[3, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent)] = 1;
                        m_MainBus_temp[4, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 1] = 1;
                        m_MainBus_temp[5, 3 * m_islandActiveBreakerCurrentMeasurements.IndexOf(breakerCurrent) + 2] = 1;
                    }
                }
                //m_MainBus_temp.L2Norm
                //add main bus temp to 
                DenseMatrix m_EQ_temp_AddMainBus = MatrixCalculationExtensions.VerticallyConcatenate(m_EQ_temp, m_MainBus_temp);

                m_island_EQ.Add(kv.Key, m_EQ_temp_AddMainBus);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[3 * (m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2), 3 * (m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2)]);
                for (int i = 0; i < m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count+2; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        m_W_temp[3 * i, 3 * i] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PhaseA.Measurement.MeasurementCovariance, 0);
                        m_W_temp[3 * i + 1, 3 * i + 1] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PhaseB.Measurement.MeasurementCovariance, 0);
                        m_W_temp[3 * i + 2, 3 * i + 2] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PhaseC.Measurement.MeasurementCovariance, 0);
                        //m_W_temp[i, i] = new Complex(1 / m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.MeasurementCovariance, 0);
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        m_W_temp[3 * i, 3 * i] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseA.Measurement.MeasurementCovariance), 0);
                        m_W_temp[3 * i + 1, 3 * i + 1] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseB.Measurement.MeasurementCovariance), 0);
                        m_W_temp[3 * i + 2, 3 * i + 2] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseC.Measurement.MeasurementCovariance), 0);
                        //m_W_temp[i, i] = new Complex(1 / (m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.MeasurementCovariance), 0);
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count+2))
                    {
                        m_W_temp[3 * i, 3 * i] = m_W_temp[0, 0];
                        m_W_temp[3 * i + 1, 3 * i + 1] = m_W_temp[1, 1];
                        m_W_temp[3 * i + 2, 3 * i + 2] = m_W_temp[2, 2];
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[3 * (m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2), 1]);

                for (int i = 0; i < m_islandActiveCurrentMeasurements.Count + m_islandActiveBreakerCurrentMeasurements.Count + 2; i++)
                {
                    if (i < m_islandActiveBreakerCurrentMeasurements.Count)
                    {
                        measurementList_temp.Add(3 * i, m_islandActiveBreakerCurrentMeasurements[i].PhaseA.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 1, m_islandActiveBreakerCurrentMeasurements[i].PhaseB.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 2, m_islandActiveBreakerCurrentMeasurements[i].PhaseC.Measurement.MagnitudeKey);
                        //measurementList_temp.Add(i, m_islandActiveBreakerCurrentMeasurements[i].MeasuredBreakerID);
                        m_Z_temp[3 * i, 0] = m_islandActiveBreakerCurrentMeasurements[i].PhaseA.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 1, 0] = m_islandActiveBreakerCurrentMeasurements[i].PhaseB.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 2, 0] = m_islandActiveBreakerCurrentMeasurements[i].PhaseC.Measurement.PerUnitComplexPhasor;
                        //m_Z_temp[i, 0] = m_islandActiveBreakerCurrentMeasurements[i].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count))
                    {
                        measurementList_temp.Add(3 * i, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseA.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 1, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseB.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 2, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseC.Measurement.MagnitudeKey);
                        //measurementList_temp.Add(i, m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].MeasuredNodeID);
                        m_Z_temp[3 * i, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseA.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 1, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseB.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 2, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PhaseC.Measurement.PerUnitComplexPhasor;
                        //m_Z_temp[i, 0] = m_islandActiveCurrentMeasurements[i - m_islandActiveBreakerCurrentMeasurements.Count].PositiveSequence.Measurement.PerUnitComplexPhasor;
                    }
                    else if (i < (m_islandActiveBreakerCurrentMeasurements.Count + m_islandActiveCurrentMeasurements.Count) + 2)
                    {
                        measurementList_temp.Add(3 * i, "main bus zero injection:PhaseA " + i.ToString());
                        measurementList_temp.Add(3 * i + 1, "main bus zero injection:PhaseB " + i.ToString());
                        measurementList_temp.Add(3 * i + 2, "main bus zero injection:PhaseC " + i.ToString());
                        m_Z_temp[3 * i, 0] = 0;
                        m_Z_temp[3 * i + 1, 0] = 0;
                        m_Z_temp[3 * i + 2, 0] = 0;
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
            m_badDataList.Clear();
            foreach (KeyValuePair<string, Tree> kv in m_topologyProcessor)
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
                badDataList = LSECalculation.CalculateLSE(H, W, Z, out X, 50, true);
                if (badDataList.Count>0)
                {
                    int i = 1;
                }
                m_badDataList.Add(kv.Key, badDataList);

                if (X.RowCount > 0)
                {
                    List<BreakerCurrentPhasorGroup> islandBreakerCurrentMeasurements = m_islandBreakerCurrentMeasurements[kv.Key].Values.ToList();
                    for (int i = 0; i < islandBreakerCurrentMeasurements.Count; i++)
                    {
                        islandBreakerCurrentMeasurements[i].PhaseA.Estimate.PerUnitComplexPhasor = X[3 * i, 0];
                        islandBreakerCurrentMeasurements[i].PhaseB.Estimate.PerUnitComplexPhasor = X[3 * i + 1, 0];
                        islandBreakerCurrentMeasurements[i].PhaseC.Estimate.PerUnitComplexPhasor = X[3 * i + 2, 0];
                        //islandBreakerCurrentMeasurements[i].PositiveSequence.Estimate.PerUnitComplexPhasor = X[i, 0];
                    }

                    Dictionary<string, CurrentPhasorGroup> islandCurrentMeasurements = m_islandCurrentMeasurements[kv.Key];

                    foreach (KeyValuePair<string, CurrentPhasorGroup> EQ_Current in islandCurrentMeasurements)
                    {
                        Complex I1A = new Complex();
                        Complex I1B = new Complex();
                        Complex I1C = new Complex();
                        Complex I2A = new Complex();
                        Complex I2B = new Complex();
                        Complex I2C = new Complex();

                        //Complex I1 = new Complex();
                        //Complex I2 = new Complex();
                        CurrentPhasorGroup currentMeasurement = EQ_Current.Value;
                        if (m_islandBreakerCurrentMeasurements[kv.Key].ContainsKey(currentMeasurement.MeasuredBreakerOne))
                        {
                            if (currentMeasurement.MeasuredBreakerOneDirection.Equals("Positive"))
                            {
                                I1A = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseA.Estimate.PerUnitComplexPhasor;
                                I1B = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseB.Estimate.PerUnitComplexPhasor;
                                I1C = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseC.Estimate.PerUnitComplexPhasor;
                                //I1 = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else if (currentMeasurement.MeasuredBreakerOneDirection.Equals("Negative"))
                            {
                                I1A = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseA.Estimate.PerUnitComplexPhasor;
                                I1B = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseB.Estimate.PerUnitComplexPhasor;
                                I1C = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PhaseC.Estimate.PerUnitComplexPhasor;
                                //I1 = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerOne].PositiveSequence.Estimate.PerUnitComplexPhasor;
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
                                I2A = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseA.Estimate.PerUnitComplexPhasor;
                                I2B = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseB.Estimate.PerUnitComplexPhasor;
                                I2C = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseC.Estimate.PerUnitComplexPhasor;
                                //I2 = m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
                            }
                            else if (currentMeasurement.MeasuredBreakerTwoDirection.Equals("Negative"))
                            {
                                I2A = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseA.Estimate.PerUnitComplexPhasor;
                                I2B = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseB.Estimate.PerUnitComplexPhasor;
                                I2C = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PhaseC.Estimate.PerUnitComplexPhasor;
                                //I2 = (-1) * m_islandBreakerCurrentMeasurements[kv.Key][currentMeasurement.MeasuredBreakerTwo].PositiveSequence.Estimate.PerUnitComplexPhasor;
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
                        EQ_Current.Value.PhaseA.Estimate.PerUnitComplexPhasor = I1A + I2A;
                        EQ_Current.Value.PhaseB.Estimate.PerUnitComplexPhasor = I1B + I2B;
                        EQ_Current.Value.PhaseC.Estimate.PerUnitComplexPhasor = I1C + I2C;
                    }
                }
            }
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
