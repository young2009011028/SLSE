
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// Encapsulates a group of phasors measuring a flow in a +, A, B, C grouping and relates them to the network model.
    /// </summary>
    /// <seealso cref="SynchrophasorAnalytics.Measurements.CurrentFlowPhasorGroup"/>
    /// <seealso cref="SynchrophasorAnalytics.Measurements.CurrentInjectionPhasorGroup"/>
    /// <seealso cref="SynchrophasorAnalytics.Measurements.VoltagePhasorGroup"/>
    /// <seealso cref="SynchrophasorAnalytics.Measurements.PhasorType"/>
    [XmlRoot("PhasorGroup")]
    public class PhasorGroup  
    {
        #region [ Constants ]

        private const int ZERO_SEQUENCE = 0;
        private const int POSITIVE_SEQUENCE = 1;
        private const int NEGATIVE_SEQUENCE = 2;
        private const string DEFAULT_INTERNAL_ID = "0";
        private const string DEFAULT_NUMBER = "0";
        private const string DEFAULT_NAME = "Undefined";
        private const string DEFAULT_DESCRIPTION = "Undefined";

        #endregion

        #region [ Private Members ]

        /// <summary>
        /// INetworkDescribable fields
        /// </summary>
        private string m_uniqueId;
        private string m_internalID;
        private string m_number;
        private string m_acronym;
        private string m_name;
        private string m_description;

        private bool m_enabled;

        private bool m_useStatusFlagForRemovingMeasurements;
        //private StatusWord m_statusWord;
        private string m_statusWordID;

        private Phasor m_posSeq;
        private Phasor m_negSeq;
        private Phasor m_zeroSeq;
        private Phasor m_phaseA;
        private Phasor m_phaseB;
        private Phasor m_phaseC;

        //private DoubleComplex ALPHA;
        //private ComplexMatrix A_INVERSE;

        private string m_negativeSequenceToPositiveSequenceRatioKey;
        private bool m_shouldSerializeEstimatedNegativeSequenceToPositiveSequenceRatio;

        #endregion


        #region [ Constructors ]

        /// <summary>
        /// A blank constructor with default values.
        /// </summary>
        public PhasorGroup()
            : this(DEFAULT_INTERNAL_ID, DEFAULT_NUMBER, DEFAULT_NAME, DEFAULT_DESCRIPTION)
        {
        }

        /// <summary>
        /// A constructor which specifies only the information required by the <see cref="SynchrophasorAnalytics.Modeling.INetworkDescribable"/> interface. The <see cref="Phasor"/> objects for and <see cref="StatusWord"/> objects are instantiated with default initializers to prevent null references.
        /// </summary>
        /// <param name="internalID">The unique integer identifier for each instance of a <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="number">A descriptive number for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="name">A descriptive name for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="description">A description of the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        public PhasorGroup(string internalID, string number, string name, string description)
            : this(internalID, number, name, description, new Phasor(), new Phasor(), new Phasor(), new Phasor())
        {
        }


        /// <summary>
        /// The designated constructor for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> class.
        /// </summary>
        /// <param name="internalID">The unique integer identifier for each instance of a <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="number">A descriptive number for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="name">A descriptive name for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="description">A description of the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.</param>
        /// <param name="positiveSequence">The <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> representing positive sequence.</param>
        /// <param name="phaseA">The <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> representing phase A.</param>
        /// <param name="phaseB">The <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> representing phase B.</param>
        /// <param name="phaseC">The <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> representing phase C.</param>
        /// <param name="statusWord">The <see cref="SynchrophasorAnalytics.Measurements.StatusWord"/> from the source device for the <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> objects in this <see cref="PhasorGroup"/>.</param>
        public PhasorGroup(string internalID, string number, string name, string description, Phasor positiveSequence, Phasor phaseA, Phasor phaseB, Phasor phaseC)
        {
            m_internalID = internalID;
            m_number = number;
            m_name = name;
            m_description = description;
            m_posSeq = positiveSequence;
            m_negSeq = new Phasor();
            m_negSeq.Measurement.VoltageLevel = m_posSeq.Measurement.VoltageLevel;
            m_negSeq.Estimate.VoltageLevel = m_posSeq.Estimate.VoltageLevel;
            m_zeroSeq = new Phasor();
            m_zeroSeq.Measurement.VoltageLevel = m_posSeq.Measurement.VoltageLevel;
            m_zeroSeq.Estimate.VoltageLevel = m_posSeq.Estimate.VoltageLevel;
            m_phaseA = phaseA;
            m_phaseB = phaseB;
            m_phaseC = phaseC;
            m_negativeSequenceToPositiveSequenceRatioKey = "Undefined";

            InitializeDefaultParameters();
        }

        #endregion


        #region [ Properties ]

        /// <summary>
        /// A statistically unique identifier for the instance of the class.
        /// </summary>
        [XmlAttribute("Uid")]
        public string UniqueId
        {
            get
            {
                //if (m_uniqueId == Guid.Empty)
                //{
                //    m_uniqueId = Guid.NewGuid();
                //}
                return m_uniqueId;
            }
            set
            {
                m_uniqueId = value;
            }
        }

        /// <summary>
        /// The unique integer identifier for each instance of a <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.
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
        /// A descriptive number for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.
        /// </summary>
        [XmlAttribute("Number")]
        public string Number
        {
            get 
            { 
                return m_number; 
            }
            set 
            {
                m_number = value; 
            }
        }

        /// <summary>
        /// A descriptive acronym for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.
        /// </summary>
        [XmlAttribute("Acronym")]
        public string Acronym
        {
            get 
            { 
                return m_acronym; 
            }
            set 
            { 
                m_acronym = value; 
            }
        }

        /// <summary>
        /// A name for the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name
        {
            get 
            { 
                return m_name; 
            }
            set 
            { 
                m_name = value; 
            }
        }

        /// <summary>
        /// A description of the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/> object.
        /// </summary>
        [XmlAttribute("Description")]
        public string Description
        {
            get 
            { 
                return m_description;
            }
            set 
            { 
                m_description = value; 
            }
        }

        /// <summary>
        /// Returns the type of the object as a string.
        /// </summary>
        [XmlIgnore()]
        public string ElementType
        {
            get 
            { 
                return this.GetType().ToString(); 
            }
        }

        /// <summary>
        /// A flag that represents whether the measurement is enabled.
        /// </summary>
        [XmlAttribute("Enabled")]
        public bool IsEnabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
            }
        }

        /// <summary>
        /// A flag which represents whether to use the <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup.Status"/>to determine whether or not to include the phasor group in the estimator.
        /// </summary>
        [XmlAttribute("UseStatusFlag")]
        public bool UseStatusFlagForRemovingMeasurements
        {
            get 
            { 
                return m_useStatusFlagForRemovingMeasurements; 
            }
            set 
            {
                m_useStatusFlagForRemovingMeasurements = value; 
            }
        }


        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Measurements.Phasor"/> representing the positive sequence phasor in this <see cref="SynchrophasorAnalytics.Measurements.PhasorGroup"/>.
        /// </summary>
        [XmlElement("PositiveSequence")]
        public Phasor PositiveSequence
        {
            get 
            { 
                return m_posSeq; 
            }
            set 
            { 
                m_posSeq = value; 
            }
        }

        /// <summary>
        /// The <see cref="Phasor"/> representing the negative sequence phasor in this <see cref="PhasorGroup"/>.
        /// </summary>
        [XmlIgnore()]
        public Phasor NegativeSequence
        {
            get 
            { 
                return m_negSeq; 
            }
            set 
            { 
                m_negSeq = value; 
            }
        }

        /// <summary>
        /// The <see cref="Phasor"/> representing the zero sequence phasor in this <see cref="PhasorGroup"/>.
        /// </summary>
        [XmlIgnore()]
        public Phasor ZeroSequence
        {
            get 
            { 
                return m_zeroSeq; 
            }
            set 
            { 
                m_zeroSeq = value; 
            }
        }

        /// <summary>
        /// The <see cref="Phasor"/> representing the A phase phasor in this <see cref="PhasorGroup"/>.
        /// </summary>
        [XmlElement("PhaseA")]
        public Phasor PhaseA
        {
            get 
            { 
                return m_phaseA; 
            }
            set 
            { 
                m_phaseA = value; 
            }
        }

        /// <summary>
        /// The <see cref="Phasor"/> representing the B phase phasor in this <see cref="PhasorGroup"/>.
        /// </summary>
        [XmlElement("PhaseB")]
        public Phasor PhaseB
        {
            get 
            { 
                return m_phaseB; 
            }
            set 
            { 
                m_phaseB = value; 
            }
        }

        /// <summary>
        /// The <see cref="Phasor"/> representing the C phase phasor in this <see cref="PhasorGroup"/>.
        /// </summary>
        [XmlElement("PhaseC")]
        public Phasor PhaseC
        {
            get 
            { 
                return m_phaseC; 
            }
            set 
            { 
                m_phaseC = value; 
            }
        }

        /// <summary>
        /// The ratio of negative sequence to positive sequence calculated with phase measurements.
        /// </summary>
        [XmlIgnore()]
        public double MeasuredNegativeSequenceToPositiveSequenceRatio
        {
            get
            {
                return m_negSeq.Measurement.Magnitude / m_posSeq.Measurement.Magnitude; 
            }
        }

        /// <summary>
        /// The ratio of negative sequence to positive sequence calculated with phase estimates.
        /// </summary>
        [XmlAttribute("I2I1Ratio")]
        public double EstimatedNegativeSequenceToPositiveSequenceRatio
        {
            get
            {
                return m_negSeq.Estimate.Magnitude / m_posSeq.Estimate.Magnitude;
            }
            set { }
        }

        /// <summary>
        /// A property which specifies whether the magnitude and angle data for the measurements and estimates should also be serialized when the object is serialized.
        /// </summary>
        [XmlIgnore()]
        public bool ShouldSerializeData
        {
            get
            {
                return m_shouldSerializeEstimatedNegativeSequenceToPositiveSequenceRatio;
            }
            set
            {
                m_shouldSerializeEstimatedNegativeSequenceToPositiveSequenceRatio = value;
            }
        }

        /// <summary>
        /// The ratio of zero sequence to positive sequence calculated with phase measurements.
        /// </summary>
        [XmlIgnore()]
        public double MeasuredZeroSequenceToPositiveSequenceRatio
        {
            get
            {
                return m_zeroSeq.Measurement.Magnitude / m_posSeq.Measurement.Magnitude;
            }
        }

        /// <summary>
        /// The ratio of zero sequence to positive sequence calculated with phase estimates.
        /// </summary>
        [XmlIgnore()]
        public double EstimatedZeroSequenceToPositiveSequenceRatio
        {
            get
            {
                return m_zeroSeq.Estimate.Magnitude / m_posSeq.Estimate.Magnitude;
            }
        }

        /// <summary>
        /// A flag which represents whether or not to include the phasor group in the
        /// positive sequence estimator.
        /// </summary>
        [XmlIgnore()]
        public bool IncludeInPositiveSequenceEstimator
        {
            get
            {
                if (m_enabled)
                {
                    if (m_useStatusFlagForRemovingMeasurements)
                    {
                        if (m_posSeq.Measurement.IncludeInEstimator == false )
                        { 
                            return false; 
                        }
                        else 
                        { 
                            return true; 
                        }
                    }
                    else
                    {
                        if (m_posSeq.Measurement.IncludeInEstimator == false) 
                        { 
                            return false; 
                        }
                        else 
                        { 
                            return true; 
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// A flag which represents whether or not to include the phasor group in the estimator
        /// </summary>
        [XmlIgnore()]
        public bool IncludeInEstimator
        {
            get
            {
                if (m_enabled)
                {
                    if (m_useStatusFlagForRemovingMeasurements)
                    {
                        if (m_phaseA.Measurement.IncludeInEstimator == false ||
                            m_phaseB.Measurement.IncludeInEstimator == false ||
                            m_phaseC.Measurement.IncludeInEstimator == false)
                        { 
                            return false; 
                        }
                        else 
                        { 
                            return true; 
                        }
                    }
                    else
                    {
                        if (m_phaseA.Measurement.IncludeInEstimator == false ||
                            m_phaseB.Measurement.IncludeInEstimator == false ||
                            m_phaseC.Measurement.IncludeInEstimator == false)
                        { 
                            return false; 
                        }
                        else 
                        { 
                            return true; 
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The output measurement key for the ratio of negative sequence magnitude to positive sequence magnitude.
        /// </summary>
        [XmlAttribute("NegativeSequenceKey")]
        public string NegativeSequenceToPositiveSequenceRatioMeasurementKey
        {
            get
            {
                return m_negativeSequenceToPositiveSequenceRatioKey;
            }
            set
            {
                m_negativeSequenceToPositiveSequenceRatioKey = value;
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Initializes default parameters that cannot be set as constants due to their type.
        /// </summary>
        private void InitializeDefaultParameters()
        {
            //ALPHA = new DoubleComplex(Math.Cos(2 * Math.PI / 3), Math.Sin(2 * Math.PI / 3));
            //A_INVERSE = ComplexMatrix.Create(new DoubleComplex[,] {{ 1, 1, 1 }, 
            //                                                       { 1, ALPHA, ALPHA * ALPHA }, 
            //                                                       { 1, ALPHA * ALPHA, ALPHA}});
        }

        #endregion
       
    }
}
