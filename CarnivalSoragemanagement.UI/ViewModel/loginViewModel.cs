using CarnivalStoragemanagement.Data;
using CarnivalStoragemanager.Model;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static CarnivalSoragemanagement.UI.Stuff.Events;

namespace CarnivalSoragemanagement.UI.ViewModel
{
    public class loginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private User _selectedUser;
        CarnivalStoragemanagementDbContext DbContext;
        IEventAggregator ea;
        private bool IsLoggedIn;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
            }
        }

        public ICommand LoginButton { get; set; }
        public loginViewModel(CarnivalStoragemanagementDbContext db, IEventAggregator eventAggregator)
        {
            DbContext = db;
            ea = eventAggregator;
            LoginButton = new DelegateCommand(OnLoginButtonExecute);
            SelectedUser = new User();
        }

        private void OnLoginButtonExecute()
        {
            var User = DbContext.Users.Where(f => f.StudentIdCard == SelectedUser.StudentIdCard).SingleOrDefault();
            var users = DbContext.Users.Include("Roles").ToList();
            foreach (var user in users)
            {
                if (User.ID == user.ID)
                {
                    User = user;
                }
            }
            if (User != null)
            {
                if (User.StudentIdCard == SelectedUser.StudentIdCard)
                {
                    IsLoggedIn = true;
                    SelectedUser = User;

                }
            }
            if (IsLoggedIn)
            {
                foreach (var role in User.Roles)
                {
                    if (role.Title == "Loaner")
                    {
                        ea.GetEvent<SwitchViewEvent>().Publish(0);
                    }
                }
                
                
                ea.GetEvent<ExchangeUserEvent>().Publish(SelectedUser);
            }
        }
    }
}
