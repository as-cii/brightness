using System;

namespace Brightness
{
    public enum RowStatus
    {
        Added,
        Deleted,
        Updated
    }

    public class RowDiff
    {

        public RowStatus Status { get; set; }
    }    
}