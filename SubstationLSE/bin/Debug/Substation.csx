﻿//******************************************************************************************************
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
        Dictionary<string, string[]> EQ_Nodes = new Dictionary<string, string[]>();

        //Measurements
        Dictionary<string, double> inputMeasurements = new Dictionary<string, double>(); 
        List<VoltagePhasorGroup> voltageMeasurements = new List<VoltagePhasorGroup>();
        List<CurrentPhasorGroup> currentMeasurements = new List<CurrentPhasorGroup>();
        List<VoltagePhasorGroup> activeVoltageMeasurements = new List<VoltagePhasorGroup>();
        List<CurrentPhasorGroup> activeCurrentMeasurements = new List<CurrentPhasorGroup>();

        //Algorithms
        TopologyProcessor topologyProcessor = new TopologyProcessor();
        CurrentEstimator currentEstimator = new CurrentEstimator();
        VoltageEstimator voltageEstimator = new VoltageEstimator();

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
                    value = null;
                }
                if (localNodes.TryGetValue(circuitBreaker.ToNodeID, out value))
                {
                    circuitBreaker.ToNode = value;
                    value = null;
                }
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
        }
        
        private void TopologyProcessor()
        {
            foreach (KeyValuePair<string, string[]> kv in CB_Nodes)
            { 
                topologyProcessor.Append(kv);
            }
        }

        private void ObservabilityAnalysis()
        {
            DetermineActiveMeasurements();
        }

        private void InsertSubstationMeasurements()
        {
            double value = 0; 
            foreach (Node node in nodes)
            {
                if (inputMeasurements.TryGetValue(node.Voltage.PositiveSequence.Measurement.MagnitudeKey, out value))
                {
                    node.Voltage.PositiveSequence.Measurement.Magnitude = value;
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
            }

            foreach (Transformer XF in transformers)
            {
                if (inputMeasurements.TryGetValue(XF.FromNodeCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                {
                    XF.FromNodeCurrent.PositiveSequence.Measurement.Magnitude = value;
                    value = 0;
                }
                if (inputMeasurements.TryGetValue(XF.ToNodeCurrent.PositiveSequence.Measurement.MagnitudeKey, out value))
                {
                    XF.ToNodeCurrent.PositiveSequence.Measurement.Magnitude = value;
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
            }
        }

        private void DetermineActiveMeasurements()
        {
            activeCurrentMeasurements.Clear();
            activeVoltageMeasurements.Clear();

            foreach (VoltagePhasorGroup voltagePhasorGroup in voltageMeasurements)
            {
                if (voltagePhasorGroup.IncludeInPositiveSequenceEstimator)
                {
                    activeVoltageMeasurements.Add(voltagePhasorGroup);
                }
            }
            foreach (CurrentPhasorGroup currentPhasorGroup in currentMeasurements)
            {
                if (currentPhasorGroup.IncludeInPositiveSequenceEstimator)
                {
                    activeCurrentMeasurements.Add(currentPhasorGroup);
                }
            }
        }

        #endregion

        #region [Public Methods]

        public void Initialize()
        {

            LinkHierarchicalReferences();
            ListSubstationMeasurements();

            foreach (CircuitBreaker breaker in circuitBreakers)
            {
                CB_Nodes.Add(breaker.InternalID, new string[] { breaker.FromNodeID, breaker.ToNodeID });
            }

            foreach (Switch sw in switches)
            {
                //if (!CB_Nodes.ContainsKey(sw.InternalID))
                { 
                    CB_Nodes.Add(sw.InternalID, new string[] { sw.FromNodeID, sw.ToNodeID }); 
                }            
            }

            foreach (Node node in nodes)
            {
                //if (node.)
            }

            foreach (TransmissionLine line in transmissionLines)
            {
                EQ_Nodes.Add(line.InternalID, new string[] { line.ConnectedNodeID});
            }

            foreach(Transformer xf in transformers)
            {
                EQ_Nodes.Add(xf.InternalID, new string[] { xf.FromNodeID, xf.ToNodeID });
            }

            foreach (Generator unit in generators)
            {
                EQ_Nodes.Add(unit.InternalID, new string[] { unit.ConnectedNodeID });
            }

            foreach (ShuntCompensator shunt in shunts)
            {
                EQ_Nodes.Add(shunt.InternalID, new string[] { shunt.ConnectedNodeID });
            }


            

        }

        public void MeasurementReceived()
        { 
            inputMeasurements.Clear();

            InsertSubstationMeasurements();
        }


        public void SLSE()
        {
            TopologyProcessor();
            ObservabilityAnalysis();

            Transformer xf = new Transformer("0","0","0","0", new Impedance());
        }

        #endregion
    }
}
