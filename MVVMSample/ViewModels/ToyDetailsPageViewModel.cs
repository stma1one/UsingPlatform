using MVVMSample.Models;
using MVVMSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample.ViewModels
{
    #region פרמטרים ממסכים אחרים
    //פרמטר ראשון - השם של התכונה במסך החדש
    //פרמטר שני- שם של המפתח במילון או השם של הפרמטר במחרוזת 

    [QueryProperty(nameof(SelectedToy),"Toy")]
    [QueryProperty(nameof(Id),"id")]
    #endregion
   public class ToyDetailsPageViewModel : ViewModelBase
    {
        private IToys toyService;
        private int id;
        private Toy? selectedToy;


        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();

                    //Fetch the Toy by Id
                    FetchToyById();
                }
               

            }
        }

        private async void FetchToyById()
        {
            SelectedToy = (await toyService?.GetToys())?.Where(t => t.Id == id).FirstOrDefault();
        }

        public Toy? SelectedToy
        {
            get => selectedToy;
            set
            {
                if (selectedToy != value)
                {
                    selectedToy = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SecondHandStatus));
                }
            }
        }

        

        public string SecondHandStatus => SelectedToy?.IsSecondHand == true ? "Condition: Second Hand" : "Condition: New";

        public ToyDetailsPageViewModel(IToys service)
        {
            toyService = service ;
            

        }


    }
}
