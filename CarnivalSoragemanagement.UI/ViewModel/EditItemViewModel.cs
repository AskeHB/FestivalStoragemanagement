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
    public class EditItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private Item _selectedItem;
        IEventAggregator ea;
        CarnivalStoragemanagementDbContext db;
        
        private bool IsNewItem;
       
        public ICommand SaveItem { get; set; }
        public ICommand RegretItemChanges { get; set; }
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public EditItemViewModel(CarnivalStoragemanagementDbContext carnivalStoragemanagementDbContext, IEventAggregator eventAggregator)
        {
            db = carnivalStoragemanagementDbContext;
            ea = eventAggregator;
            SaveItem = new DelegateCommand(OnSaveItemExecute);
            
            RegretItemChanges = new DelegateCommand(OnRegretItemChangesExecute);
            ea.GetEvent<ExchangeItemEvent>().Subscribe(OnReciveItem);

        }

        private void OnReciveItem(Item obj)
        {
            
            if (obj.ProductName == "NNull")
            {
                SelectedItem = new Item();
                IsNewItem = true;
            }
            else
            {
                SelectedItem = obj;
                IsNewItem = false;
            }
        }
        
        private void OnRegretItemChangesExecute()
        {
            ea.GetEvent<SwitchViewEvent>().Publish(5);
        }

        private void OnSaveItemExecute()
        {
            
            if (IsNewItem)
            {
                db.Items.Add(SelectedItem);
                db.SaveChanges();
            }
            else if (!IsNewItem)
            {
                foreach (var item in db.Items)
                {
                    if (item.Id == SelectedItem.Id)
                    {
                        var Result = db.Items.Find(item.Id);
                        Result = SelectedItem;
                        
                        break;
                    }
                }
                db.SaveChanges();
            }
            ea.GetEvent<SwitchViewEvent>().Publish(5);
        }
    }
}
