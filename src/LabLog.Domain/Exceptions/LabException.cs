using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabLog.Domain.Exceptions
{
    public class LabException : Exception
    {
        public String LabMessage { get; set; }
        private List<Exception> _exceptions = new List<Exception>();
        private int _exceptionCounter = 0;
        public LabException()
        {
        }

        public int Count()
        {
            return _exceptions.Count();
        }

        public Action NextException()
        {
            if (_exceptionCounter < _exceptions.Count())
            {
                _exceptionCounter++;
                throw _exceptions[_exceptionCounter-1];
            }
            return null;
        }

        public void AddException(Exception ex)
        {
            _exceptions.Add(ex);
        }

    }
}
