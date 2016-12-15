//******************************************************************************************************
//  Node.cs
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      October 1st 2015
//      October 14th 2015
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
    public class ShuntCompensator
    {
        #region [Private Members]
        private string m_internalID;
        private Node m_connectedNode;
        private string m_connectedNodeID;
        private CurrentPhasorGroup m_shuntCurrent;
        private string m_shuntCurrentID; 
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
        [XmlAttribute("ConnectedNode")]
        public string ConnectedNodeID
        {
            get
            {
                return m_connectedNodeID;
            }
            set
            {
                m_connectedNodeID = value;
            }
        }

        /// <summary>
        ///
        /// </summary>

        public Node ConnectedNode
        {
            get
            {
                return m_connectedNode;
            }
            set
            {
                m_connectedNode = value;
                m_connectedNodeID = value.InternalID;
                //if (m_current != null)
                //{
                //    m_current.MeasuredConnectedNode = m_connectedNode;
                //}
            }
        }

        [XmlElement("Current")]
        public CurrentPhasorGroup ShuntCurrent
        {
            get
            {
                return m_shuntCurrent;
            }
            set
            {
                m_shuntCurrent = value;
                m_shuntCurrentID = value.InternalID;
                m_shuntCurrent.MeasuredNode = this.m_connectedNode;
            }
        }
        #endregion
    }
}
