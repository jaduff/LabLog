using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabLog.Domain.Exceptions
{
    public class LabException : Exception
    {
        public String LabMessage { get; }
        public LabException(string _message)
        {
            LabMessage = _message;
        }


    }
}
