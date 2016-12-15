
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// Represents the estimate of a phasor value
    /// </summary>
    [Serializable()]
    public class PhasorEstimate : PhasorBase
    {
        #region [ Private Constants ]

        private const string DEFAULT_MEASUREMENT_KEY = "Undefined";

        #endregion

    }
}
