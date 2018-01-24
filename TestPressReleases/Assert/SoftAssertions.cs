namespace TestPressReleases.Assert
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    internal class SoftAssertions
    {
        private readonly List<SingleAssert> verifications = new List<SingleAssert>();

        internal void That(bool result, string message)
        {
            this.verifications.Add(new SingleAssert(result, message));
        }

        internal void AssertAll()
        {
            var failed = this.verifications.Where(v => v.Failed).ToList();
            failed.Should().BeEmpty();
        }
    }
}