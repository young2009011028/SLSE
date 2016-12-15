
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// Represents the base kV of a <see cref="SynchrophasorAnalytics.Modeling.Node"/> and of <see cref="SynchrophasorAnalytics.Measurements.PhasorBase"/>.
    /// </summary>
    [Serializable()]
    public class VoltageLevel 
    {
        #region [ Private Constants ]

        private const int DEFAULT_INTERNAL_ID = 0;
        private const string DEFAULT_ACRONYM = "KV";
        #endregion

        #region [ Private Members ]

        private string m_uniqueId;
        private string m_internalID;
        private double m_voltageLevel;

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
        /// An integer identifier for each <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> which is intended to be unique among other objects of the same type.
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
        /// A descriptive number for the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> object. Will always return the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel.InternalID"/>.
        /// </summary>
        [XmlIgnore()]
        public string Number
        {
            get 
            { 
                return m_internalID; 
            }
            set 
            { 
            }
        }

        /// <summary>
        /// A descriptive acronym for the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> object. Will always return 'KV'.
        /// </summary>
        [XmlIgnore()]
        public string Acronym
        {
            get 
            { 
                return DEFAULT_ACRONYM; 
            }
            set 
            { 
            }
        }

        /// <summary>
        /// A name for the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> object. Will always return 'voltage + kV'. For example, '500 kV'.
        /// </summary>
        [XmlIgnore()]
        public string Name
        {
            get 
            { 
                return m_voltageLevel.ToString() + " kV"; 
            }
            set 
            { 
            }
        }

        /// <summary>
        /// A description of the <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> object. Will always return 'voltage + kV Voltage Level Definition'. For example, '500 kV Voltage Level Definition'.
        /// </summary>
        [XmlIgnore()]
        public string Description
        {
            get 
            { 
                return m_voltageLevel.ToString() + " kV Voltage Level Definition"; 
            }
            set 
            { 
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
        /// The voltage level in line-line kilovolts.
        /// </summary>
        [XmlAttribute("KV")]
        public double Value
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

        #endregion

    }
}
