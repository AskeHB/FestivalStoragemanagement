using CarnivalStoragemanager.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalSoragemanagement.UI.Stuff
{
    public class Events
    {
        public class SwitchViewEvent : PubSubEvent<int> { }
        public class ExchangeLoanEvent : PubSubEvent<Loan> { }
        public class ExchangeUserEvent : PubSubEvent<User> { }
        public class ExchangeItemEvent : PubSubEvent<Item> { }
        public class NotifyProvider : PubSubEvent { }


    }
}
