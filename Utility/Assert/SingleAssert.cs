namespace Assert
{
    public class SingleAssert
    {
        public SingleAssert(bool result, string message)
        {
            this.Result = result;
            this.Message = message;
        }

        public string Message { get; private set; }

        public bool Result { get; private set; }

        public bool Failed => !this.Result;

        public override string ToString()
        {
            return this.Message;
        }
    }
}