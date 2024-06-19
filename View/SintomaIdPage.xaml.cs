using KineApp.DataAcces;
using KineApp.Model;


namespace KineApp.Views;											

public partial class SintomaIdPage : ContentPage
{
	private KineDBconexion _dbConexion;
	private int _id;
	public SintomaIdPage(int idSintoma)
	{
		InitializeComponent();
		_dbConexion = new KineDBconexion();
		_id = idSintoma;
		MostarSintomas();
	}

	public void MostarSintomas()
	{
        var sintoma = _dbConexion.GetItems<SintomaModel>()
                              .FirstOrDefault(s => s.IdSintoma == _id);
        if (sintoma != null)
        {
            SintomaNombre.Title = sintoma.Nombre;
        }

        //Buscar la lista de musculos relacionados con el sintoma
        var musculosSintoma = _dbConexion.GetItems<MusculoSintomaModel>().Where(ms => ms.IdSintoma == _id).Select(ms => ms.IdMusculo).ToList();

		//Buscamos en la tabla musculos 
		var musculo = _dbConexion.GetItems<MusculoModel>().Where(m => musculosSintoma.Contains(m.IdMusculo)).ToList();

        // Cargamos a la lista 
        MusculosCollectionView.ItemsSource = musculo;

		var sintomaParte = _dbConexion.GetItems<SintomaParte>().Where(sp => sp.IdSintoma == _id).Select(sp => sp.IdParte).ToList();

		var parte = _dbConexion.GetItems<ParteModel>().Where(p => sintomaParte.Contains(p.IdParte)).ToList();

        SintomasCollectionView.ItemsSource = parte;

    }																						 
}