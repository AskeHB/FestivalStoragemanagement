using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalStoragemanager.Model
{
    public class User
    {

        public User()
        {
            Loans = new Collection<Loan>();
            Roles = new Collection<Role>();
        }
        public int ID { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string StudentIdCard { get; set; }

        public ICollection<Loan> Loans { get; set; }

        public ICollection<Role> Roles { get; set; }



    }
}
