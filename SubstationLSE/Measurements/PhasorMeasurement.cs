
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// Represents a phasor value as a measurement.
    /// </summary>
    [Serializable()]
    public class PhasorMeasurement : PhasorBase
    {
        #region [ Private Constants ]

        private const string DEFAULT_MEASUREMENT_KEY = "Undefined";

        #endregion

        #region [ Private Members ]

        private double m_measurementVariance;
        private bool m_measurementShouldBeCalibrated;

        private double m_rcf;
        private double m_pacf;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// A flag which represents whether or not to include the <see cref="SynchrophasorAnalytics.Measurements.PhasorMeasurement"/> in the state estimator.
        /// </summary>
        [XmlIgnore()]
        public bool IncludeInEstimator
        {
            get 
            {
                if (MeasurementWasReported == false)
                {
                    return false;
                }
                else if (Type == PhasorType.VoltagePhasor && (Magnitude < (0.2 * (VoltageLevel * 1000)) || (double.IsNaN(Magnitude)) || (double.IsNaN(AngleInDegrees))))
                {
                    return false;
                }
                else if (Type == PhasorType.CurrentPhasor && (Magnitude < 10 || (double.IsNaN(Magnitude)) || (double.IsNaN(AngleInDegrees))))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// The measurement variance for the <see cref="SynchrophasorAnalytics.Measurements.PhasorMeasurement"/>.
        /// </summary>
        [XmlAttribute("Variance")]
        public double MeasurementVariance
        {
            get 
            { 
                return m_measurementVariance; 
            }
            set 
            { 
                m_measurementVariance = value; 
            }
        }

        /// <summary>
        /// The measurement covariance
        /// </summary>
        [XmlIgnore]
        public double MeasurementCovariance
        {
            get 
            { 
                return m_measurementVariance * m_measurementVariance; 
            }
        }

        /// <summary>
        /// The ratio correction factor (RCF) for the <see cref="SynchrophasorAnalytics.Measurements.PhasorMeasurement"/>.
        /// </summary>
        [XmlAttribute("RCF")]
        public double RCF
        {
            get 
            { 
                return m_rcf; 
            }
            set 
            { 
                m_rcf = value; 
            }
        }

        /// <summary>
        /// The phase angle correction factor (PACF) for the <see cref="SynchrophasorAnalytics.Measurements.PhasorMeasurement"/>.
        /// </summary>
        [XmlAttribute("PACF")]
        public double PACF
        {
            get 
            { 
                return m_pacf; 
            }
            set 
            { 
                m_pacf = value; 
            }
        }

        /// <summary>
        /// A flag representing whether or not to calibrate the measurement using the RCF and PACF during insertion.
        /// </summary>
        [XmlAttribute("Calibrated")]
        public bool MeasurementShouldBeCalibrated
        {
            get 
            { 
                return m_measurementShouldBeCalibrated; 
            }
            set 
            { 
                m_measurementShouldBeCalibrated = value; 
            }
        }


        #endregion


    }
}
