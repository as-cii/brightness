using CsvHelper;
using DiffPlex;
using DiffPlex.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightness.Extensions;
using Brightness.IO;

namespace Brightness.Diff.Strategies
{
    public class LineByLineDiffStrategy : IDiffStrategy
    {
        /// <summary>
        /// Compares two buffers and outputs differences of chunks.
        /// </summary>
        /// <param name="oldBuffer"></param>
        /// <param name="newBuffer"></param>
        /// <returns></returns>
        public IEnumerable<RowDiff<TIdentity, TModel>> CreateDiff<TModel, TIdentity>(Stream oldBuffer,
            Stream newBuffer, Func<TModel, TIdentity> identity)
        {
            string oldText, newText, header;
            ReadBuffers(oldBuffer, newBuffer, out oldText, out newText, out header);

            DiffResult diffResult = new Differ().CreateLineDiffs(oldText, newText, false);
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

            return ProcessAndParseDiff(diffs, identity, header);
        }

        private static void ReadBuffers(Stream oldBuffer, Stream newBuffer, out string oldText, out string newText, out string header)
        {

            using (var twoStream = new TwoStreamReader(oldBuffer, newBuffer))
            {
                twoStream.StreamA.ReadLine();
                header = twoStream.StreamB.ReadLine();
                twoStream.ReadLines(out oldText, out newText);
            }
        }

        private static DiffHash<TIdentity, TModel> ProcessAndParseDiff<TModel, TIdentity>(IEnumerable<ChunkDiff> diffs,
    Func<TModel, TIdentity> identity, string header)
        {
            var hash = new DiffHash<TIdentity, TModel>();
            foreach (var item in diffs)
            {
                using (var reader = new StringReader(string.Format("{0}\n{1}", header, item.Text)))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.Delimiter = "|";

                    while (csv.Read())
                    {
                        var model = csv.GetRecord<TModel>();
                        var id = identity(model);

                        hash.MergeAdd(id, model, item.Status);
                    }
                }
            }

            return hash;
        }
    }
}
