using CarnivalStoragemanagement.Data;
using CarnivalStoragemanager.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static CarnivalSoragemanagement.UI.Stuff.Events;

namespace CarnivalSoragemanagement.UI.ViewModel
{
    public class UserAdminViewModel : INotifyPropertyChanged
    {
        CarnivalStoragemanagementDbContext dbContext;
        IEventAggregator ea;
        private User _selectedUser;
        public ICommand NewUser { get; set; }
        public ICommand EditUser { get; set; }
        public ICommand DeleteUser { get; set; }
        public ICommand Return { get; set; }
        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SearchString"));
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }
        public ObservableCollection<User> AllUsers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public UserAdminViewModel(IEventAggregator eventAggregator, CarnivalStoragemanagementDbContext db)
        {
            ea = eventAggregator;
            dbContext = db;

            AllUsers = new ObservableCollection<User>(dbContext.Users);
            Return = new DelegateCommand(OnReturnExecute);
            NewUser = new DelegateCommand(OnNewUserExecute);
            EditUser = new DelegateCommand(OnEditUserExecute);
            DeleteUser = new DelegateCommand(OnDeleteUserExecute);
            PropertyChanged += EditUsersViewModel_PropertyChanged;
        }

        private void EditUsersViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchString")
            {
                UsersSearch(SearchString);
            }
        }

        private void UsersSearch(string searchstring)
        {
            //if (searchstring != null)
            //{
            //    AllUsers.Clear();
            //    foreach (var role in dbContext.Users)
            //    {
            //        if (role.Firstname.ToLower().Contains(searchstring.ToLower()))
            //        {
            //            AllUsers.Add(role);
            //        }
            //    }
            //}

        }

        private void OnReturnExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(0);
        }

        private void OnEditUserExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(2);
            ea.GetEvent<ExchangeUserEvent>().Publish(SelectedUser);
        }

        private void OnDeleteUserExecute()
        {
            dbContext.Users.Remove(SelectedUser);
            AllUsers.Remove(SelectedUser);

            dbContext.SaveChanges();

        }

        private void OnNewUserExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(2);
            ea.GetEvent<ExchangeUserEvent>().Publish(new User() { Firstname = "NNull"});
        }
    }
}
