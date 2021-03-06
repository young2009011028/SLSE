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
    class VoltageEstimator
    {
        #region [ Private Members ]

        private Dictionary<string, Dictionary<string, VoltagePhasorGroup>> m_islandVoltageMeasurements = new Dictionary<string, Dictionary<string, VoltagePhasorGroup>>();
        private Dictionary<string, DenseMatrix> m_island_H = new Dictionary<string, DenseMatrix>();
        private Dictionary<string, DenseMatrix> m_island_W = new Dictionary<string, DenseMatrix>();
        private Dictionary<string, DenseMatrix> m_island_Z = new Dictionary<string, DenseMatrix>();

        private Dictionary<string, HashSet<int>> m_badDataList = new Dictionary<string, HashSet<int>>();
        private Dictionary<string, Dictionary<int, string>> m_measurementList = new Dictionary<string, Dictionary<int, string>>();

        private Dictionary<string, VoltagePhasorGroup> m_activeVoltageMeasurements = new Dictionary<string, VoltagePhasorGroup>();
        private TopologyProcessor m_topologyProcessor = new TopologyProcessor();
        private List<Node> m_Nodes = new List<Node>();

        #endregion

        #region [ Properties ]

        public Dictionary<string, Dictionary<string, VoltagePhasorGroup>> IslandVoltageMeasurements
        {
            get
            {
                return m_islandVoltageMeasurements;
            }
            set
            {
                m_islandVoltageMeasurements = value;
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

        #region [ Constructor ]

        public VoltageEstimator(Dictionary<string, VoltagePhasorGroup> activeBreakerCurrentMeasurements, TopologyProcessor topologyProcessor, List<Node> nodes)
        {
            m_activeVoltageMeasurements = activeBreakerCurrentMeasurements;
            m_topologyProcessor = topologyProcessor;
            m_Nodes = nodes;
        }

        #endregion

        #region [ Private Methods ]

        private void DistributeMeasurements()
        {
            m_islandVoltageMeasurements.Clear();
            foreach(KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                Dictionary<string, VoltagePhasorGroup> islandActiveVoltageMeasurements = new Dictionary<string, VoltagePhasorGroup>();
                foreach (KeyValuePair<string, VoltagePhasorGroup> ku in m_activeVoltageMeasurements)
                {
                    if (kv.Value.ContainsKey(ku.Key))
                    {
                        islandActiveVoltageMeasurements.Add(ku.Key, ku.Value);
                    }
                }
                m_islandVoltageMeasurements.Add(kv.Key, islandActiveVoltageMeasurements);
            }
        }

        private void VoltageLSEFormulation()
        {
            foreach(KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_islandVoltageMeasurements[kv.Key] == null || m_islandVoltageMeasurements[kv.Key].Count < 1)
                {
                    Console.WriteLine("Voltage State Estimator: unobservable island");
                    continue;
                }

                Dictionary<string, VoltagePhasorGroup> islandActiveVoltageMeasurements = new Dictionary<string, VoltagePhasorGroup>();
                if (m_islandVoltageMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveVoltageMeasurements = m_islandVoltageMeasurements[kv.Key];
                }

                DenseMatrix m_H_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                List<VoltagePhasorGroup> m_islandActiveVoltageMeasurements = islandActiveVoltageMeasurements.Values.ToList();
                m_H_temp = DenseMatrix.OfArray(new Complex[m_islandActiveVoltageMeasurements.Count, 1]);

                foreach (VoltagePhasorGroup voltagePhasorGroup in m_islandActiveVoltageMeasurements)
                {
                    m_H_temp[m_islandActiveVoltageMeasurements.IndexOf(voltagePhasorGroup),0] = new Complex(1, 0);
                }
                m_island_H.Add(kv.Key, m_H_temp);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[m_islandActiveVoltageMeasurements.Count, m_islandActiveVoltageMeasurements.Count]);
                for (int i = 0; i < m_W_temp.RowCount; i++)
                {
                    if (i < m_islandActiveVoltageMeasurements.Count)
                    {
                        m_W_temp[i, i] = new Complex(1 / m_islandActiveVoltageMeasurements[i].PositiveSequence.Measurement.MeasurementCovariance, 0);
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[m_islandActiveVoltageMeasurements.Count, 1]);

                for (int i = 0; i < m_Z_temp.RowCount; i++)
                {
                    if (i < m_islandActiveVoltageMeasurements.Count)
                    {
                        measurementList_temp.Add(i, m_islandActiveVoltageMeasurements[i].PositiveSequence.Measurement.MagnitudeKey);
                        m_Z_temp[i, 0] = m_islandActiveVoltageMeasurements[i].PositiveSequence.Measurement.PerUnitComplexPhasor;
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
            foreach (KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_island_H == null || !m_island_H.ContainsKey(kv.Key))
                {
                    Console.WriteLine("not enough voltage measurement");
                    continue;
                }
                HashSet<int> badDataList = new HashSet<int>();
                DenseMatrix X;
                DenseMatrix Z;
                DenseMatrix H;
                DenseMatrix W;
                DenseMatrix P;

                Z = m_island_Z[kv.Key];
                H = m_island_H[kv.Key];
                W = m_island_W[kv.Key];
                badDataList = LSECalculation.CalculateLSE(H, W, Z, out X, 10, true);
                m_badDataList.Add(kv.Key, badDataList);

                if (X.RowCount > 0)
                {
                    foreach (Node node in m_Nodes)
                    {
                        if (kv.Value.ContainsKey(node.InternalID))
                        {
                            node.Voltage.PositiveSequence.Estimate.PerUnitComplexPhasor = X[0, 0];
                        }
                    }

                    //List<VoltagePhasorGroup> islandVoltageMeasurements = m_islandVoltageMeasurements[kv.Key].Values.ToList();

                    //for (int i = 0; i < islandVoltageMeasurements.Count; i++)
                    //{
                    //    islandVoltageMeasurements[i].PositiveSequence.Estimate.PerUnitComplexPhasor = X[0, 0];
                    //}
                    // if (input has NaN, LSE can still give estimate to all the nodes in this island)

                }
            }
        }

        private void ThreePhaseVoltageLSEFormulation()
        {
            foreach (KeyValuePair<string, Tree> kv in m_topologyProcessor)
            {
                if (m_islandVoltageMeasurements[kv.Key] == null || m_islandVoltageMeasurements[kv.Key].Count < 1)
                {
                    Console.WriteLine("Voltage State Estimator: unobservable island");
                    continue;
                }

                Dictionary<string, VoltagePhasorGroup> islandActiveVoltageMeasurements = new Dictionary<string, VoltagePhasorGroup>();
                if (m_islandVoltageMeasurements.ContainsKey(kv.Key))
                {
                    islandActiveVoltageMeasurements = m_islandVoltageMeasurements[kv.Key];
                }

                DenseMatrix m_H_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                List<VoltagePhasorGroup> m_islandActiveVoltageMeasurements = islandActiveVoltageMeasurements.Values.ToList();
                m_H_temp = DenseMatrix.OfArray(new Complex[3 * m_islandActiveVoltageMeasurements.Count, 3 * 1]);

                foreach (VoltagePhasorGroup voltagePhasorGroup in m_islandActiveVoltageMeasurements)
                {
                    int indexOfMeasuredVoltage = m_islandActiveVoltageMeasurements.IndexOf(voltagePhasorGroup);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (i == j)
                            {
                                m_H_temp[3 * indexOfMeasuredVoltage + i, j] = new Complex(1, 0);
                            }
                        }
                    }
                }
                m_island_H.Add(kv.Key, m_H_temp);

                DenseMatrix m_W_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_W_temp = DenseMatrix.OfArray(new Complex[3 * m_islandActiveVoltageMeasurements.Count, 3 * m_islandActiveVoltageMeasurements.Count]);
                for (int i = 0; i < m_islandActiveVoltageMeasurements.Count; i++)
                {
                    if (i < m_islandActiveVoltageMeasurements.Count)
                    {
                        m_W_temp[3 * i, 3 * i] = new Complex(1 / m_islandActiveVoltageMeasurements[i].PhaseA.Measurement.MeasurementCovariance, 0);
                        m_W_temp[3 * i + 1, 3 * i + 1] = new Complex(1 / m_islandActiveVoltageMeasurements[i].PhaseB.Measurement.MeasurementCovariance, 0);
                        m_W_temp[3 * i + 2, 3 * i + 2] = new Complex(1 / m_islandActiveVoltageMeasurements[i].PhaseC.Measurement.MeasurementCovariance, 0);
                    }
                    else
                    {
                        Console.WriteLine("unexpected measurement");
                    }
                }
                m_island_W.Add(kv.Key, m_W_temp);

                Dictionary<int, string> measurementList_temp = new Dictionary<int, string>();
                DenseMatrix m_Z_temp = DenseMatrix.OfArray(new Complex[1, 1]);
                m_Z_temp = DenseMatrix.OfArray(new Complex[3 * m_islandActiveVoltageMeasurements.Count, 1]);

                for (int i = 0; i < m_islandActiveVoltageMeasurements.Count; i++)
                {
                    if (i < m_islandActiveVoltageMeasurements.Count)
                    {
                        measurementList_temp.Add(3 * i, m_islandActiveVoltageMeasurements[i].PhaseA.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 1, m_islandActiveVoltageMeasurements[i].PhaseB.Measurement.MagnitudeKey);
                        measurementList_temp.Add(3 * i + 2, m_islandActiveVoltageMeasurements[i].PhaseC.Measurement.MagnitudeKey);
                        m_Z_temp[3 * i, 0] = m_islandActiveVoltageMeasurements[i].PhaseA.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 1, 0] = m_islandActiveVoltageMeasurements[i].PhaseB.Measurement.PerUnitComplexPhasor;
                        m_Z_temp[3 * i + 2, 0] = m_islandActiveVoltageMeasurements[i].PhaseC.Measurement.PerUnitComplexPhasor;
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
                if (m_island_H == null || !m_island_H.ContainsKey(kv.Key))
                {
                    Console.WriteLine("not enough voltage measurement");
                    continue;
                }
                HashSet<int> badDataList = new HashSet<int>();
                DenseMatrix X;
                DenseMatrix Z;
                DenseMatrix H;
                DenseMatrix W;
                DenseMatrix P;

                Z = m_island_Z[kv.Key];
                H = m_island_H[kv.Key];
                W = m_island_W[kv.Key];
                badDataList = LSECalculation.CalculateLSE(H, W, Z, out X, 10, true);
                m_badDataList.Add(kv.Key, badDataList);

                if (X.RowCount > 0)
                {
                    foreach (Node node in m_Nodes)
                    {
                        if (kv.Value.ContainsKey(node.InternalID))
                        {
                            node.Voltage.PhaseA.Estimate.PerUnitComplexPhasor = X[0, 0];
                            node.Voltage.PhaseB.Estimate.PerUnitComplexPhasor = X[1, 0];
                            node.Voltage.PhaseC.Estimate.PerUnitComplexPhasor = X[2, 0];
                        }
                    }

                    //List<VoltagePhasorGroup> islandVoltageMeasurements = m_islandVoltageMeasurements[kv.Key].Values.ToList();

                    //for (int i = 0; i < islandVoltageMeasurements.Count; i++)
                    //{
                    //    islandVoltageMeasurements[i].PositiveSequence.Estimate.PerUnitComplexPhasor = X[0, 0];
                    //}
                    // if (input has NaN, LSE can still give estimate to all the nodes in this island)

                }
            }
        }

        #endregion

        #region [ Public Methods ]

        public void CompleteVoltageLSE()
        {
            DistributeMeasurements();
            VoltageLSEFormulation();
            SolveLSE();
        }

        public void CompleteThreePhaseVoltageLSE()
        {
            DistributeMeasurements();
            ThreePhaseVoltageLSEFormulation();
            SolveThreePhaseLSE();
        }

        public void FastVoltageLSE(Dictionary<string, VoltagePhasorGroup> activeVoltageMeasurements)
        {
            m_activeVoltageMeasurements.Clear();
            m_activeVoltageMeasurements = activeVoltageMeasurements;

            DistributeMeasurements();
            SolveLSE();
        }

        #endregion
    }
}
