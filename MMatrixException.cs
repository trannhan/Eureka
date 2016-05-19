using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WindowsFormsApplication1
{
    public class MMatrixException: Exception
    {
        public MMatrixException()
            : base()
        { }

        public MMatrixException(string Message)
            : base(Message)
        { }

        public MMatrixException(string Message, Exception InnerException)
            : base(Message, InnerException)
        { }
    }
}
