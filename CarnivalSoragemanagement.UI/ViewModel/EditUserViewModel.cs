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
    public class EditUserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private User _selectedUser;
        IEventAggregator ea;
        CarnivalStoragemanagementDbContext db;
        private Role _selectedRole;
        private bool IsNewUser;
        public ICommand SaveUser { get; set; }
        public ICommand RegretUserChanges { get; set; }
        private Role _selectedAllRoles;
        public ObservableCollection<Role> AllRoles { get; set; }

        public Role SelectedAllRoles
        {
            get { return _selectedAllRoles; }
            set {
                _selectedAllRoles = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAllRoles"));
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
        

        public Role SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedRole"));
            }
        }

        public EditUserViewModel(CarnivalStoragemanagementDbContext carnivalStoragemanagementDbContext, IEventAggregator eventAggregator)
        {
            db = carnivalStoragemanagementDbContext;
            ea = eventAggregator;
            SaveUser = new DelegateCommand(OnSaveUserExecute);
            RegretUserChanges = new DelegateCommand(OnRegretUserChangesExecute);
            ea.GetEvent<ExchangeUserEvent>().Subscribe(OnReciveUser);
            AllRoles = new ObservableCollection<Role>(db.Roles);
        }

        private void OnReciveUser(User obj)
        {
            if (obj.Firstname == "NNull")
            {
                SelectedUser = new User();
                IsNewUser = true;
            }
            else
            {
                SelectedUser = obj;
                IsNewUser = false;
            }
        }

        private void OnRegretUserChangesExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(4);
        }
        
        private void OnSaveUserExecute()
        {
            if (IsNewUser)
            {
                db.Users.Add(SelectedUser);
                db.SaveChanges();
            }
            else if (!IsNewUser)
            {
                foreach (var User in db.Users)
                {
                    if (User.ID == SelectedUser.ID)
                    {
                        var user = db.Users.Find(User);
                        user = SelectedUser;
                        db.SaveChanges();
                        break;
                    }
                }
            }
            
            ea.GetEvent<SwitchViewEvent>().Publish(4);
        }
    }
}
