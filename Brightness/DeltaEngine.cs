using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.IO;
using Brightness.IO;
using System.Linq;
using System.Text;
using Brightness.Diff;
using CsvHelper;

namespace Brightness
{
    public static class DeltaEngine
    {
        private static readonly Dictionary<Operation, RowStatus> operationsMapping = new Dictionary<Operation,RowStatus>()
        {
            { Operation.DELETE, RowStatus.Deleted },
            { Operation.INSERT, RowStatus.Added }
        };

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
            var diff = new diff_match_patch()
            {
                Diff_EditCost = 10,
                Diff_Timeout = 2
            };

            using (var oldFileReader = new StreamReader(oldFile))
            using (var newFileReader = new StreamReader(newFile))
            {
                // skip old header, take just the new one
                oldFileReader.SkipCurrentLine();
                string newHeader = newFileReader.ReadLine();

                string oldBuffer;
                string newBuffer;
                StreamReaderEx.ReadChunks(oldFileReader, newFileReader, out oldBuffer, out newBuffer);

                var diffs = from diffChunk in diff.diff_lineMode(oldBuffer, newBuffer)
                            where diffChunk.operation != Operation.EQUAL &&
                                  !string.IsNullOrWhiteSpace(diffChunk.text)
                            select diffChunk;

                return ProcessMyersDiff(diffs, identity, newHeader);
            }
        }

        private static IEnumerable<RowDiff<TIdentity, TModel>> ProcessMyersDiff<TModel, TIdentity>(IEnumerable<DiffMatchPatch.Diff> diffs, 
            Func<TModel, TIdentity> identity, string header)
        {
            var hash = new DiffHash<TIdentity, TModel>();
            foreach (var item in diffs)
            {
                using (var reader = new StringReader(string.Format("{0}\n{1}", header, item.text)))
                using (var csv = new CsvReader(reader))
                {
                    while (csv.Read())
                    {
                        var model = csv.GetRecord<TModel>();
                        var status = operationsMapping[item.operation];
                        var id = identity(model);

                        hash.Add(id, model, status);
                    }
                }
            }

            return hash;
        }

    }
}
