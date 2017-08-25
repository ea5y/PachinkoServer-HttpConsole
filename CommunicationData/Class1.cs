using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XY.Pachinko.CommunicationData
{
    public class BaseRes
    {
        public string Result;
    }

    public class DIORes : BaseRes
    {
    }

    public class PachinkoRes : BaseRes
    {
        public string data;
    }

    public class InputRes : BaseRes
    {
        public byte[] InputData;
    }
}
