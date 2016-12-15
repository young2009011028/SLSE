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

namespace SubstationLSE.Measurements
{
    public class CurrentPhasorGroup:PhasorGroup
    {
        #region [Private Members]

        string measuredNodeID;
        Node measuredNode;

        string measuredBreakerOne;
        string measuredBreakerOneDirection;
        string measuredBreakerTwo;
        string measuredBreakerTwoDirection;

        #endregion

        #region [Properties]

        public string MeasuredNodeID
        {
            get
            {
                return measuredNodeID;
            }
            set
            {
                measuredNodeID = value;
            }
        }

        public Node MeasuredNode
        {
            get
            {
                return measuredNode;
            }
            set
            {
                measuredNode = value;
            }
        }

        [XmlAttribute("MeasuredBreakerOne")]
        public string MeasuredBreakerOne
        {
            get
            {
                return measuredBreakerOne;
            }
            set
            {
                measuredBreakerOne = value;
            }
        }

        [XmlAttribute("MeasuredBreakerOneDirection")]
        public string MeasuredBreakerOneDirection
        {
            get
            {
                return measuredBreakerOneDirection;
            }
            set
            {
                measuredBreakerOneDirection = value;
            }
        }

        [XmlAttribute("MeasuredBreakerTwo")]
        public string MeasuredBreakerTwo
        {
            get
            {
                return measuredBreakerTwo;
            }
            set
            {
                measuredBreakerTwo = value;
            }
        }

        [XmlAttribute("MeasuredBreakerTwoDirection")]
        public string MeasuredBreakerTwoDirection
        {
            get
            {
                return measuredBreakerTwoDirection;
            }
            set
            {
                measuredBreakerTwoDirection = value;
            }
        }

        #endregion
    }
}
