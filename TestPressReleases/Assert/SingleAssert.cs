namespace TestPressReleases.Assert
{
    internal class SingleAssert
    {
        private readonly string message;
        private readonly bool result;

        internal SingleAssert(bool result, string message)
        {
            this.result = result;
            this.message = message;
        }

        internal bool Failed => !this.result;

        public override string ToString()
        {
            return this.message;
        }
    }
}