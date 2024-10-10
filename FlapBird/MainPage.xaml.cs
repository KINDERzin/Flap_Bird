namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int Gravidade = 2; // Pixel/Milisegundo
	const int tempoEntreFrames = 25; // Milisegundos
	bool estaMorto = true;

	public MainPage()
	{
		InitializeComponent();
	}

	void AplicaGravidade()
	{
		imagemPersonagem.TranslationY += Gravidade;
	}

	async Task Desenha()
	{
		while (!estaMorto)
		{
			AplicaGravidade();
			await Task.Delay(tempoEntreFrames);
		}

	}

	void OnGameOverClicked(object s, TappedEventArgs e)
	{
		frameGameOver.IsVisible = false;
		Inicializar();
		Desenha();

	}

	void Inicializar()
	{
		estaMorto = false;
		imagemPersonagem.TranslationY = 0;
	}
}

