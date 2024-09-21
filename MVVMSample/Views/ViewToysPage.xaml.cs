
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

    #endregion
}