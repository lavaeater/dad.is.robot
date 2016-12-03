using NUnit.Framework;
using Otter.Custom;

namespace robot.dad.tests
{
    [TestFixture]
    public class RangeForcerTests
    {
        [Test]
        public void TestMaxRange_ReturnsValueInRange()
        {
            Assert.That(242f.ForceRange(100, 1), Is.EqualTo(100f));
        }
    }
}
