using MVVMSample.Models;
using MVVMSample.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVVMSample.ViewModels
{
   public class ViewToysPageViewModel:ViewModelBase
    {
        #region Fields
        private double? price;
        private ObservableCollection<Toy> toys;
        private IToys toyService;
        private List<Toy> fullList;
        private bool isRefreshing;
        
      


        #region נבחר מהרשימה
        private Toy selectedToy;
        public Toy SelectedToy
        {
            get
            {
                return selectedToy;
            }
            set
            {
                if (selectedToy != value)
                {
                    selectedToy = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region בחירת אוסף פריטים מהרישמה
       public ObservableCollection<object> SelectedToys
        {
            get; set;
        }
        
        #endregion


        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Toy> Toys
        {
            get => toys;
            set
            {
                if (toys != value)
                {
                    toys = value;
                    OnPropertyChanged();
                }
            }
        }
        public double? Price
        {
            get
            {
                return price;
            }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged();
                    RefreshCommands();
                }
            }
        }

        #endregion

        #region COMMANDS

        public ICommand DeleteCommand
        {
        get; private set; }
        public ICommand RefreshCommand
        {
            get; private set;
        }
        public ICommand FilterAbovePriceCommand
        {
            get; private set;
        }

        public ICommand FilterBelowPriceCommand
        {
            get;private set;
        }

        #region Navigation
        public ICommand ShowDetailsCommand
        {
            get;private set;
        }
        //Shell Navigation Pass Arguments


        //Shell Navigation Pass Object

        #endregion

        #endregion

        #region Constructor
        public ViewToysPageViewModel(IToys service)
        {
            #region Init Data
            Price = null;
            toyService=service;
         //   toys=new ObservableCollection<Toy>(toyService.GetToys());
           
           
           
            #endregion

            #region Init Commands
            FilterAbovePriceCommand = new Command(execute:FilterAbove,()=>Toys!=null&&Toys.Count>0);
            FilterBelowPriceCommand = new Command(FilterBelow,()=>Price>0);
            RefreshCommand = new Command(Refresh);
            DeleteCommand = new Command<Toy>(async(t) => {if(await toyService.DeleteToy(t))Refresh(); });

            #region Navigation Commands
            //Navigation with Parametes
            // ShowDetailsCommand = new Command(async() => { await GotoWithArguments(); });

            //Navigation With Object
            ShowDetailsCommand = new Command(async() => { await GoToDetailsPage(); });
            #endregion

            #region Commands By LINQ
            //FilterAbovePriceCommand = new Command(() => Toys = new ObservableCollection<Toy>(Toys.Where(t => t.Price > Price)));
            //FilterBelowPriceCommand = new Command(() => {
            //    var toys = Toys.Where(t => t.Price > Price);
            //    foreach (var toy in toys)
            //    {
            //       Toys.Remove(toy);
            //    }
            //});
            #endregion

            #endregion

        }


        #endregion

        #region Methods
        private async void Refresh()
        {
            IsRefreshing = true;
            fullList = await toyService.GetToys();
            Toys = new ObservableCollection<Toy>(fullList);
            Price = null;
            RefreshCommands();
            IsRefreshing = false;
        }
        private void FilterAbove()
        {
            //כל הצעצועים שגדולים מהמחיר
            var toys = fullList.Where(t => t.Price > Price).ToList();
            Toys.Clear();
            foreach (var t in toys)
                Toys.Add(t);
            RefreshCommands();
            
        } private void FilterBelow()
        {
            var toys = fullList?.Where(t => t.Price <= Price);
           if(Toys!=null&& Toys.Count>0) 
            Toys.Clear();
            if(toys!=null&&Toys!=null)
            foreach (var t in toys)
                Toys.Add(t);
            RefreshCommands();

        }

        private void RefreshCommands()
        {
            var filterabove = FilterAbovePriceCommand as Command;
            if (filterabove != null)
            {
                filterabove.ChangeCanExecute();

            }
            var filterbelow = FilterBelowPriceCommand as Command;

            filterbelow?.ChangeCanExecute();

            

        }
        #region Navigation Methods
        //Navigation with Object
        private async Task GoToDetailsPage()
        {
            if (SelectedToy == null)
                return;
            //האובייקטים שנרצה להעביר יישמרו במילון
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Toy", SelectedToy);
            //נשלח את המידע עם הפניה למסך
            await Shell.Current.GoToAsync("/Details", data);
            //נבטל את הבחירה בחזרה למסך הקודם
            SelectedToy = null;
            

           
        }
        //Navigation With Parameters
        private async Task GotoWithArguments()
        {
            if (SelectedToy != null)
            {
                await Shell.Current.GoToAsync($"/Details?id={SelectedToy.Id}");

                SelectedToy = null;
            }
            
        }
        #endregion
        async Task GetToysAsync()
        {
            fullList = await toyService.GetToys();
           
            Toys = new ObservableCollection<Toy>();
            if(fullList != null)
            foreach (var toy in fullList)
                Toys.Add(toy);
        }

        #endregion


    }
}
