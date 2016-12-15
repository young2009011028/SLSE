using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    public class Phasor
    {
        #region [Private Members]
        private string m_internalID;
        private PhasorMeasurement m_phasorMeasurement;
        private PhasorEstimate m_phasorEstimate;
        private string m_magnitudeResidualOutputKey;
        private string m_angleResidualOutputKey;

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
        /// A <see cref="SynchrophasorAnalytics.Measurements.PhasorMeasurement"/> - a measured phasor value.
        /// </summary>
        [XmlElement("Measurement")]
        public PhasorMeasurement Measurement
        {
            get
            {
                return m_phasorMeasurement;
            }
            set
            {
                m_phasorMeasurement = value;
            }
        }

        /// <summary>
        /// A <see cref="SynchrophasorAnalytics.Measurements.PhasorEstimate"/>. - an estimated phasor value.
        /// </summary>
        [XmlElement("Estimate")]
        public PhasorEstimate Estimate
        {
            get
            {
                return m_phasorEstimate;
            }
            set
            {
                m_phasorEstimate = value;
            }
        }

        /// <summary>
        /// The output measurement key for the magnitude of the residual value.
        /// </summary>
        [XmlAttribute("MagnitudeResidualKey")]
        public string MagnitudeResidualKey
        {
            get
            {
                return m_magnitudeResidualOutputKey;
            }
            set
            {
                m_magnitudeResidualOutputKey = value;
            }
        }

        /// <summary>
        /// The output measurement key for the magnitude of the residual value.
        /// </summary>
        [XmlAttribute("AngleResidualKey")]
        public string AngleResidualKey
        {
            get
            {
                return m_angleResidualOutputKey;
            }
            set
            {
                m_angleResidualOutputKey = value;
            }
        }


        #endregion


        #region [ Constructors ]

        /// <summary>
        /// A blank constructor with default values.
        /// </summary>
        public Phasor()
            : this(new PhasorMeasurement(), new PhasorEstimate())
        {
        }

        /// <summary>
        /// A constructor for the <see cref="Phasor"/> class which only specifies the <see cref="PhasorMeasurement"/>.
        /// </summary>
        /// <param name="measurement"></param>
        public Phasor(PhasorMeasurement measurement)
            : this(measurement, new PhasorEstimate())
        {
        }

        /// <summary>
        /// The designated constructor for the class.
        /// </summary>
        /// <param name="measurement">A measured phasor - <see cref="PhasorMeasurement"/>.</param>
        /// <param name="estimate">An estimated phasor - <see cref="PhasorEstimate"/>.</param>
        public Phasor(PhasorMeasurement measurement, PhasorEstimate estimate)
        {
            m_phasorMeasurement = measurement;
            m_phasorEstimate = estimate;
            m_magnitudeResidualOutputKey = "Undefined";
            m_angleResidualOutputKey = "Undefined";
        }

        #endregion

    }
}
