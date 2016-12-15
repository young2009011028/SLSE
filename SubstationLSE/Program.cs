using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SubstationLSE
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = "test.xml";
            Substation substation = new Substation();
            substation = substation.DeserializeFromXml(filepath);

            substation.Initialize();
           // substation.MeasurementReceived();
            substation.SLSE();

        }


    }
}
