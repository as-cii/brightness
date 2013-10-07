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
using Brightness.Diff.Strategies;

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
            return new LineByLineDiffStrategy().CreateDiff(oldFile, newFile, identity);
        }
    }
}
