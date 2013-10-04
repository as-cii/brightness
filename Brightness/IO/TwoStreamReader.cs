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
        private readonly int chunkSize;

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


        public TwoStreamReader(Stream a, Stream b, int chunkSize)
        {
            this.streamA = new StreamReader(a);
            this.streamB = new StreamReader(b);
            this.chunkSize = chunkSize;
        }

        public bool ReadLines(out string outputA, out string outputB)
        {
            var outputABuilder = new StringBuilder();
            var outputBBuilder = new StringBuilder();
            var hasReadSomething = false;

            for (int i = 0; i < chunkSize; i++)
            {
                if (!(streamA.EndOfStream && streamB.EndOfStream))
                {
                    hasReadSomething = true;
                }

                if (!streamA.EndOfStream)
                    outputABuilder.AppendLine(streamA.ReadToEnd());

                if (!streamB.EndOfStream)
                    outputBBuilder.AppendLine(streamB.ReadToEnd());
            }

            outputA = outputABuilder.ToString();
            outputB = outputBBuilder.ToString();

            return hasReadSomething;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.streamA.Dispose();
            this.streamB.Dispose();
        }
    }
}
