
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SubstationLSE
{
    /// <summary>
    /// Specifies the type of connection for a particular side of a <see cref="SynchrophasorAnalytics.Modeling.Transformer"/>.
    /// </summary>
    [Serializable()]
    public enum TransformerConnectionType
    {
        /// <summary>
        /// Represents a delta connection on one side of a <see cref="SynchrophasorAnalytics.Modeling.Transformer"/>.
        /// </summary>
        [XmlEnum("Delta")]
        Delta,

        /// <summary>
        /// Represents a wye connection on one side of a <see cref="SynchrophasorAnalytics.Modeling.Transformer"/>.
        /// </summary>
        [XmlEnum("Wye")]
        Wye
    }
}
