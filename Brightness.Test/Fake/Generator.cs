using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Test.Fake
{
    public static class Generator
    {
        public const string FAKE_CSV_1 = @"";
        public const string FAKE_CSV_2 = @"";
        private static Random random = new Random();


        private static string GenerateLine(uint id)
        {
            // use random numbers to generate totally different lines
            string firstName = random.Next().ToString();
            string lastName = random.Next().ToString();

            return string.Format("{0}|{1}|{2}", id, firstName, lastName);
        }

        /// <summary>
        /// Generate two similar buffers of ASCII encoded text.
        /// </summary>
        /// <param name="maxLines"></param>
        /// <param name="newFile"></param>
        /// <param name="oldFile"></param>
        public static void GenerateSimilarLines(uint maxLines, uint additions, uint changes, uint deletions,
            out byte[] newFile, out byte[] oldFile)
        {
            var oldFileBuilder = new StringBuilder();
            var newFileBuilder = new StringBuilder();

            string header = "Id|FirstName|LastName";
            oldFileBuilder.AppendLine(header);
            newFileBuilder.AppendLine(header);

            uint lineIndex = 0;

            for (uint changeCount = 0; changeCount < changes; changeCount++, lineIndex++)
            {
                string line1 = GenerateLine(lineIndex);
                string line2 = GenerateLine(lineIndex);

                oldFileBuilder.AppendLine(line1);
                newFileBuilder.AppendLine(line2);
            }

            for (uint deleteCount = 0; deleteCount < deletions; deleteCount++, lineIndex++)
            {
                string line = GenerateLine(lineIndex);

                oldFileBuilder.AppendLine(line);
            }

            for (; lineIndex < maxLines - additions; lineIndex++)
            {
                string line = GenerateLine(lineIndex);

                oldFileBuilder.AppendLine(line);
                newFileBuilder.AppendLine(line);
            }

            for (uint addCount = 0; addCount < additions; addCount++, lineIndex++)
            {
                string line = GenerateLine(lineIndex);

                newFileBuilder.AppendLine(line);
            }

            newFile = Encoding.ASCII.GetBytes(newFileBuilder.ToString());
            oldFile = Encoding.ASCII.GetBytes(oldFileBuilder.ToString());
        }
    }
}
