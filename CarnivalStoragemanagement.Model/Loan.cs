using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalStoragemanager.Model
{
    public class Loan
    {
        public Loan()
        {
            Borrower = new User();
            Item = new Item();
            ReturnDate = new DateTime();
            LoanDate = new DateTime();
        }
        public int ID { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime LoanDate { get; set; }

        public bool Consumable { get; set; }

        public User Borrower { get; set; }

        public Item Item { get; set; }





    }
}
