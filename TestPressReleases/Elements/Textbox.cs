namespace Tests.Elements
{
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors;

    public class Textbox : Element, ITypeable
    {
        public Textbox(string locator, string name=null) : base(locator, name)
        {
        }
    }
}
