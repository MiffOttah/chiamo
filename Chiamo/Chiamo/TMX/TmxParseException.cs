using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Chiamo.TMX
{
    public class TmxParseException : Exception
    {
        public TmxParseException(string message) : base(message)
        {
        }
    }
}
