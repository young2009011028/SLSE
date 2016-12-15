using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstationLSE.Measurements
{
    public class BreakerCurrentPhasorGroup:PhasorGroup
    {
        #region [Private Members]

        string m_fromNodeID;
        Node m_fromNode;        

        string m_toNodeID;
        Node m_toNode;

        string m_measuredBreakerID;
        CircuitBreaker m_measuredBreaker;

        #endregion

        #region [Properties]

        public string FromNodeID
        {
            get
            {
                return m_fromNodeID;
            }
            set
            {
                m_fromNodeID = value;
            }
        }

        public Node FromNode
        {
            get
            {
                return m_fromNode;
            }
            set
            {
                m_fromNode = value;
            }
        }

        public string ToNodeID
        {
            get
            {
                return m_toNodeID;
            }
            set
            {
                m_toNodeID = value;
            }
        }

        public Node ToNode
        {
            get
            {
                return m_toNode;
            }
            set
            {
                m_toNode = value;
            }
        }

        public string MeasuredBreakerID
        {
            get
            {
                return m_measuredBreakerID;
            }
            set
            {
                m_measuredBreakerID = value;
            }
        }

        public CircuitBreaker MeasuredBreaker
        {
            get
            {
                return m_measuredBreaker;
            }
            set
            {
                m_measuredBreaker = value;
            }
        }

        #endregion
    }
}
