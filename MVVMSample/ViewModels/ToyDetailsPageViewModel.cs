using MVVMSample.Models;
using MVVMSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ICommand ChangePhotoCommand
        {
            get; protected set;
            
            
        }


        public string SelectedImage
        {
            get => SelectedToy?.Image; set
            {
                if (value != SelectedToy?.Image) { SelectedToy.Image = value; OnPropertyChanged(); }
            }

        }
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
                    OnPropertyChanged(nameof(SecondHandStatus)); OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        

        public string SecondHandStatus => SelectedToy?.IsSecondHand == true ? "Condition: Second Hand" : "Condition: New";

        public ToyDetailsPageViewModel(IToys service)
        {
            toyService = service ;
            ChangePhotoCommand = new Command(async () => 
            {
                SelectedImage = "loadingforever.gif";
                string choice=await Shell.Current.DisplayActionSheet(" בחר מקור", "ביטול", "בטל", "צלם","בחר קובץ");
                switch (choice)
                {
                    case "צלם":
                        break;
                    case "בחר קובץ":
                        break;
                    default:
                        break;
                }

            }
            );
            

        }
      


    }
}
