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
            //Add Support for Loading page and modal login
        }
    }
}
