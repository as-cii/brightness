using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Brightness.Diff;
using Brightness.Diff.Strategies;
using Brightness.Test.Fake;

namespace Brightness.Test.Diff.Strategies
{
    [TestClass]
    public class LineByLineDiffStrategyTest : BaseStrategyTest<LineByLineDiffStrategy>
    {
    }
}
