using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Brightness.Diff;
using CsvHelper;
using Brightness.IO;
using DiffPlex.DiffBuilder.Model;
using DiffPlex.DiffBuilder;
using DiffPlex.Model;
using DiffPlex;

namespace Brightness
{
    public static class DeltaEngine
    {
        public static IEnumerable<RowDiff<TIdentity, TModel>> Diff<TModel, TIdentity>(string fileName1, 
            string fileName2, Func<TModel, TIdentity> identity)
        {
            using (var fs1 = File.Open(fileName1, FileMode.Open))
            using (var fs2 = File.Open(fileName2, FileMode.Open))
            {
                return Diff(fs1, fs2, identity);
            }
        }

        public static IEnumerable<RowDiff<TIdentity, TModel>> Diff<TModel, TIdentity>(Stream oldFile, 
            Stream newFile, Func<TModel, TIdentity> identity)
        {
            using (var twoStream = new TwoStreamReader(oldFile, newFile, 50000))
            {
                // skip old header, take just the new one
                twoStream.StreamA.ReadLine();
                string newHeader = twoStream.StreamB.ReadLine();

                var diffHash = new DiffHash<TIdentity, TModel>();

                string oldBuffer, newBuffer;
                twoStream.ReadLines(out oldBuffer, out newBuffer);
                var diffs = Diffy.ChunkDifferences(oldBuffer, newBuffer);

                ProcessMyersDiff(diffs, identity, diffHash, newHeader);


                return diffHash;
            }
        }


        private static void ProcessMyersDiff<TModel, TIdentity>(IEnumerable<ChunkDiff> diffs, 
            Func<TModel, TIdentity> identity, DiffHash<TIdentity, TModel> hash, string header)
        {
            foreach (var item in diffs)
            {
                using (var reader = new StringReader(string.Format("{0}\n{1}", header, item.Text)))
                using (var csv = new CsvReader(reader))
                {
                    while (csv.Read())
                    {
                        var model = csv.GetRecord<TModel>();
                        var id = identity(model);

                        hash.Add(id, model, item.Status);
                    }
                }
            }
        }

    }
}
