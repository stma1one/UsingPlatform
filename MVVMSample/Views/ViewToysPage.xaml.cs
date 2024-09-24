
using MVVMSample.Helpers;
using MVVMSample.ViewModels;

namespace MVVMSample.Views;

public partial class ViewToysPage : ContentPage
{
	public ViewToysPage(ViewToysPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

	}
	#region Method ביצוע פעולות אסינכרוניות בזמן עליית הדף
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (App.user == null)
			await Shell.Current.GoToAsync("Login");
		else
		{
			var vm = BindingContext as ViewToysPageViewModel;
			 vm?.LoadData().Awaiter();
		}
	}	
		#endregion

}