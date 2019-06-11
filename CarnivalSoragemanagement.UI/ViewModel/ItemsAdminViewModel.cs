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
    public class ItemsAdminViewModel : INotifyPropertyChanged
    {
        CarnivalStoragemanagementDbContext dbContext;
        IEventAggregator ea;
        private Item _selectedItem;
        public ICommand NewItem { get; set; }
        public ICommand EditItem { get; set; }
        public ICommand DeleteItem { get; set; }
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

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedItem"));
            }
        }
        public ObservableCollection<Item> AllItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ItemsAdminViewModel(IEventAggregator eventAggregator, CarnivalStoragemanagementDbContext db)
        {
            ea = eventAggregator;
            dbContext = db;

            AllItems = new ObservableCollection<Item>(dbContext.Items);
            Return = new DelegateCommand(OnReturnExecute);
            NewItem = new DelegateCommand(OnNewItemExecute);
            EditItem = new DelegateCommand(OnEditItemExecute);
            DeleteItem = new DelegateCommand(OnDeleteItemExecute);
            PropertyChanged += EditItemsViewModel_PropertyChanged;
        }

        private void EditItemsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchString")
            {
                ItemsSearch(SearchString);
            }
        }

        private void ItemsSearch(string searchstring)
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

        private void OnEditItemExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(6);
            ea.GetEvent<ExchangeItemEvent>().Publish(SelectedItem);
        }

        private void OnDeleteItemExecute()
        {
            dbContext.Items.Remove(SelectedItem);
            AllItems.Remove(SelectedItem);

            dbContext.SaveChanges();

        }

        private void OnNewItemExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(6);
            ea.GetEvent<ExchangeItemEvent>().Publish(new Item() { ProductName = "NNull" });


        }
    }
}
