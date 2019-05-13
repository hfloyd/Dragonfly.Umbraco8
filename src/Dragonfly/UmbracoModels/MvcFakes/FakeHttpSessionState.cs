namespace Dragonfly.UmbracoModels.MvcFakes
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpSessionState(SessionStateItemCollection SessionItems)
        {
            _sessionItems = SessionItems;
        }

        public override void Add(string Name, object Value)
        {
            _sessionItems[Name] = Value;
        }

        public override int Count
        {
            get
            {
                return _sessionItems.Count;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _sessionItems.GetEnumerator();
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return _sessionItems.Keys;
            }
        }

        public override object this[string Name]
        {
            get
            {
                return _sessionItems[Name];
            }
            set
            {
                _sessionItems[Name] = value;
            }
        }

        public override object this[int Index]
        {
            get
            {
                return _sessionItems[Index];
            }
            set
            {
                _sessionItems[Index] = value;
            }
        }

        public override void Remove(string Name)
        {
            _sessionItems.Remove(Name);
        }
    }

   


}
