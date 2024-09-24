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
            ChangePhotoCommand = new Command(async () => await ChangeImage()

            );
            

        }
        public async Task ChangeImage()
        {
            FileResult photo = null;
            var backup = SelectedImage;
            SelectedImage = "loadingforever.gif";
            string choice = await Shell.Current.DisplayActionSheet(" בחר מקור", "ביטול", "בטל", "צלם", "בחר קובץ");
            try
            {
                switch (choice)
                {
                    case "צלם":
                        MediaPickerOptions options = new MediaPickerOptions() { Title = "צלם" };
                        photo = await MediaPicker.Default.CapturePhotoAsync(options);
                        break;
                    case "בחר קובץ":
                        options = new MediaPickerOptions() { Title = "בחר קובץ" };
                        photo = await MediaPicker.Default.PickPhotoAsync(options);
                        break;
                    default:
                        break;
                }
                if (photo != null)
                {
                    bool success = await toyService.UploadToyImage(photo, SelectedToy);
                    if (success)
                    {
                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream sourceStream = await photo.OpenReadAsync();
                        using FileStream localFileStream = File.OpenWrite(localFilePath);

                        await sourceStream.CopyToAsync(localFileStream);
                        SelectedImage = localFilePath;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            await Shell.Current.DisplayAlert("משהו השתבש", "לא הצלחתי להעלות תמונה", "אישור");
            SelectedImage = backup;
            return;



        }


   }   
}
