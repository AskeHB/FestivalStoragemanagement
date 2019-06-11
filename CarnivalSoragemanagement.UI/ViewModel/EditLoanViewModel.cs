using CarnivalStoragemanagement.Data;
using CarnivalStoragemanager.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static CarnivalSoragemanagement.UI.Stuff.Events;

namespace CarnivalSoragemanagement.UI.ViewModel
{
    public class EditLoanViewModel : INotifyPropertyChanged
    {
        EventAggregator ea;
        
        CarnivalStoragemanagementDbContext db;

        bool isthere;
        public ICommand DeleteLoan { get; set; }
        public ICommand SaveLoan { get; set; }
        public ICommand RegretChanges { get; set; }
        public ICommand AddBorrower { get; set; }
        public ICommand AddItem { get; set; }

        private Loan _selectedLoan;

        public Loan SelectedLoan
        {
            get { return _selectedLoan; }
            set
            {
                _selectedLoan = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLoan"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public EditLoanViewModel(CarnivalStoragemanagementDbContext dbcontext, EventAggregator eventAggregator)
        {

            RfidReader rdr =  RfidReader.Instance;
            ea = eventAggregator;
            db = dbcontext;
            SaveLoan = new DelegateCommand(OnSaveLoanExecute);
            RegretChanges = new DelegateCommand(OnRegretChangesExecute);
            AddBorrower = new DelegateCommand(OnAddBorrowerExecute);
            AddItem = new DelegateCommand(OnAddItemExecute);
            DeleteLoan = new DelegateCommand(OnDeleteLoanExecute);
            ea.GetEvent<ExchangeLoanEvent>().Subscribe(SetSelectedLoan);
            
            
        }

        private void OnDeleteLoanExecute()
        {
            db.Loans.Remove(SelectedLoan);
            db.SaveChanges();
            ea.GetEvent<SwitchViewEvent>().Publish(0);
        }

        private void OnAddItemExecute()
        {
            isthere = false;
            var id = 0;
            foreach (var dbitem in db.Items)
            {
                if (dbitem.PartNumber == SelectedLoan.Item.PartNumber)
                {
                    id = dbitem.Id;
                    isthere = true;
                    
                }
                
            }
            if (!isthere)
            {
                
                //lav msgbox med ikke modtaget feedback
            }
            else if (isthere)
            {
                Debug.WriteLine("Item found!");
                var item = db.Items.Find(id);
                SelectedLoan.Item = item;
            }


        }

        private void OnAddBorrowerExecute()
        {
            isthere = false;
            var id = 0;
            foreach (var dbuser in db.Users)
            {
                if (dbuser.StudentIdCard == SelectedLoan.Borrower.StudentIdCard)
                {
                    id = dbuser.ID;
                    isthere = true;

                }

            }
            if (!isthere)
            {
                
                //lav msgbox med ikke modtaget feedback
            }
            else if (isthere)
            {
                Debug.WriteLine("Borrower found!");
                var user = db.Users.Find(id);
                SelectedLoan.Borrower = user;
            }
        }

        private void SetSelectedLoan(Loan obj)
        {
            if (obj == null)
            {
                SelectedLoan = new Loan()
                {
                    ID = 0
                };

            }
            else if (obj != null)
            {
                SelectedLoan = obj;
            }
            
        }

        private void OnRegretChangesExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(0);
        }

        private void OnSaveLoanExecute()
        {
            var id = 0;
            isthere = false;
            ea.GetEvent<SwitchViewEvent>().Publish(0);
            foreach (var Loan in db.Loans)
            {
                if (Loan.ID == SelectedLoan.ID)
                {
                    
                    isthere = true;
                    id = Loan.ID;
                }
                
                
            }
            if (isthere)
            {
                var loan = db.Loans.Find(id);
                loan = SelectedLoan;
                
            }
            else if (!isthere)
            {
                db.Loans.Add(SelectedLoan);
            }
            
            db.SaveChanges();
            ea.GetEvent<NotifyProvider>().Publish();
        }
    }
}
