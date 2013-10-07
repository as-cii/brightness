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
                Delimiter = "|"
            };

            var hash = new DiffHash<TIdentity, TModel>();

            using (var oldBufferReader = new StreamReader(oldBuffer))
            using (var newBufferReader = new StreamReader(newBuffer))
            using (var oldCsv = new CsvReader(oldBufferReader, csvConfig))
            using (var newCsv = new CsvReader(newBufferReader, csvConfig))
            {
                while (newCsv.Read())
                {
                    var row = newCsv.GetRecord<TModel>();
                    var id = identity(row);

                    hash.Add(id, row);
                }

                while (oldCsv.Read())
                {
                    var row = oldCsv.GetRecord<TModel>();
                    var id = identity(row);

                    hash.CompareAndAdd(id, row);
                }
            }

            return hash;
        }
    }
}
