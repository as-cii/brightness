using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightness.Diff
{
    public class ChunkDiff
    {
        public RowStatus Status { get; set; }

        public string Text { get; set; }

        public ChunkDiff(RowStatus status, string text)
        {
            this.Status = status;
            this.Text = text;
        }
    }
}
