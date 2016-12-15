//******************************************************************************************************
//  CircuitBreaker.cs
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
    public class CircuitBreaker
    {
        #region [Private Members]

        private string m_internalID;
        private Node fromNode;
        private Node toNode;
        private string fromNodeID;
        private string toNodeID;
        private string m_normalState;
        private string m_actualState;
        private BreakerCurrentPhasorGroup m_breakerCurrent; 

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
        /// The normal or default state of the switch. Either <see cref="SynchrophasorAnalytics.Modeling.SwitchingDeviceNormalState.Open"/> or <see cref="SynchrophasorAnalytics.Modeling.SwitchingDeviceNormalState.Closed"/>
        /// </summary>
        [XmlAttribute("Normally")]
        public string NormalState
        {
            get
            {
                return m_normalState;
            }
            set
            {
                m_normalState = value;
            }
        }

        /// <summary>
        /// The actual current state of the switch. Either <see cref="SynchrophasorAnalytics.Modeling.SwitchingDeviceActualState.Open"/> or <see cref="SynchrophasorAnalytics.Modeling.SwitchingDeviceActualState.Closed"/>.
        /// </summary>
        [XmlAttribute("Actually")]
        public string ActualState
        {
            get
            {
                return m_actualState;
            }
            set
            {
                m_actualState = value;
            }
        }

        [XmlElement("BreakerCurrent")]
        public BreakerCurrentPhasorGroup BreakerCurrent
        {
            get
            {
                return m_breakerCurrent;
            }
            set
            {
                m_breakerCurrent = value;
                m_breakerCurrent.FromNode = this.FromNode;
                m_breakerCurrent.ToNode = this.ToNode;
            }
        }

        #endregion
    }
}
