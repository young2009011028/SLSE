//******************************************************************************************************
//  Substation.cs
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      September 30th 2015
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using SubstationLSE.Measurements;
using SubstationLSE.Topology;
using SubstationLSE.Algorithm;

namespace SubstationLSE
{
    public class Substation
    {
        #region [Private Members]
        private string m_internalID;
        private string m_mode; 

        //Equipments at the substation
        List<Node> nodes = new List<Node>();
        List<CircuitBreaker> circuitBreakers = new List<CircuitBreaker>();
        List<Switch> switches = new List<Switch>();
        List<Transformer> transformers = new List<Transformer>();
        List<TransmissionLine> transmissionLines = new List<TransmissionLine>();
        List<Generator> generators = new List<Generator>();
        List<ShuntCompensator> shunts = new List<ShuntCompensator>();

        //Substation configuration
        Dictionary<string, string[]> CB_Nodes = new Dictionary<string, string[]>();
        Dictionary<string, string> Node_EQ = new Dictionary<string, string>();
        Dictionary<string, string[]> EQ_Nodes = new Dictionary<string, string[]>();

        //Measurements
        private Dictionary<string, double> inputMeasurements = new Dictionary<string, double>();
        private Dictionary<string, double> outputMeasurements = new Dictionary<string, double>();
        private HashSet<string> badDataList = new HashSet<string>();
        List<VoltagePhasorGroup> voltageMeasurements = new List<VoltagePhasorGroup>();
        List<CurrentPhasorGroup> currentMeasurements = new List<CurrentPhasorGroup>();
        List<BreakerCurrentPhasorGroup> breakerCurrentMeasurements = new List<BreakerCurrentPhasorGroup>();
        private Dictionary<string, VoltagePhasorGroup> activeVoltageMeasurements = new Dictionary<string, VoltagePhasorGroup>();
        private Dictionary<string, CurrentPhasorGroup> activeCurrentMeasurements = new Dictionary<string, CurrentPhasorGroup>();
        private Dictionary<string, BreakerCurrentPhasorGroup> activeBreakerCurrentMeasurements = new Dictionary<string, BreakerCurrentPhasorGroup>();

        //Algorithms
        TopologyProcessor topologyProcessor;
        CurrentEstimator currentEstimator;
        VoltageEstimator voltageEstimator;

        //Condition Flags
        bool breakerStatusChange = true;
        bool measurementStatusChange = true; 

        #endregion

        #region [Properties]
        /// <summary>
        /// An string identifier globally unique among other objects of the same type.
        /// </summary>
        [XmlAttribute("ID")]
        public string InternalID
        {
            get
            {
                return m_internalID;
            }
            set
            {
                m_internalID = value;
            }
        }

