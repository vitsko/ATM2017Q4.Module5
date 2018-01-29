namespace Assert
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    public class SoftAssertions
    {
        private readonly List<SingleAssert> verifications = new List<SingleAssert>();

        public void That(bool result, string message)
        {
            this.verifications.Add(new SingleAssert(result, message));
        }

        public void AssertAll()
        {
            var failed = this.verifications.Where(v => v.Failed).ToList();
            failed.Should().BeEmpty();
        }
    }
}