using MVVMSample.Models;
using MVVMSample.Views;
using System.Windows.Input;

namespace MVVMSample
{
    public partial class AppShell : Shell
    {

        public static User? user; 
      public ICommand LogoutCommand=> new Command(async () => await this.DisplayAlert("טרם פותח", "טרם פותח", "ביטול"));
        //{
        //    get; private set;   
        //}
        public AppShell()
        {
            
            InitializeComponent();
            BindingContext=this;    
            #region רישום מסכים פנימיים
            Routing.RegisterRoute("Details", typeof(ToyDetailsPage));
            Routing.RegisterRoute("Login",typeof(LoginPage));   
           
            #endregion
        }


      


    }
}
