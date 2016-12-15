//******************************************************************************************************
//  
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      October 1st 2015
//
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Numerics;
using SubstationLSE.Measurements;

namespace SubstationLSE
{
    public class Transformer
    {
        #region [ Private Constants ]

        /// <summary>
        /// Default values
        /// </summary>
        private const string DEFAULT_INTERNAL_ID = "0";
        private const string DEFAULT_NUMBER = "0";
        private const string DEFAULT_NAME = "Undefined";
        private const string DEFAULT_DESCRIPTION = "Uundefined";

        #endregion

        #region [Private Members]
        private string m_internalID;
        private Node fromNode;
        private Node toNode;
        private string fromNodeID;
        private string toNodeID;

        private Impedance m_impedance; 
        private TapConfiguration m_tapConfiguration;

        private int m_fixedTapPosition;
        private string m_tapPositionInputKey;
        private int m_tapPositionMeasurement;
        private string m_tapPositionOutputKey;
        private bool m_ultcIsEnabled;

        private TransformerConnectionType m_fromNodeConnectionType;
        private TransformerConnectionType m_toNodeConnectionType;


        private CurrentPhasorGroup m_fromNodeCurrent;
        private string m_fromNodeCurrentPhasorGroupID;
        private CurrentPhasorGroup m_toNodeCurrent;
        private string m_toNodeCurrentPhasorGroupID;

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
        /// 
        /// </summary>
        [XmlAttribute("FromNode")]
        public string FromNodeID
        {
            get
            {
                return fromNodeID;
            }
            set
            {
                fromNodeID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("ToNode")]
        public string ToNodeID
        {
            get
            {
                return toNodeID;
            }
            set
            {
                toNodeID = value;
            }
        }

        public Node FromNode
        {
            get
            {
                return fromNode;
            }
            set
            {
                fromNode = value;
                fromNodeID = value.InternalID;
            }
        }

        public Node ToNode
        {
            get
            {
                return toNode;
            }
            set
            {
                toNode = value;
                toNodeID = value.InternalID;
            }
        }

        /// <summary>
        /// The raw singe value impedance parameters for resistance, reactance, and susceptance.
        /// </summary>
        [XmlElement("Impedance")]
        public Impedance RawImpedanceParameters
        {
            get
            {
                return m_impedance;
            }
            set
            {
                m_impedance = value;
            }
        }

        [XmlElement("TapConfiguration")]
        public TapConfiguration Tap
        {
            get
            {
                return m_tapConfiguration;
            }
            set
            {
                m_tapConfiguration = value;
            }
        }

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Measurements.CurrentFlowPhasorGroup"/> that originates at the <see cref="SynchrophasorAnalytics.Modeling.ITwoTerminal.FromNode"/>
        /// </summary>
        [XmlElement("FromNodeCurrent")]
        public CurrentPhasorGroup FromNodeCurrent
        {
            get
            {
                return m_fromNodeCurrent;
            }
            set
            {
                m_fromNodeCurrent = value;
                m_fromNodeCurrentPhasorGroupID = value.InternalID;
                //if (this.FromNode != null)
                //{
                //    m_fromNodeCurrent.FromNode = this.FromNode;
                //    m_fromNodeCurrent.ToNode = this.ToNode;
                //}
            }
        }

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup.InternalID"/> of the <see cref="SynchrophasorAnalytics.Modeling.Transformer.FromNodeCurrent"/>.
        /// </summary>
        [XmlAttribute("FromNodeCurrentPhasorGroup")]
        public string FromNodeCurrentPhasorGroupID
        {
            get
            {
                return m_fromNodeCurrentPhasorGroupID;
            }
            set
            {
                m_fromNodeCurrentPhasorGroupID = value;
            }
        }

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Measurements.CurrentFlowPhasorGroup"/> that originates at the <see cref="SynchrophasorAnalytics.Modeling.SeriesBranchBase.ToNode"/> of the <see cref="SynchrophasorAnalytics.Modeling.Transformer"/>
        /// </summary>
        [XmlElement("ToNodeCurrent")]
        public CurrentPhasorGroup ToNodeCurrent
        {
            get
            {
                return m_toNodeCurrent;
            }
            set
            {
                m_toNodeCurrent = value;
                m_toNodeCurrentPhasorGroupID = value.InternalID;
                if (this.ToNode != null)
                {
                    //m_toNodeCurrent.MeasuredFromNode = this.ToNode;
                    //m_toNodeCurrent.MeasuredToNode = this.FromNode;
                }
            }
        }

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup.InternalID"/> of the <see cref="SynchrophasorAnalytics.Modeling.Transformer.ToNodeCurrent"/>.
        /// </summary>
        [XmlAttribute("ToNodeCurrentPhasorGroup")]
        public string ToNodeCurrentPhasorGroupID
        {
            get
            {
                return m_toNodeCurrentPhasorGroupID;
            }
            set
            {
                m_toNodeCurrentPhasorGroupID = value;
            }
        }

        /// <summary>
        /// The input measurement key for the tap position
        /// </summary>
        [XmlAttribute("TapPositionInputKey")]
        public string TapPositionInputKey
        {
            get
            {
                return m_tapPositionInputKey;
            }
            set
            {
                m_tapPositionInputKey = value;
            }
        }

        /// <summary>
        /// The input measurement for the tap position
        /// </summary>
        [XmlIgnore()]
        public int TapPositionMeasurement
        {
            get
            {
                return m_tapPositionMeasurement;
            }
            set
            {
                m_tapPositionMeasurement = value;
                if (m_tapConfiguration != null)
                {
                    m_tapConfiguration.CurrentPosition = value;
                }
            }
        }

        /// <summary>
        /// The output measurement key for the tap position
        /// </summary>
        [XmlAttribute("TapPositionOutputKey")]
        public string TapPositionOutputKey
        {
            get
            {
                return m_tapPositionOutputKey;
            }
            set
            {
                m_tapPositionOutputKey = value;
            }
        }

        /// <summary>
        /// A tap position used to indicate when a transformer tap is permanently or semi-permantly fixed into a particular position
        /// </summary>
        [XmlAttribute("FixedTapPosition")]
        public int FixedTapPosition
        {
            get
            {
                return m_fixedTapPosition;
            }
            set
            {
                m_fixedTapPosition = value;
            }
        }

        /// <summary>
        /// A flag which determines whether the transformer should accept measurements to determine what the actual tap setting is.
        /// </summary>
        [XmlAttribute("EnableUltc")]
        public bool UltcIsEnabled
        {
            get
            {
                return m_ultcIsEnabled;
            }
            set
            {
                m_ultcIsEnabled = value;
            }
        }
        #endregion

        #region [ Constructors ]

        /// <summary>
        /// A default constructor with default values.
        /// </summary>
        public Transformer()
            : this(DEFAULT_INTERNAL_ID, DEFAULT_NUMBER, DEFAULT_NAME, DEFAULT_DESCRIPTION, new Impedance())
        {
        }

        /// <summary>
        /// A constructor for the <see cref="SynchrophasorAnalytics.Modeling.Transformer"/> class which requires all of the properties defined by the <see cref="SynchrophasorAnalytics.Modeling.INetworkDescribable"/> interface as well as the nominal <see cref="SynchrophasorAnalytics.Modeling.Impedance"/> values.
        /// </summary>
        /// <param name="internalID">A unique integer identifier</param>
        /// <param name="number">A non-unique integer identifier</param>
        /// <param name="name">A string name for the object</param>
        /// <param name="description">A string description for the object</param>
        /// <param name="impedance">The nominal impedance for the transformer.</param>
        public Transformer(string internalID, string number, string name, string description, Impedance impedance)
            : this(internalID, number, name, description, impedance, "0", "0")
        {
        }

        /// <summary>
        /// A constructor for the <see cref="SynchrophasorAnalytics.Modeling.Transformer"/> class which requires all of the properties defined by the <see cref="SynchrophasorAnalytics.Modeling.INetworkDescribable"/> interface as well as the nominal <see cref="SynchrophasorAnalytics.Modeling.Impedance"/> values as well as the internal id of the from and to node current flow measurements.
        /// </summary>
        /// <param name="internalID">A unique integer identifier</param>
        /// <param name="number">A non-unique integer identifier</param>
        /// <param name="name">A string name for the object</param>
        /// <param name="description">A string description for the object</param>
        /// <param name="impedance">The nominal impedance for the transformer.</param>
        /// <param name="fromNodeCurrentPhasorGroupID">The internal id of the current flow phasor group on the from node.</param>
        /// <param name="toNodeCurrentPhasorGroupID">The internal id of the current flow phasor group on the to node.</param>
        public Transformer(string internalID, string number, string name, string description, Impedance impedance, string fromNodeCurrentPhasorGroupID, string toNodeCurrentPhasorGroupID)
            : this(internalID, number, name, description, impedance, fromNodeCurrentPhasorGroupID, toNodeCurrentPhasorGroupID, new TapConfiguration() )
        {
        }

        /// <summary>
        /// A constructor for the <see cref="SynchrophasorAnalytics.Modeling.Transformer"/> class which requires all of the properties defined by the <see cref="SynchrophasorAnalytics.Modeling.INetworkDescribable"/> interface as well as the nominal <see cref="SynchrophasorAnalytics.Modeling.Impedance"/> values as well as the internal id of the from and to node current flow measurements and the tap model of the transformer.
        /// </summary>
        /// <param name="internalID">A unique integer identifier</param>
        /// <param name="number">A non-unique integer identifier</param>
        /// <param name="name">A string name for the object</param>
        /// <param name="description">A string description for the object</param>
        /// <param name="impedance">The nominal impedance for the transformer.</param>
        /// <param name="fromNodeCurrentPhasorGroupID">The internal id of the current flow phasor group on the from node.</param>
        /// <param name="toNodeCurrentPhasorGroupID">The internal id of the current flow phasor group on the to node.</param>
        /// <param name="tapConfigurationID">The tap model of the transformer.</param>
        public Transformer(string internalID, string number, string name, string description, Impedance impedance, string fromNodeCurrentPhasorGroupID, string toNodeCurrentPhasorGroupID, TapConfiguration tapConfiguration)          
        {
            m_internalID = internalID;
            //m_number = number;
            //m_name = name;
            //m_description = description;
            m_impedance = impedance;
            m_fromNodeCurrentPhasorGroupID = fromNodeCurrentPhasorGroupID;
            m_toNodeCurrentPhasorGroupID = toNodeCurrentPhasorGroupID;
            m_tapConfiguration = tapConfiguration;
            m_fromNodeConnectionType = TransformerConnectionType.Wye;
            m_toNodeConnectionType = TransformerConnectionType.Wye;
            m_tapPositionInputKey = "Undefined";
            m_tapPositionOutputKey = "Undefined";
            //m_complexPowerHasBeenInitialized = false;
            //this.RawImpedanceParameters.ParentElement = this;
        }

        #endregion

        #region [ Private Methods ]

        private Complex ComputeComplexMultiplierA()
        {
            double phaseShift = ComputePhaseShift();
            double offNominalTapRatio = ComputeOffNominalTapRatio();

            return offNominalTapRatio * (new Complex(Math.Cos(phaseShift), Math.Sin(phaseShift)));
        }

        private double ComputePhaseShift()
        {
            // From side is considered primary side. ANSI Standards dictate primary side voltage always leads secondary for delta-wye
            if (m_fromNodeConnectionType == TransformerConnectionType.Delta && m_toNodeConnectionType == TransformerConnectionType.Delta)
            {
                return 0;
            }
            else if (m_fromNodeConnectionType == TransformerConnectionType.Wye && m_toNodeConnectionType == TransformerConnectionType.Wye)
            {
                return 0;
            }
            else if (m_fromNodeConnectionType == TransformerConnectionType.Delta && m_toNodeConnectionType == TransformerConnectionType.Wye)
            {
                return 30;
            }
            else if (m_fromNodeConnectionType == TransformerConnectionType.Wye && m_toNodeConnectionType == TransformerConnectionType.Delta)
            {
                return 30;
            }
            else
            {
                return 0;
            }
        }

        private double ComputeOffNominalTapRatio()
        {
            if (m_ultcIsEnabled)
            {
                m_tapConfiguration.CurrentPosition = m_tapPositionMeasurement;
            }
            else
            {
                if (m_tapConfiguration != null)
                {
                    m_tapConfiguration.CurrentPosition = m_fixedTapPosition;
                }
            }
            return m_tapConfiguration.CurrentMultiplier;
        }

        //private Impedance ComputeOffNominalImpedance()
        //{
        //    Impedance offNominalImpedance = new Impedance() { ParentElement = this };

        //    // Scale the series impedance (R + jX) * a
        //    offNominalImpedance.ThreePhaseSeriesImpedance = NominalImpedance.ThreePhaseSeriesImpedance.Multiply(EffectiveComplexMultiplier.Magnitude);

        //    // Compute the equivalent shunt susceptance on the from side of the transformer
        //    offNominalImpedance.ThreePhaseFromSideShuntSusceptance = NominalImpedance.ThreePhaseSeriesAdmittance.Multiply(((1 - EffectiveComplexMultiplier.Magnitude) / (EffectiveComplexMultiplier.Magnitude * EffectiveComplexMultiplier.Magnitude)));

        //    // Compute the equivalent shunt susceptance on the to side of the transformer
        //    offNominalImpedance.ThreePhaseToSideShuntSusceptance = NominalImpedance.ThreePhaseSeriesAdmittance.Multiply(((EffectiveComplexMultiplier.Magnitude - 1) / EffectiveComplexMultiplier.Magnitude));

        //    return offNominalImpedance;
        //}

        #endregion
    }
}
