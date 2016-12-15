//******************************************************************************************************
//  Switch.cs
//  Created by Lin Zhang, July 1st 2015
//  Modification: 
//      
//******************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SubstationLSE
{
    public class Switch
    {
        #region [Private Members]
        private string m_internalID;
        private Node fromNode;
        private Node toNode; 
        private string fromNodeID;
        private string toNodeID;

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
        #endregion
    }
}
