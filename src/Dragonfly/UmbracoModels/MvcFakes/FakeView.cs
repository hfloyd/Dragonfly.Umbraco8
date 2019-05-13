namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class FakeView : IView
    {
        public void Render(ViewContext ViewContext, TextWriter Writer)
        {
            throw new InvalidOperationException();
        }
    }
}
