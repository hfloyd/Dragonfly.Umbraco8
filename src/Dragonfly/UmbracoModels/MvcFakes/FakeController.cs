namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class FakeController : ControllerBase
    {
        public void Execute(RequestContext RequestContext)
        {
            throw new InvalidOperationException();
        }

        protected override void ExecuteCore()
        {
            throw new InvalidOperationException();
        }
    }
}
