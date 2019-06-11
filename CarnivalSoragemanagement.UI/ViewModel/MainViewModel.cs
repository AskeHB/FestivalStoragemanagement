using CarnivalSoragemanagement.UI.View;
using CarnivalStoragemanagement.Data;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarnivalSoragemanagement.UI.Stuff.Events;

namespace CarnivalSoragemanagement.UI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _switchview;
        private CarnivalStoragemanagementDbContext db;
        public loginViewModel LoginViewModel { get; set; }
        public EditLoanViewModel EditLoanViewModel { get; set; }
        public EditUserViewModel EditUserViewModel { get; set; }
        public UserAdminViewModel UserAdminViewModel { get; set; }
        public ItemsAdminViewModel ItemsAdminViewModel { get; set; }
        public EditItemViewModel EditItemViewModel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        EventAggregator ea;
        public int SwitchView
        {
            get { return _switchview; }
            set
            {
                _switchview = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SwitchView"));
            }
        }
        public StartBorrowerView StartBorrowerView { get; set; }
        
        public StartBorrowerViewModel StartBorrowerViewModel { get; set; }
        public MainViewModel()
        {
            ea = new EventAggregator();
            db = new CarnivalStoragemanagementDbContext();
            StartBorrowerViewModel = new StartBorrowerViewModel(db,ea);
            EditItemViewModel = new EditItemViewModel(db, ea);
            ItemsAdminViewModel = new ItemsAdminViewModel(ea,db);
            UserAdminViewModel = new UserAdminViewModel(ea, db);
            EditLoanViewModel = new EditLoanViewModel(db,ea);
            LoginViewModel = new loginViewModel(db, ea);
            StartBorrowerView = new StartBorrowerView();
            ea.GetEvent<SwitchViewEvent>().Subscribe(SetSwitchView);
            EditUserViewModel = new EditUserViewModel(db, ea);
        }

        private void SetSwitchView(int obj)
        {
            SwitchView = obj;
        }

        
    }
    
}