        /// <summary>
        /// positive or three phase
        /// </summary>
        [XmlAttribute("Mode")]
        public string Mode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode = value;
            }
        }

        /// <summary>
        /// A list of node at the substation
        /// </summary>
        [XmlArray("Nodes")]
        public List<Node> Nodes
        {
            get
            {
                return nodes;
            }
            set
            {
                nodes = value;
            }
        }

        /// <summary>
        /// A list of circuitbreaker at the substation
        /// </summary>
        [XmlArray("CircuitBreakers")]
        public List<CircuitBreaker> CircuitBreakers
        {
            get
            {
                return circuitBreakers;
            }
            set
            {
                circuitBreakers = value;
            }
        }

        /// <summary>
        /// A list of switch at the substation
        /// </summary>
        [XmlArray("Switches")]
        public List<Switch> Switches
        {
            get
            {
                return switches;
            }
            set
            {
                switches = value;
            }
        }

        /// <summary>
        /// A list of transformer at the substation
        /// </summary>
        [XmlArray("Transformers")]
        public List<Transformer> Transformers
        {
            get
            {
                return transformers;
            }
            set
            {
                transformers = value;
            }
        }

        /// <summary>
        /// A list of transmissionLine at the substation
        /// </summary>
        [XmlArray("TransmissionLines")]
        public List<TransmissionLine> TransmissionLines
        {
            get
            {
                return transmissionLines;
            }
            set
            {
                transmissionLines = value;
            }
        }

        /// <summary>
        /// A list of generator at the substation
        /// </summary>
        [XmlArray("Generators")]
        public List<Generator> Generators
        {
            get
            {
                return generators;
            }
            set
            {
                generators = value;
            }
        }

        /// <summary>
        /// A list of shunt at the substation
        /// </summary>
        [XmlArray("Shunts")]
        public List<ShuntCompensator> Shunts
        {
            get
            {
                return shunts;
            }
            set
            {
                shunts = value;
            }
        }

        [XmlIgnore()]
        public Dictionary<string, double> InputMeasurements
        {
            get
            {
                return inputMeasurements;
            }
            set
            {
                inputMeasurements = value;
            }
        }

        [XmlIgnore()]
        public Dictionary<string, double> OutputMeasurements
        {
            get
            {
                return outputMeasurements;
            }
            set
            {
                outputMeasurements = value;
            }
        }

        [XmlIgnore()]
        public HashSet<string> BadDataList 
        {
            get
            {
                return badDataList;
            }
            set
            {
                badDataList = value;
            }
        }

        #endregion

        #region [Private Methods]
        
        private void LinkHierarchicalReferences()
        {
            Dictionary<string, Node> localNodes = Nodes.Distinct().ToDictionary(x => x.InternalID, x => x);
            foreach (CircuitBreaker circuitBreaker in CircuitBreakers)
            {
                Node value = null;
                if (localNodes.TryGetValue(circuitBreaker.FromNodeID, out value))
                {
                    circuitBreaker.FromNode = value;
                    circuitBreaker.BreakerCurrent.FromNode = value;
                    circuitBreaker.BreakerCurrent.FromNodeID = value.InternalID;
                    value = null;
                }
                if (localNodes.TryGetValue(circuitBreaker.ToNodeID, out value))
                {
                    circuitBreaker.ToNode = value;
                    circuitBreaker.BreakerCurrent.ToNode = value;
                    circuitBreaker.BreakerCurrent.ToNodeID = value.InternalID;
                    value = null;
                }
                circuitBreaker.BreakerCurrent.MeasuredBreakerID = circuitBreaker.InternalID;
                circuitBreaker.BreakerCurrent.MeasuredBreaker = circuitBreaker;
            }

            foreach (Switch circuitSwitch in Switches)
            {
                Node value = null;
                if (localNodes.TryGetValue(circuitSwitch.FromNodeID, out value))
                {
                    circuitSwitch.FromNode = value;
                    value = null;
                }
                if (localNodes.TryGetValue(circuitSwitch.ToNodeID, out value))
                {
                    circuitSwitch.ToNode = value;
                    value = null;
                }
            }

            foreach (Transformer transformer in Transformers)
            {
                Node value = null;
                if (localNodes.TryGetValue(transformer.FromNodeID, out value))
                {
                    transformer.FromNode = value;
                    transformer.FromNodeCurrent.MeasuredNode = value;
                    transformer.FromNodeCurrent.MeasuredNodeID = value.InternalID;
                    //transformer.FromNodeCurrent.MeasuredFromNode = value;
                    //transformer.ToNodeCurrent.MeasuredToNode = value;
                    value = null;
                }
                if (localNodes.TryGetValue(transformer.ToNodeID, out value))
                {
                    transformer.ToNode = value;
                    transformer.ToNodeCurrent.MeasuredNode = value;
                    transformer.ToNodeCurrent.MeasuredNodeID = value.InternalID;
                    //transformer.ToNodeCurrent.MeasuredFromNode = value;
                    //transformer.FromNodeCurrent.MeasuredToNode = value;
                    value = null;
                }
            }

            foreach (ShuntCompensator shunt in Shunts)
            {
                Node value = null;
                if (localNodes.TryGetValue(shunt.ConnectedNodeID, out value))
                {
                    shunt.ConnectedNode = value;
                    shunt.ShuntCurrent.MeasuredNode = value;
                    shunt.ShuntCurrent.MeasuredNodeID = value.InternalID;
                    value = null;
                }
            }

            foreach (TransmissionLine transmissionLine in TransmissionLines)
            {
                Node value = null;
                if (localNodes.TryGetValue(transmissionLine.ConnectedNodeID, out value))
                {
                    transmissionLine.ConnectedNode = value;
                    transmissionLine.LineCurrent.MeasuredNode = value;
                    transmissionLine.LineCurrent.MeasuredNodeID = value.InternalID;
                    value = null;
                }
            }
        }

        private void ListSubstationMeasurements()
        {
            foreach (Node node in nodes)
            {
                if (node.Voltage.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    voltageMeasurements.Add(node.Voltage);
                }
            }

            foreach(Transformer xf in transformers)
            {
                if (xf.FromNodeCurrent.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    currentMeasurements.Add(xf.FromNodeCurrent);
                }
                if (xf.ToNodeCurrent.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    currentMeasurements.Add(xf.ToNodeCurrent);
                }
            }

            foreach (ShuntCompensator shunt in shunts)
            {
                if (shunt.ShuntCurrent.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    currentMeasurements.Add(shunt.ShuntCurrent);
                }
            }

            foreach (TransmissionLine line in transmissionLines)
            {
                if (line.LineCurrent.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    currentMeasurements.Add(line.LineCurrent);
                }
            }

            foreach (CircuitBreaker cb in circuitBreakers)
            {
                if (cb.BreakerCurrent.PositiveSequence.Measurement.MagnitudeKey != "Undefined")
                {
                    breakerCurrentMeasurements.Add(cb.BreakerCurrent);
                }
            }
        }

        private void SubstationConfig()
        {
            foreach (TransmissionLine line in transmissionLines)
            {
                Node_EQ.Add(line.ConnectedNodeID, line.InternalID);
                EQ_Nodes.Add(line.InternalID, new string[] { line.ConnectedNodeID });
            }

            foreach (Transformer xf in transformers)
            {
                Node_EQ.Add(xf.FromNodeID, xf.InternalID);
                Node_EQ.Add(xf.ToNodeID, xf.InternalID);

                EQ_Nodes.Add(xf.InternalID, new string[] { xf.FromNodeID, xf.ToNodeID });
            }

            foreach (Generator unit in generators)
            {
                Node_EQ.Add(unit.ConnectedNodeID, unit.InternalID);
                EQ_Nodes.Add(unit.InternalID, new string[] { unit.ConnectedNodeID });
            }

            foreach (ShuntCompensator shunt in shunts)
            {
                Node_EQ.Add(shunt.ConnectedNodeID, shunt.InternalID);
                EQ_Nodes.Add(shunt.InternalID, new string[] { shunt.ConnectedNodeID });
            }
        }

        private void DetermineBreakerStatus()
        {
            CB_Nodes.Clear();
            foreach (CircuitBreaker breaker in circuitBreakers)
            {
                if (!CB_Nodes.ContainsKey(breaker.InternalID))
                {
                    if (breaker.ActualState.Equals("Closed"))
                        CB_Nodes.Add(breaker.InternalID, new string[] { breaker.FromNodeID, breaker.ToNodeID });
                }
            }

            foreach (Switch sw in switches)
            {
                if (!CB_Nodes.ContainsKey(sw.InternalID))
                {
                    CB_Nodes.Add(sw.InternalID, new string[] { sw.FromNodeID, sw.ToNodeID });
                }
            }
        }

        private void TopologyProcessor()
        {
            topologyProcessor = new TopologyProcessor();
            foreach (KeyValuePair<string, string[]> kv in CB_Nodes)
            { 
                topologyProcessor.Append(kv);
            }
        }

        private void ObservabilityAnalysis()
        {
            
        }

        private void StateEstimator()
        {
            if (currentEstimator == null || breakerStatusChange || measurementStatusChange)
            {
                if (Mode.Equals("PositiveSequence"))
                {
                    currentEstimator = new CurrentEstimator(activeBreakerCurrentMeasurements, activeCurrentMeasurements, topologyProcessor);
                    currentEstimator.CompleteCurrentLSE();

                    voltageEstimator = new VoltageEstimator(activeVoltageMeasurements, topologyProcessor, nodes);
                    voltageEstimator.CompleteVoltageLSE();
                }
                if (Mode.Equals("ThreePhase"))
                {
                    currentEstimator = new CurrentEstimator(activeBreakerCurrentMeasurements, activeCurrentMeasurements, topologyProcessor);
                    currentEstimator.CompleteThreePhaseCurrentLSE();

                    voltageEstimator = new VoltageEstimator(activeVoltageMeasurements, topologyProcessor, nodes);
                    voltageEstimator.CompleteThreePhaseVoltageLSE();
                }
                //breakerStatusChange = false;
                //measurementStatusChange = false;
            }
            else
            {
                currentEstimator.FastCurrentLSE(activeBreakerCurrentMeasurements, activeCurrentMeasurements);
                voltageEstimator.FastVoltageLSE(activeVoltageMeasurements);
            }          
        }

        private void OutputInterface()
        {
            badDataList.Clear();
            foreach (KeyValuePair<string, HashSet<int>> kv in currentEstimator.BadDataList)
            {
                if (kv.Value == null)
                {
                    continue;
                }
                if (kv.Value.Count>0)
                {
                    Console.WriteLine("Island:   " + kv.Key);
                    Dictionary<int, string> measurementMapping = currentEstimator.MeasurementList[kv.Key];
                    foreach (int kw in kv.Value)
                    {
                        if (measurementMapping.ContainsKey(kw))
                        {
                            Console.WriteLine("Current Measurement Bad Data at:    " + measurementMapping[kw]);
                            badDataList.Add(measurementMapping[kw]);
                        }

                    }
                }

            }

            //if (voltageEstimator.BadDataList.Count>0)
            //{
            //    int i = 1;
            //    i = 3;
            //}

            foreach (KeyValuePair<string, HashSet<int>> kv in voltageEstimator.BadDataList)
            {
                if (kv.Value == null)
                {
                    continue;
                }

                if (kv.Value.Count>0)
                {
                    Console.WriteLine("Island:   " + kv.Key);
                    Dictionary<int, string> measurementMapping = voltageEstimator.MeasurementList[kv.Key];
                    foreach (int kw in kv.Value)
                    {
                        if (measurementMapping.ContainsKey(kw))
                        {
                            Console.WriteLine("Voltage Measurement Bad Data at:    " + measurementMapping[kw]);
                            badDataList.Add(measurementMapping[kw]);
                        }
                    }
                }

            }

            outputMeasurements.Clear();
            if (Mode.Equals("PositiveSequence"))
            {
                foreach (KeyValuePair<string, Dictionary<string, BreakerCurrentPhasorGroup>> kv in currentEstimator.IslandBreakerCurrentMeasurements)
                {
                    foreach (KeyValuePair<string, BreakerCurrentPhasorGroup> kw in kv.Value)
                    {
                        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.MagnitudeKey, kw.Value.PositiveSequence.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.AngleKey, kw.Value.PositiveSequence.Estimate.AngleInDegrees);
                        }
                    }
                }

                foreach (KeyValuePair<string, Dictionary<string, CurrentPhasorGroup>> kv in currentEstimator.IslandCurrentMeasurements)
                {
                    foreach (KeyValuePair<string, CurrentPhasorGroup> kw in kv.Value)
                    {
                        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.MagnitudeKey, kw.Value.PositiveSequence.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.AngleKey, kw.Value.PositiveSequence.Estimate.AngleInDegrees);
                        }
                    }
                }

                foreach (Node node in nodes)
                {            
                    if (inputMeasurements.ContainsKey(node.Voltage.PositiveSequence.Estimate.MagnitudeKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PositiveSequence.Estimate.MagnitudeKey))
                        {
                            if (!double.IsNaN(node.Voltage.PositiveSequence.Estimate.Magnitude))
                            outputMeasurements.Add(node.Voltage.PositiveSequence.Estimate.MagnitudeKey, node.Voltage.PositiveSequence.Estimate.Magnitude);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PositiveSequence.Estimate.AngleKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PositiveSequence.Estimate.AngleKey))
                        {
                            if (!double.IsNaN(node.Voltage.PositiveSequence.Estimate.AngleInDegrees))
                            outputMeasurements.Add(node.Voltage.PositiveSequence.Estimate.AngleKey, node.Voltage.PositiveSequence.Estimate.AngleInDegrees);
                        }
                    }      
                }
                //foreach (KeyValuePair<string, Dictionary<string, VoltagePhasorGroup>> kv in voltageEstimator.IslandVoltageMeasurements)
                //{
                //    foreach (KeyValuePair<string, VoltagePhasorGroup> kw in kv.Value)
                //    {
                //        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                //        {
                //            if (!outputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                //            {
                //                outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.MagnitudeKey, kw.Value.PositiveSequence.Estimate.Magnitude);
                //            }
                //        }
                //        if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                //        {
                //            if (!outputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                //            {
                //                outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.AngleKey, kw.Value.PositiveSequence.Estimate.AngleInDegrees);
                //            }
                //        }
                //    }
                //}
            }
            if (Mode.Equals("ThreePhase"))
            {
                foreach (KeyValuePair<string, Dictionary<string, BreakerCurrentPhasorGroup>> kv in currentEstimator.IslandBreakerCurrentMeasurements)
                {
                    foreach (KeyValuePair<string, BreakerCurrentPhasorGroup> kw in kv.Value)
                    {
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseA.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseA.Estimate.MagnitudeKey, kw.Value.PhaseA.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseA.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseA.Estimate.AngleKey, kw.Value.PhaseA.Estimate.AngleInDegrees);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseB.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseB.Estimate.MagnitudeKey, kw.Value.PhaseB.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseB.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseB.Estimate.AngleKey, kw.Value.PhaseB.Estimate.AngleInDegrees);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseC.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseC.Estimate.MagnitudeKey, kw.Value.PhaseC.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseC.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseC.Estimate.AngleKey, kw.Value.PhaseC.Estimate.AngleInDegrees);
                        }
                        //if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                        //{
                        //    outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.MagnitudeKey, kw.Value.PositiveSequence.Estimate.Magnitude);
                        //}
                        //if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                        //{
                        //    outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.AngleKey, kw.Value.PositiveSequence.Estimate.AngleInDegrees);
                        //}
                    }
                }

                foreach (KeyValuePair<string, Dictionary<string, CurrentPhasorGroup>> kv in currentEstimator.IslandCurrentMeasurements)
                {
                    foreach (KeyValuePair<string, CurrentPhasorGroup> kw in kv.Value)
                    {
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseA.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseA.Estimate.MagnitudeKey, kw.Value.PhaseA.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseA.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseA.Estimate.AngleKey, kw.Value.PhaseA.Estimate.AngleInDegrees);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseB.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseB.Estimate.MagnitudeKey, kw.Value.PhaseB.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseB.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseB.Estimate.AngleKey, kw.Value.PhaseB.Estimate.AngleInDegrees);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseC.Estimate.MagnitudeKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseC.Estimate.MagnitudeKey, kw.Value.PhaseC.Estimate.Magnitude);
                        }
                        if (inputMeasurements.ContainsKey(kw.Value.PhaseC.Estimate.AngleKey))
                        {
                            outputMeasurements.Add(kw.Value.PhaseC.Estimate.AngleKey, kw.Value.PhaseC.Estimate.AngleInDegrees);
                        }
                        //if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.MagnitudeKey))
                        //{
                        //    outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.MagnitudeKey, kw.Value.PositiveSequence.Estimate.Magnitude);
                        //}
                        //if (inputMeasurements.ContainsKey(kw.Value.PositiveSequence.Estimate.AngleKey))
                        //{
                        //    outputMeasurements.Add(kw.Value.PositiveSequence.Estimate.AngleKey, kw.Value.PositiveSequence.Estimate.AngleInDegrees);
                        //}
                    }
                }

                foreach (Node node in nodes)
                {
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseA.Estimate.MagnitudeKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseA.Estimate.MagnitudeKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseA.Estimate.Magnitude))
                                outputMeasurements.Add(node.Voltage.PhaseA.Estimate.MagnitudeKey, node.Voltage.PhaseA.Estimate.Magnitude);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseA.Estimate.AngleKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseA.Estimate.AngleKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseA.Estimate.AngleInDegrees))
                                outputMeasurements.Add(node.Voltage.PhaseA.Estimate.AngleKey, node.Voltage.PhaseA.Estimate.AngleInDegrees);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseB.Estimate.MagnitudeKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseB.Estimate.MagnitudeKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseB.Estimate.Magnitude))
                                outputMeasurements.Add(node.Voltage.PhaseB.Estimate.MagnitudeKey, node.Voltage.PhaseB.Estimate.Magnitude);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseB.Estimate.AngleKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseB.Estimate.AngleKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseB.Estimate.AngleInDegrees))
                                outputMeasurements.Add(node.Voltage.PhaseB.Estimate.AngleKey, node.Voltage.PhaseB.Estimate.AngleInDegrees);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseC.Estimate.MagnitudeKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseC.Estimate.MagnitudeKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseC.Estimate.Magnitude))
                                outputMeasurements.Add(node.Voltage.PhaseC.Estimate.MagnitudeKey, node.Voltage.PhaseC.Estimate.Magnitude);
                        }
                    }
                    if (inputMeasurements.ContainsKey(node.Voltage.PhaseC.Estimate.AngleKey))
                    {
                        if (!outputMeasurements.ContainsKey(node.Voltage.PhaseC.Estimate.AngleKey))
                        {
                            if (!double.IsNaN(node.Voltage.PhaseC.Estimate.AngleInDegrees))
                                outputMeasurements.Add(node.Voltage.PhaseC.Estimate.AngleKey, node.Voltage.PhaseC.Estimate.AngleInDegrees);
                        }
                    }
                }
            }

        }

        private void InsertSubstationMeasurements()
        {
            double value = 0;
            if (Mode.Equals("PositiveSequence"))
            {
                foreach (Node node in nodes)
                {
                    if (inputMeasurements.TryGetValue(node.Voltage.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        node.Voltage.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        node.Voltage.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }

                foreach (CircuitBreaker cb in circuitBreakers)
                {
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        cb.BreakerCurrent.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        cb.BreakerCurrent.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }


                foreach (TransmissionLine line in transmissionLines)
                {
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        line.LineCurrent.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        line.LineCurrent.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }

                foreach (Transformer XF in transformers)
                {
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        XF.FromNodeCurrent.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        XF.FromNodeCurrent.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        XF.ToNodeCurrent.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        XF.ToNodeCurrent.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }

                foreach (ShuntCompensator shunt in shunts)
                {
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                    {
                        shunt.ShuntCurrent.PositiveSequence.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PositiveSequence.Measurement.AngleKey, out value))
                    {
                        shunt.ShuntCurrent.PositiveSequence.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
            }
            if (Mode.Equals("ThreePhase"))
            {
                foreach (Node node in nodes)
                {
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        node.Voltage.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseA.Measurement.AngleKey, out value))
                    {
                        node.Voltage.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        node.Voltage.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseB.Measurement.AngleKey, out value))
                    {
                        node.Voltage.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        node.Voltage.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(node.Voltage.PhaseC.Measurement.AngleKey, out value))
                    {
                        node.Voltage.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
                foreach (CircuitBreaker cb in circuitBreakers)
                {
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        cb.BreakerCurrent.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseA.Measurement.AngleKey, out value))
                    {
                        cb.BreakerCurrent.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        cb.BreakerCurrent.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseB.Measurement.AngleKey, out value))
                    {
                        cb.BreakerCurrent.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        cb.BreakerCurrent.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(cb.BreakerCurrent.PhaseC.Measurement.AngleKey, out value))
                    {
                        cb.BreakerCurrent.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
                foreach (TransmissionLine line in transmissionLines)
                {
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        line.LineCurrent.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseA.Measurement.AngleKey, out value))
                    {
                        line.LineCurrent.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        line.LineCurrent.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseB.Measurement.AngleKey, out value))
                    {
                        line.LineCurrent.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        line.LineCurrent.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(line.LineCurrent.PhaseC.Measurement.AngleKey, out value))
                    {
                        line.LineCurrent.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
                foreach (Transformer XF in transformers)
                {
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseA.Measurement.AngleKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseA.Measurement.AngleKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseB.Measurement.AngleKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseB.Measurement.AngleKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PhaseC.Measurement.AngleKey, out value))
                    {
                        XF.FromNodeCurrent.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PhaseC.Measurement.AngleKey, out value))
                    {
                        XF.ToNodeCurrent.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
                foreach (ShuntCompensator shunt in shunts)
                {
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseA.Measurement.MagnitudeKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseA.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseA.Measurement.AngleKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseA.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseB.Measurement.MagnitudeKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseB.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseB.Measurement.AngleKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseB.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseC.Measurement.MagnitudeKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseC.Measurement.Magnitude = value;
                        value = 0;
                    }
                    if (inputMeasurements.TryGetValue(shunt.ShuntCurrent.PhaseC.Measurement.AngleKey, out value))
                    {
                        shunt.ShuntCurrent.PhaseC.Measurement.AngleInDegrees = value;
                        value = 0;
                    }
                }
            }

        }

        private void DetermineActiveMeasurements()
        {
            activeBreakerCurrentMeasurements.Clear();
            activeCurrentMeasurements.Clear();
            activeVoltageMeasurements.Clear();

            if (Mode.Equals("PositiveSequence"))
            {
                foreach (VoltagePhasorGroup voltagePhasorGroup in voltageMeasurements)
                {
                    if (voltagePhasorGroup.IncludeInPositiveSequenceEstimator)
                    {
                        activeVoltageMeasurements.Add(voltagePhasorGroup.MeasuredNodeID, voltagePhasorGroup);
                    }
                }
                foreach (CurrentPhasorGroup currentPhasorGroup in currentMeasurements)
                {
                    if (currentPhasorGroup.IncludeInPositiveSequenceEstimator)
                    {
                        activeCurrentMeasurements.Add(currentPhasorGroup.MeasuredNodeID, currentPhasorGroup);
                    }
                }
                foreach (BreakerCurrentPhasorGroup breakerCurrent in breakerCurrentMeasurements)
                {
                    if (breakerCurrent.IncludeInPositiveSequenceEstimator)
                    {
                        //activeBreakerCurrentMeasurements.Add(breakerCurrent.FromNodeID, breakerCurrent);
                        //activeBreakerCurrentMeasurements.Add(breakerCurrent.ToNodeID, breakerCurrent); 
                        activeBreakerCurrentMeasurements.Add(breakerCurrent.MeasuredBreakerID, breakerCurrent);
                    }
                }
            }
            if (Mode.Equals("ThreePhase"))
            {
                foreach (VoltagePhasorGroup voltagePhasorGroup in voltageMeasurements)
                {
                    if (voltagePhasorGroup.IncludeInEstimator)
                    {
                        activeVoltageMeasurements.Add(voltagePhasorGroup.MeasuredNodeID, voltagePhasorGroup);
                    }
                }
                foreach (CurrentPhasorGroup currentPhasorGroup in currentMeasurements)
                {
                    if (currentPhasorGroup.IncludeInEstimator)
                    {
                        activeCurrentMeasurements.Add(currentPhasorGroup.MeasuredNodeID, currentPhasorGroup);
                    }
                }
                foreach (BreakerCurrentPhasorGroup breakerCurrent in breakerCurrentMeasurements)
                {
                    if (breakerCurrent.IncludeInEstimator)
                    {
                        //activeBreakerCurrentMeasurements.Add(breakerCurrent.FromNodeID, breakerCurrent);
                        //activeBreakerCurrentMeasurements.Add(breakerCurrent.ToNodeID, breakerCurrent); 
                        activeBreakerCurrentMeasurements.Add(breakerCurrent.MeasuredBreakerID, breakerCurrent);
                    }
                }
            }
        }

        #endregion

        #region [Public Methods]

        public void Initialize()
        {
            LinkHierarchicalReferences();
            ListSubstationMeasurements();
            SubstationConfig();        
        }     

        //public void MeasurementReceived()
        //{ 
        //    inputMeasurements.Clear();
        //    // add input here


        //}

        public void SLSE()
        {
            InsertSubstationMeasurements();
            DetermineActiveMeasurements();
            if (breakerStatusChange || measurementStatusChange)
            {
               DetermineBreakerStatus();
               TopologyProcessor();
               ObservabilityAnalysis();
            }
            StateEstimator();
            OutputInterface();
        }



        public Substation DeserializeFromXml(string pathName)
        {
            try
            {
                // Create an empy NetworkMeasurements object reference.
                Substation substation = null;

                // Create an XmlSerializer with the type of NetworkMeasurements.
                XmlSerializer deserializer = new XmlSerializer(typeof(Substation));

                // Read the data in from the file.
                StreamReader reader = new StreamReader(pathName);

                // Cast the deserialized data as a NetworkMeasurements object.
                substation = (Substation)deserializer.Deserialize(reader);

                // Close the connection.
                reader.Close();

                return substation;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to Deserialize the Network from the Configuration File: " + exception.ToString());
            }
        }

        #endregion
    }
}
