using MVVMSample.Models;
using MVVMSample.Views;
using System.Text.Json;

namespace MVVMSample
{
    public partial class App : Application
    {
		public static User? user;

		#region Add Service Provider - retrieve services from DI container
		IServiceProvider provider;
		#endregion
		public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
			#region add Service proivder by injection
			provider = serviceProvider;
			#endregion
			//שינוי הצבעה ל 
			//SHELL
			MainPage = new AppShell();
			//Add Support for Loading page and modal login
			if (Preferences.Default.ContainsKey("userObj"))
			{
				string json = Preferences.Default.Get<string>("userObj",null);
				user=JsonSerializer.Deserialize<User>(json);	
			}
			if (user != null)
			{
				Shell.Current.GoToAsync("//MainPage");
			}
			else
			{

				Page login = provider?.GetService<LoginPage>();
				Shell.Current.Navigation.PushModalAsync(login);
				
			}
        }
    }
}
