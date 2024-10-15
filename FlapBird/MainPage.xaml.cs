namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int Gravidade = 2; // Pixel/Milisegundo
	const int tempoEntreFrames = 25; // Milisegundos
	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;

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
			GerenciarCanos();
			AplicaGravidade();
			if (VericaColizao())
			{
				estaMorto = true;
				frameGameOver.IsVisible = true;
				break;
			}
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
		posteBaixo.TranslationX = 0;
		posteCima.TranslationX = 0;
	}

	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
	}

	void GerenciarCanos()
	{
		posteCima.TranslationX -= velocidade;
		posteBaixo.TranslationX -= velocidade;
		if (posteBaixo.TranslationX < -larguraJanela)
		{
			posteBaixo.TranslationX = 20;
			posteCima.TranslationX = 20;
		}
	}

	bool VerificaColizaoTeto()
	{
		var minY = -alturaJanela / 2;

		if (imagemPersonagem.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColizaoChao()
	{
		var maxY = alturaJanela / 2 - imagemChao.HeightRequest - 210;

		if (imagemPersonagem.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColizao()
	{
		if (!estaMorto)
		{
			if (VerificaColizaoTeto() || VerificaColizaoChao())
			{
				return true;
			}
		}

		return false;
	}
}