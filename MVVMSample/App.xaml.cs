using MVVMSample.Views;

namespace MVVMSample
{
    public partial class App : Application
    {
		#region Add Service Provider - retrieve services from DI container
		
		#endregion
		public App()
        {
            InitializeComponent();
			#region add Service proivder by injection
			
			#endregion
			//שינוי הצבעה ל 
			//SHELL
			MainPage = new AppShell();
            //Add Support for Loading page and modal login
        }
    }
}
