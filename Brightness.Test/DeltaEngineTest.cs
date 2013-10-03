using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Brightness.Test
{
    [TestClass]
    public class DeltaEngineTest
    {
        [TestInitialize]
        public void Hello()
        {
            // Stub here...
        }

        [TestMethod]
        public void ShouldDetectAdditions()
        {
            IEnumerable<RowDiff> diff = DeltaEngine.Diff<Fake.Employee>("fake_file1", "fake_file2");

            Assert.AreEqual(3, diff.Count(a => a.Status == RowStatus.Added));
        }

        [TestMethod]
        public void ShouldDetectUpdates()
        {
            IEnumerable<RowDiff> diff = DeltaEngine.Diff<Fake.Employee>("fake_file1", "fake_file2");

            Assert.AreEqual(3, diff.Count(a => a.Status == RowStatus.Updated));
        }

        [TestMethod]
        public void ShouldDetectDeletions()
        {
            IEnumerable<RowDiff> diff = DeltaEngine.Diff<Fake.Employee>("fake_file1", "fake_file2");

            Assert.AreEqual(3, diff.Count(a => a.Status == RowStatus.Deleted));
        }
    }
}
