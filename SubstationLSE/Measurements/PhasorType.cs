
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    #region [ Enumeration ] 

    /// <summary>
    /// Defines the data type for the <see cref="PhasorBase"/> which is intrinsically a complex number. To define translation from per unit to base values, the phasor must be defined as voltage, current, or complex power.
    /// </summary>
    /// <seealso cref="PhasorBase"/>
    public enum PhasorType
    {
        /// <summary>
        /// The enumeration for a voltage phasor.
        /// </summary>
        [XmlEnum("V")]
        VoltagePhasor,

        /// <summary>
        /// The enumeration for a current phasor.
        /// </summary>
        [XmlEnum("I")]
        CurrentPhasor,

        /// <summary>
        /// The enumeration for the phasor representation of complex power
        /// </summary>
        [XmlEnum("S")]
        ComplexPower,
    }

    #endregion
}
