using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalStoragemanager.Model
{
    public class Role
    {

        public Role()
        {
            Users = new Collection<User>();
        }
        public int ID { get; set; }

        public string Title { get; set; }

        public ICollection<User> Users { get; set; }



    }
}
