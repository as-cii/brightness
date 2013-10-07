using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff.Strategies
{
    interface IDiffStrategy
    {
        IEnumerable<RowDiff<TIdentity, TModel>> CreateDiff<TModel, TIdentity>(string header, string oldBuffer,
            string newBuffer, Func<TModel, TIdentity> identity);
    }
}
