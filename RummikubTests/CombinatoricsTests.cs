using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RummikubLib;

namespace RummikubTests
{
    [TestFixture]
    public class CombinatoricsTests
    {
        public static IEnumerable<TestCaseData> GetSublistsTestCases = new[]
        {
            new TestCaseData(
                    new int[0],
                    new[] { new int[0] })
                .SetName("Empty list"),

            new TestCaseData(
                    new[] { 1 },
                    new[] { new int[0], new[] { 1 } })
                .SetName("List of size 1"),

            new TestCaseData(
                    new[] { 1, 2 },
                    new[] { new int[0], new[] { 1 }, new[] { 2 }, new[] { 1, 2 } })
                .SetName("List of size 2"),

            new TestCaseData(
                    new[] { 1, 1 },
                    new[] { new int[0], new[] { 1 }, new[] { 1 }, new[] { 1, 1 } })
                .SetName("List with duplicate element"),

            new TestCaseData(
                    new[] { 1, 2, 3 },
                    new[] { new int[0], new[] { 1 }, new[] { 2 }, new[] { 3 },
                        new[] { 1, 2 }, new[] { 2, 3 }, new[] { 1, 3 }, new[] { 1, 2, 3 } })
                .SetName("List of size 3")
        };

        [Test]
        [TestCaseSource(nameof(GetSublistsTestCases))]
        public void TestGetSubsets(IReadOnlyList<int> list, IEnumerable<IEnumerable<int>> expectedSubsets)
        {
            var actualSubsets = list.GetSublists();
            Assert.That(actualSubsets, Is.EquivalentTo(expectedSubsets).Using(new CollectionEqualityComparer<int>()));
        }
    }
}