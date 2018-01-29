namespace Assert
{
    public class SingleAssert
    {
        private readonly string message;
        private readonly bool result;

        public SingleAssert(bool result, string message)
        {
            this.result = result;
            this.message = message;
        }

        public bool Failed => !this.result;

        public override string ToString()
        {
            return this.message;
        }
    }
}