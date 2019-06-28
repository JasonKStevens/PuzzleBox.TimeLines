using NUnit.Framework;
using PuzzleBox.Time.Payloads;
using PuzzleBox.Time.Timelines;

namespace PuzzleBox.Time.Tests
{
    public class PayloadTests
    {
        [Test]
        public void Equality()
        {
            Assert.AreEqual(new KeyCountPayload("Craft", 1.5m), new KeyCountPayload("Craft", 1.5m));
        }
    }
}
