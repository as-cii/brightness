using DiffPlex;
using DiffPlex.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff
{
    internal static class Diffy
    {
        private static void AppendLineIfNonEmpty(this StringBuilder builder, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            builder.AppendLine(text);
        }

        /// <summary>
        /// Compares two buffers and outputs differences of chunks.
        /// </summary>
        /// <param name="oldBuffer"></param>
        /// <param name="newBuffer"></param>
        /// <returns></returns>
        internal static IEnumerable<ChunkDiff> ChunkDifferences(string oldBuffer, string newBuffer)
        {
            DiffResult diffResult = new Differ().CreateLineDiffs(oldBuffer, newBuffer, false);
            var diffs = new List<ChunkDiff>();
            int bPos = 0;
            foreach (var diffBlock in diffResult.DiffBlocks)
            {
                bPos += diffBlock.InsertStartB;

                int i = 0;
                var builder = new StringBuilder();

                for (; i < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); i++)
                {
                    builder.AppendLineIfNonEmpty(diffResult.PiecesOld[i + diffBlock.DeleteStartA]);
                }
                diffs.Add(new ChunkDiff(RowStatus.Deleted, builder.ToString()));

                i = 0;
                builder.Clear();
                for (; i < Math.Min(diffBlock.DeleteCountA, diffBlock.InsertCountB); i++)
                {
                    builder.AppendLineIfNonEmpty(diffResult.PiecesNew[i + diffBlock.InsertStartB]);
                    bPos++;
                }
                diffs.Add(new ChunkDiff(RowStatus.Added, builder.ToString()));
                builder.Clear();
                if (diffBlock.DeleteCountA > diffBlock.InsertCountB)
                {
                    for (; i < diffBlock.DeleteCountA; i++)
                    {
                        builder.AppendLineIfNonEmpty(diffResult.PiecesOld[i + diffBlock.DeleteStartA]);
                    }
                    diffs.Add(new ChunkDiff(RowStatus.Deleted, builder.ToString()));
                }
                else
                {
                    for (; i < diffBlock.InsertCountB; i++)
                    {
                        builder.AppendLineIfNonEmpty(diffResult.PiecesNew[i + diffBlock.InsertStartB]);
                        bPos++;
                    }
                    diffs.Add(new ChunkDiff(RowStatus.Added, builder.ToString()));
                }
            }

            return diffs;
        }

    }
}
