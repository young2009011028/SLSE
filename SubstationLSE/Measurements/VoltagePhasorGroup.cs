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

namespace SubstationLSE.Measurements
{
    public class VoltagePhasorGroup: PhasorGroup
    {
        #region [Private Members]

        string measuredNodeID;
        Node measuredNode;

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
        #endregion
    }
}
