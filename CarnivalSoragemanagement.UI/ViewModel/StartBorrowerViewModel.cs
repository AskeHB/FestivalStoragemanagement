using CarnivalStoragemanagement.Data;
using CarnivalStoragemanager.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using static CarnivalSoragemanagement.UI.Stuff.Events;

namespace CarnivalSoragemanagement.UI.ViewModel
{
    public class StartBorrowerViewModel : INotifyPropertyChanged
    {
        private CarnivalStoragemanagementDbContext dbContext;
        public event PropertyChangedEventHandler PropertyChanged = delegate {};
        IEventAggregator ea;
        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }


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
        public ICommand NewLoan { get; set; }
        public ICommand DeleteLoan { get; set; }

        public ICommand EditItems { get; set; }
        public ICommand EditUsers { get; set; }
        
        private ObservableCollection<Loan> _allLoans;

        public ObservableCollection<Loan> AllLoans
        {
            get { return _allLoans; }
            set {
                    _allLoans = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AllLoans"));
                }
        }

        public StartBorrowerViewModel(CarnivalStoragemanagementDbContext db, IEventAggregator eventAggregator)
        {
            ea = eventAggregator;
            dbContext = db;
            //var loans = db.Loans.Include("Item").Include("Borrower").ToList();
            AllLoans = new ObservableCollection<Loan>();
            updateLoans();
            EditUsers = new DelegateCommand(OnEditUsersExecute);
            EditItems = new DelegateCommand(OnEditItemsExecute);
            NewLoan = new DelegateCommand(OnNewLoanExecute);
            DeleteLoan = new DelegateCommand(OnDeleteLoanExecute);
            ea.GetEvent<ExchangeUserEvent>().Subscribe(SetUser);
            ea.GetEvent<NotifyProvider>().Subscribe(OnNotifyExecute);
            PropertyChanged += StartBorrowerViewModel_PropertyChanged;
            
        }

        private void OnEditItemsExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(5);
        }

        private void StartBorrowerViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedLoan")
            {
                ea.GetEvent<ExchangeLoanEvent>().Publish(SelectedLoan);
                ea.GetEvent<SwitchViewEvent>().Publish(1);
            }
        }

        private void OnNotifyExecute()
        {
            updateLoans();
        }

        private void updateLoans()
        {
            var result = dbContext.Loans.Include("Item").Include("Borrower").ToList();
            AllLoans.Clear();
            foreach (var Loan in result)
            {
                AllLoans.Add(Loan);
            }
        }
        private void OnEditUsersExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(4);
        }

        private void SetUser(User obj)
        {
            SelectedUser = obj;
        }

        private void OnDeleteLoanExecute()
        {
            dbContext.Loans.Remove(SelectedLoan);
            dbContext.SaveChanges();

        }

        private void OnNewLoanExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(1);
           
            
           
            ea.GetEvent<ExchangeLoanEvent>().Publish(null);
            
            
        }
    }
}
