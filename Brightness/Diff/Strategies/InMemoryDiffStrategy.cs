using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff.Strategies
{
    public class InMemoryDiffStrategy : IDiffStrategy
    {
        public IEnumerable<RowDiff<TIdentity, TModel>> CreateDiff<TModel, TIdentity>(System.IO.Stream oldBuffer, 
            System.IO.Stream newBuffer, Func<TModel, TIdentity> identity)
        {   
            var csvConfig = new CsvConfiguration()
            {
                Delimiter = "|",
                BufferSize = (1 << 14)
            };

            var hash = new DiffHash<TIdentity, TModel>();

            using (var oldBufferReader = new StreamReader(oldBuffer))
            using (var newBufferReader = new StreamReader(newBuffer))
            using (var oldCsv = new CsvReader(oldBufferReader, csvConfig))
            using (var newCsv = new CsvReader(newBufferReader, csvConfig))
            {
                bool oldHasData = true;
                bool newHasData = true;

                do
                {
                    if (oldHasData && oldCsv.Read())
                    {
                        oldHasData = true;

                        var row = oldCsv.GetRecord<TModel>();
                        var id = identity(row);

                        hash.CompareAdd(id, row, string.Join("", oldCsv.CurrentRecord), RowStatus.Deleted);
                    }
                    else
                    {
                        oldHasData = false;
                    }

                    if (newHasData && newCsv.Read())
                    {
                        var row = newCsv.GetRecord<TModel>();
                        var id = identity(row);

                        hash.CompareAdd(id, row, string.Join("", newCsv.CurrentRecord), RowStatus.Added);
                    }
                    else
                    {
                        newHasData = false;
                    }
                } while (oldHasData || newHasData);
            }

            return hash;
        }
    }
}
