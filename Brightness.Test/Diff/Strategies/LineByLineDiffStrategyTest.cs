using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Brightness.Diff;
using Brightness.Diff.Strategies;
using Brightness.Test.Fake;

namespace Brightness.Test.Strategies
{
    [TestClass]
    public class LineByLineDiffStrategyTest
    {
        private MemoryStream oldFile;
        private MemoryStream newFile;

        [TestInitialize]
        public void Before()
        {
            byte[] newFileBuffer;
            byte[] oldFileBuffer;
            Fake.Generator.GenerateSimilarLines(1000, 100, 100, 100, out newFileBuffer, out oldFileBuffer);

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
            
            var diff = new LineByLineDiffStrategy().CreateDiff(oldFile, newFile, (Employee a) => a.Id);

            Assert.AreEqual(100, diff.Count(a => a.Status == RowStatus.Added));
        }

        [TestMethod]
        public void ShouldDetectUpdates()
        {
            var diff = new LineByLineDiffStrategy().CreateDiff(oldFile, newFile, (Employee a) => a.Id);

            Assert.AreEqual(100, diff.Count(a => a.Status == RowStatus.Updated));
        }

        [TestMethod]
        public void ShouldDetectDeletions()
        {
            var diff = new LineByLineDiffStrategy().CreateDiff(oldFile, newFile, (Employee a) => a.Id);

            Assert.AreEqual(100, diff.Count(a => a.Status == RowStatus.Deleted));
        }
    }
}
