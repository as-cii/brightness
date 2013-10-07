using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff.Strategies
{
    interface IDiffStrategy
    {
        IEnumerable<RowDiff<TIdentity, TModel>> CreateDiff<TModel, TIdentity>(Stream oldBuffer,
            Stream newBuffer, Func<TModel, TIdentity> identity);
    }
}
