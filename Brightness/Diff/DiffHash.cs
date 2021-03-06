﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff
{
    public class DiffHash<TIdentity, TModel> : IEnumerable<RowDiff<TIdentity, TModel>>
    {
        private readonly Dictionary<TIdentity, RowDiff<TIdentity, TModel>> diffHash;

        public DiffHash()
        {
            this.diffHash = new Dictionary<TIdentity, RowDiff<TIdentity, TModel>>();
        }

        public void CompareAdd(TIdentity id, TModel model, string plain, RowStatus status)
        {
            if (!diffHash.ContainsKey(id))
            {
                this.diffHash.Add(id, new RowDiff<TIdentity, TModel>(id, model, status, plain));
            }
            else
            {
                var row = this.diffHash[id];

				if (plain == row.PlainRow)
				{
					this.diffHash.Remove(id);
				}
				else 
				{
					row.Status = RowStatus.Updated;
                    if (status == RowStatus.Added)
                    {
                        row.Row = model;
                        row.PlainRow = plain;
                    }
				}
            }
        }

        public IEnumerator<RowDiff<TIdentity, TModel>> GetEnumerator()
        {
            return diffHash.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
