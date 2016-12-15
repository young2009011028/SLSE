
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SubstationLSE.Measurements
{
    /// <summary>
    /// The interface which describes how a phasor should behave.
    /// </summary>
    public interface IPhasor
    {
        /// <summary>
        /// The <see cref="SynchrophasorAnalytics.Modeling.VoltageLevel"/> of the <see cref="SynchrophasorAnalytics.Measurements.IPhasor"/>
        /// </summary>
        double VoltageLevel
        {
            get;
            set;
        }

        /// <summary>
        /// The magnitude of the phasor value.
        /// </summary>
        double Magnitude
        {
            get;
            set;
        }

        /// <summary>
        /// The per-unitized magnitude of the phasor value.
        /// </summary>
        double PerUnitMagnitude
        {
            get;
            set;
        }

        /// <summary>
        /// The angle of the phasor value in degrees.
        /// </summary>
        double AngleInDegrees
        {
            get;
            set;
        }

        /// <summary>
        /// The angle of the phasor value in radians.
        /// </summary>
        double AngleInRadians
        {
            get;
            set;
        }

        /// <summary>
        /// The phasor value as a complex number
        /// </summary>
        //DoubleComplex ComplexPhasor
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The phasor value as a per unit complex number
        /// </summary>
        //DoubleComplex PerUnitComplexPhasor
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The measurement key that the magnitude should respond to.
        /// </summary>
        string MagnitudeKey
        {
            get;
            set;
        }

        /// <summary>
        /// The measurement key that the angle should respond to.
        /// </summary>
        string AngleKey
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the <see cref="SynchrophasorAnalytics.Measurements.PhasorType"/> of the measurement.
        /// </summary>
        PhasorType Type
        {
            get;
            set;
        }

    }
}
