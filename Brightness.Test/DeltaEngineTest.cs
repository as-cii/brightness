using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Brightness.Diff;

namespace Brightness.Test
{
    [TestClass]
    public class DeltaEngineTest
    {
        private MemoryStream oldFile;
        private MemoryStream newFile;

        [TestInitialize]
        public void Before()
        {
            byte[] newFileBuffer;
            byte[] oldFileBuffer;
            Fake.Generator.GenerateSimilarLines(1000000, 1520, 6330, 2240, out newFileBuffer, out oldFileBuffer);

            oldFile = new MemoryStream(oldFileBuffer);
            newFile = new MemoryStream(newFileBuffer);
        }

        [TestCleanup]
        public void After()
        {
            oldFile.Dispose();
            newFile.Dispose();
        }

        [TestMethod]
        public void ShouldDetectAdditions()
        {
            var diff = DeltaEngine.Diff<Brightness.Test.Fake.Employee, int>(oldFile, newFile, a => a.Id);

            Assert.AreEqual(1520, diff.Count(a => a.Status == RowStatus.Added));
        }

        [TestMethod]
        public void ShouldDetectUpdates()
        {
            var diff = DeltaEngine.Diff<Brightness.Test.Fake.Employee, int>(oldFile, newFile, a => a.Id);

            Assert.AreEqual(6330, diff.Count(a => a.Status == RowStatus.Updated));
        }

        [TestMethod]
        public void ShouldDetectDeletions()
        {
            var diff = DeltaEngine.Diff<Brightness.Test.Fake.Employee, int>(oldFile, newFile, a => a.Id);

            Assert.AreEqual(2240, diff.Count(a => a.Status == RowStatus.Deleted));
        }
    }
}
