using System;
using System.Collections.Generic;

namespace Brightness
{
    public enum RowStatus
    {
        Added = 1,
        Deleted = 2,
        Updated = 3
    }

    public class RowDiff<TIdentity, TModel> : IEquatable<RowDiff<TIdentity, TModel>>
    {
        public TIdentity Id { get; set; }
        public TModel Row { get; set; }
        public RowStatus Status { get; set; }

        public RowDiff(TIdentity id, TModel row, RowStatus status)
        {
            this.Row = row;
            this.Id = id;
            this.Status = status;
        }

        public bool Equals(RowDiff<TIdentity, TModel> other)
        {
            return EqualityComparer<TIdentity>.Default.Equals(this.Id, other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void Merge(RowDiff<TIdentity, TModel> row)
        {
            if (!this.Equals(row))
            {
                throw new Exception("Invalid merge. The rows must have the same keys!");
            }

            if (this.Status == RowStatus.Updated || this.Status == row.Status)
            {
                throw new Exception("Invalid merge. Can't add/remove the same key multiple times!");
            }

            this.Status = (short)this.Status + row.Status;
            if (row.Status == RowStatus.Added)
                this.Row = row.Row;
        }
    }    
}