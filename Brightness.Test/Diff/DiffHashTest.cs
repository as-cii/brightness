using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brightness.Diff;
using System.Linq;

namespace Brightness.Test.Diff
{
    [TestClass]
    public class DiffHashTest
    {
        [TestMethod]
        public void ShouldMergeCollisionsAndChangeStatus()
        {
            DiffHash<int, string> x = new DiffHash<int, string>();
            x.MergeAdd(1, "hello", RowStatus.Added);
            x.MergeAdd(1, "goodbye", RowStatus.Deleted);
            x.MergeAdd(2, "x", RowStatus.Added);

            Assert.AreEqual(2, x.Count());
            Assert.AreEqual(RowStatus.Updated, x.First().Status);
            Assert.AreEqual("hello", x.First().Row);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionIfMultipleAddAreFound()
        {
            DiffHash<int, string> x = new DiffHash<int, string>();
            
            x.MergeAdd(1, "hello", RowStatus.Added);
            x.MergeAdd(1, "goodbye", RowStatus.Added);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionIfMultipleDeleteAreFound()
        {
            DiffHash<int, string> x = new DiffHash<int, string>();

            x.MergeAdd(1, "hello", RowStatus.Deleted);
            x.MergeAdd(1, "goodbye", RowStatus.Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ShouldThrowExceptionIfTryingToMergeAnUpdatedRow()
        {
            DiffHash<int, string> x = new DiffHash<int, string>();

            x.MergeAdd(1, "hello", RowStatus.Updated);
            x.MergeAdd(1, "goodbye", RowStatus.Deleted);
        }
    }
}
