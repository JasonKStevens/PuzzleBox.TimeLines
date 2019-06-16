using NUnit.Framework;
using PuzzleBox.Time.Timelines;

namespace PuzzleBox.Time.Tests
{
  public class PayloadTests
  {
    [Test]
    public void Equality()
    {
      Assert.AreEqual(new CraftHoursPayload("Craft", 1.5m), new CraftHoursPayload("Craft", 1.5m));
    }
  }
}
