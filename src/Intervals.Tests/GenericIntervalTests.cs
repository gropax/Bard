using System;
using Xunit;

namespace Intervals.Tests
{
    public class GenericIntervalTests
    {
        [Theory]
        [InlineData(-1, 0, false)]
        [InlineData(0, 1, true)]
        [InlineData(0, 2, true)]
        [InlineData(1, 2, true)]
        [InlineData(1, 4, true)]
        [InlineData(2, 3, true)]
        [InlineData(3, 4, true)]
        [InlineData(3, 5, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 6, false)]
        public void TestIntersectsWithNonStrict(int start, int end, bool intersect)
        {
            var interval = new Interval(1, 3);
            var other = new Interval(start, end - start);
            Assert.Equal(intersect, interval.IntersectsWith(other, ContainsMode.NON_STRICT));
            Assert.Equal(intersect, other.IntersectsWith(interval, ContainsMode.NON_STRICT));
        }

        [Theory]
        [InlineData(-1, 0, false)]
        [InlineData(0, 1, false)]
        [InlineData(0, 2, true)]
        [InlineData(1, 2, true)]
        [InlineData(1, 4, true)]
        [InlineData(2, 3, true)]
        [InlineData(3, 4, true)]
        [InlineData(3, 5, true)]
        [InlineData(4, 5, false)]
        [InlineData(5, 6, false)]
        public void TestIntersectsWithStrict(int start, int end, bool intersect)
        {
            var interval = new Interval(1, 3);
            var other = new Interval(start, end - start);
            Assert.Equal(intersect, interval.IntersectsWith(other, ContainsMode.STRICT));
            Assert.Equal(intersect, other.IntersectsWith(interval, ContainsMode.STRICT));
        }
    }
}
