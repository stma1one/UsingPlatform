using MVVMSample.Views;

namespace MVVMSample
{
    public partial class App : Application
    {
       public IServiceProvider ServiceProvider
        {
            get; protected set;
        }
        public App(IServiceProvider service)
        {
            InitializeComponent();
            ServiceProvider = service;
            //שינוי הצבעה ל 
            //SHELL
            MainPage = new AppShell();
            Shell.Current.GoToAsync("//LoadingPage");
            var page = ServiceProvider?.GetService<LoginPage>();
            Shell.Current.Navigation.PushModalAsync(page);
        }
    }
}
