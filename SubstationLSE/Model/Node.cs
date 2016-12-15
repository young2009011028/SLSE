//******************************************************************************************************
//  Node.cs
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      October 1st 2015
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SubstationLSE.Measurements;

namespace SubstationLSE
{
    public class Node
    {
        #region [Private Members]
        private string m_internalID;

        private VoltagePhasorGroup m_voltage;
        private string m_voltagePhasorGroupID;

        private double m_voltageLevel; 

        //private VoltageLevel m_voltageLevel;
        //private string m_voltageLevelID;

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

        [XmlElement("Voltage")]
        public VoltagePhasorGroup Voltage
        {
            get
            {
                return m_voltage;
            }
            set
            {
                m_voltage = value;
                m_voltagePhasorGroupID = value.InternalID;
                m_voltage.MeasuredNode = this;
                m_voltage.MeasuredNodeID = this.InternalID; 
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

        //[XmlIgnore()]
        //public VoltageLevel BaseKV
        //{
        //    get
        //    {
        //        return m_voltageLevel;
        //    }
        //    set
        //    {
        //        m_voltageLevel = value;
        //        m_voltageLevelID = value.InternalID;
        //    }
        //}

        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel.InternalID"/> of the baseKV <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/>
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

        #endregion
    }
}
