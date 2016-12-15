
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// The base class for phasor values
    /// </summary>
    [Serializable()]
    public partial class PhasorBase : IPhasor
    {
        #region [ Private Members ]

        private double m_magnitude;
        private string m_magnitudeKey;
        private bool m_magnitudeValueWasReported;

        private double m_angleInDegrees;
        private string m_angleKey;
        private bool m_angleValueWasReported;

        private PhasorType m_type;

        private double m_voltageLevel; 
        //private VoltageLevel m_baseKV;
        //private string m_voltageLevelID;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> of the phasor measurement.
        /// </summary>
        //[XmlIgnore()]
        //public VoltageLevel BaseKV
        //{
        //    get 
        //    { 
        //        return m_baseKV; 
        //    }
        //    set 
        //    { 
        //        m_baseKV = value;
        //        m_voltageLevelID = value.InternalID;
        //    }
        //}

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel.InternalID"/> of the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/>.
        /// </summary>
        //[XmlAttribute("BaseKV")]
        //public string VoltageLevelID
        //{
        //    get 
        //    { 
        //        return m_voltageLevelID; 
        //    }
        //    set 
        //    { 
        //        m_voltageLevelID = value; 
        //    }
        //}

        /// <summary>
        /// The magnitude of the phasor measurement in line-to-neutral volts.
        /// </summary>
        [XmlAttribute("Magnitude")]
        public double Magnitude
        {
            get 
            { 
                return m_magnitude;
            }
            set 
            {
                m_magnitudeValueWasReported = true;
                m_magnitude = value;
            }
        }

        /// <summary>
        /// The magnitude of the phasor measurement in line-to-line per unit.
        /// </summary>
        [XmlIgnore()]
        public double PerUnitMagnitude
        {
            get
            {
                double perUnitValue = 0;
                if (m_type == PhasorType.CurrentPhasor)
                {
                    perUnitValue = m_magnitude / ((100 * 1000000) / (VoltageLevel * 1000 * Math.Sqrt(3)));
                }
                else if (m_type == PhasorType.VoltagePhasor)
                {
                    perUnitValue = m_magnitude * Math.Sqrt(3) / (VoltageLevel * 1000);
                }
                return perUnitValue;
            }
            set
            {
                if (m_type == PhasorType.CurrentPhasor)
                {
                    m_magnitudeValueWasReported = true;
                    m_magnitude = value * ((100 * 1000000) / (VoltageLevel * 1000 * Math.Sqrt(3)));
                }
                else if (m_type == PhasorType.VoltagePhasor)
                {
                    m_magnitudeValueWasReported = true;
                    m_magnitude = value * (VoltageLevel * 1000) / Math.Sqrt(3);
                }
            }
        }

        /// <summary>
        ///  The angle of the phasor measurement in degrees.
        /// </summary>
        [XmlAttribute("Angle")]
        public double AngleInDegrees
        {
            get 
            { 
                return m_angleInDegrees; 
            }
            set 
            {
                m_angleValueWasReported = true;
                m_angleInDegrees = value; 
            }
        }

        /// <summary>
        /// The angle of the phasor measurement in radians.
        /// </summary>
        [XmlIgnore()]
        public double AngleInRadians
        {
            get 
            { 
                return (m_angleInDegrees * Math.PI / 180); 
            }
            set 
            {
                m_angleValueWasReported = true;
                m_angleInDegrees = (value * 180 / Math.PI); 
            }
        }

        /// <summary>
        /// The voltage level in line-line kilovolts.
        /// </summary>
        [XmlAttribute("BaseKV")]
        public double VoltageLevel
        {
            get
            {
                return m_voltageLevel;
            }
            set
            {
                m_voltageLevel = value;
            }
        }

        /// <summary>
        /// The complex value of the phasor measurement in line-to-neutral volts.
        /// </summary>
        [XmlIgnore()]
        public Complex ComplexPhasor
        {
            get
            {
                double realpart = m_magnitude * Math.Cos(m_angleInDegrees * Math.PI / 180);
                double imagPart = m_magnitude * Math.Sin(m_angleInDegrees * Math.PI / 180);
                return (new Complex(realpart, imagPart));
            }
            set
            {
                m_magnitudeValueWasReported = true;
                m_magnitude = Math.Sqrt(value.Real * value.Real + value.Imaginary * value.Imaginary);

                m_angleValueWasReported = true;
                m_angleInDegrees = Math.Atan2(value.Imaginary, value.Real) * 180 / Math.PI;
            }
        }

        /// <summary>
        /// The complex value of the phasor measurement in line-to-line per unit.
        /// </summary>
        [XmlIgnore()]
        public Complex PerUnitComplexPhasor
        {
            get
            {
                Complex perUnitComplexPhasor = new Complex(0, 0);
                if (m_type == PhasorType.CurrentPhasor)
                {
                    perUnitComplexPhasor = ComplexPhasor / ((100 * 1000000) / (VoltageLevel * 1000 * Math.Sqrt(3)));
                }
                else if (m_type == PhasorType.VoltagePhasor)
                {
                    perUnitComplexPhasor = ComplexPhasor * Math.Sqrt(3) / (VoltageLevel * 1000);
                }
                return perUnitComplexPhasor;
            }
            set
            {
                if (m_type == PhasorType.CurrentPhasor)
                {
                    ComplexPhasor = value * ((100 * 1000000) / (VoltageLevel * 1000 * Math.Sqrt(3)));
                }
                else if (m_type == PhasorType.VoltagePhasor)
                {
                    ComplexPhasor = value * (VoltageLevel * 1000) / Math.Sqrt(3);
                }
            }
        }

        /// <summary>
        /// The input measurement key for the <see cref="SynchrophasorAnalytics.Measurements.PhasorBase.Magnitude"/> of the <see cref="SynchrophasorAnalytics.Measurements.PhasorBase"/>.
        /// </summary>
        [XmlAttribute("MagnitudeKey")]
        public string MagnitudeKey
        {
            get 
            { 
                return m_magnitudeKey; 
            }
            set 
            { 
                m_magnitudeKey = value; 
            }
        }

        /// <summary>
        /// The input measurement key for the <see cref="SynchrophasorAnalytics.Measurements.PhasorBase.AngleInDegrees"/> of the <see cref="SynchrophasorAnalytics.Measurements.PhasorBase"/>.
        /// </summary>
        [XmlAttribute("AngleKey")]
        public string AngleKey
        {
            get 
            { 
                return m_angleKey; 
            }
            set 
            { 
                m_angleKey = value; 
            }
        }

        /// <summary>
        /// Specifies whether the phasor measurement is a current phasor or a voltage phasor  or complex power with the <see cref="SynchrophasorAnalytics.Measurements.PhasorType"/> enumeration, either <see cref="SynchrophasorAnalytics.Measurements.PhasorType.VoltagePhasor"/>, <see cref="SynchrophasorAnalytics.Measurements.PhasorType.CurrentPhasor"/>, or <see cref="SynchrophasorAnalytics.Measurements.PhasorType.ComplexPower"/>.
        /// </summary>
        [XmlAttribute("Type")]
        public PhasorType Type
        {
            get
            { 
                return m_type; 
            }
            set
            { 
                m_type = value; 
            }
        }

        /// <summary>
        /// A flag which represents whether or not the measurement was received for the current timestamp.
        /// </summary>
        [XmlIgnore()]
        public bool MeasurementWasReported
        {
            get
            {
                if (m_magnitudeValueWasReported && m_angleValueWasReported)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// A flag which represents whether or not the magnitude measurement was received for the current timestamp.
        /// </summary>
        [XmlIgnore()]
        public bool MagnitudeValueWasReported
        {
            get
            {
                return m_magnitudeValueWasReported;
            }
        }

        /// <summary>
        /// A flag which represents whether or not the angle measurement was received for the current timestamp.
        /// </summary>
        [XmlIgnore()]
        public bool AngleValueWasReported
        {
            get
            {
                return m_angleValueWasReported;
            }
        }

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// A blank constructor with default values.
        /// </summary>
        public PhasorBase()
            : this(PhasorType.VoltagePhasor, 110)
        {
        }

        /// <summary>
        /// A constructor with default values except for the <see cref="PhasorType"/> and the <see cref="VoltageLevel"/> of the phasor.
        /// </summary>
        /// <param name="type">The <see cref="PhasorType"/> of the phasor.</param>
        /// <param name="baseKV">The <see cref="VoltageLevel"/> of the phasor.</param>
        public PhasorBase(PhasorType type, double baseKV)
            : this("Undefined", "Undefined", type, baseKV)
        {
        }

        /// <summary>
        /// Designated constructor method for the <see cref="PhasorBase"/> object.
        /// </summary>
        /// <param name="magnitudeKey">The measurement key which corresponds to the magnitude of the phasor.</param>
        /// <param name="angleKey">The measurement key which corresponds to the  angle of the phasor.</param>
        /// <param name="type">The <see cref="PhasorType"/> of the phasor.</param>
        /// <param name="baseKV">The <see cref="VoltageLevel"/> of the phasor.</param>
        public PhasorBase(string magnitudeKey, string angleKey, PhasorType type, double baseKV)
        {
            m_magnitudeValueWasReported = false;
            m_angleValueWasReported = false;
            m_magnitude = 0;
            m_angleInDegrees = 0;
            m_magnitudeKey = magnitudeKey;
            m_angleKey = angleKey;
            m_type = type;
            m_voltageLevel = baseKV;
            //m_shouldSerializeData = false;
        }

        #endregion

    }
}
