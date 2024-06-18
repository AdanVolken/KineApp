using KineApp.DataAcces;
using KineApp.Model;

namespace KineApp.View;

public partial class SintomaPage : ContentPage
{
	private KineDBconexion _dbconexion;
	public SintomaPage()
	{
		InitializeComponent();
		_dbconexion = new KineDBconexion();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }

	private void LoadData() 
	{
		var sintomas = _dbconexion.GetItems<SintomaModel>();

        SintomasCollectionView.SelectedItem = sintomas;

    }
}