using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.IO
{
    public static class StreamReaderEx
    {
        private const uint LINES_CHUNK_SIZE = 5000;

        internal static void SkipCurrentLine(this StreamReader reader)
        {
            reader.ReadLine();
        }

        // TODO: optimize this step, we could directly read blocks
        internal static void ReadChunks(StreamReader oldFileReader, StreamReader newFileReader, out string oldBuffer, out string newBuffer)
        {
            var oldBufferBuilder = new StringBuilder();
            var newBufferBuilder = new StringBuilder();

            for (int i = 0; i < LINES_CHUNK_SIZE; i++)
            {
                oldBufferBuilder.AppendLine(oldFileReader.ReadLine());
                newBufferBuilder.AppendLine(newFileReader.ReadLine());
            }

            oldBuffer = oldBufferBuilder.ToString();
            newBuffer = newBufferBuilder.ToString();
        }
    }
}
