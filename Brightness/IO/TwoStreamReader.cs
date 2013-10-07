using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Brightness.IO
{
    public class TwoStreamReader : IDisposable
    {
        private readonly StreamReader streamA;
        private readonly StreamReader streamB;

        public StreamReader StreamA
        {
            get
            {
                return this.streamA;
            }
        }

        public StreamReader StreamB
        {
            get
            {
                return this.streamB;
            }
        }


        public TwoStreamReader(Stream a, Stream b)
        {
            this.streamA = new StreamReader(a);
            this.streamB = new StreamReader(b);
        }

        public void ReadLines(out string outputA, out string outputB)
        {
            outputA = streamA.ReadToEnd();
            outputB = streamB.ReadToEnd();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.streamA.Dispose();
            this.streamB.Dispose();
        }
    }
}
