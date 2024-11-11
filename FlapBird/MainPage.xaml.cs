namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int Gravidade = 5; // Pixel/Milisegundo
	const int tempoEntreFrames = 25; // Milisegundos
	const int forcaPulo = 30;
	const int maxTempoPulando = 3;
	const int aberturaMinima = 200;

	int velocidade = 5;
	int tempoPulando = 0;
	int pontuacao = 0;

	double larguraJanela = 0;
	double alturaJanela = 0;

	bool estaMorto = true;
	bool estaPulando = false;


	public MainPage()
	{
		InitializeComponent();
		SoundHelper.Play("fundo.wav");
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	void AplicaGravidade()
	{
		imagemPersonagem.TranslationY += Gravidade;
	}

	async Task Desenha()
	{
		while (!estaMorto)
		{
			if (estaPulando)
				AplicaPulo();
			else
				AplicaGravidade();

			GerenciarCanos();

			if (VericaColizao())
			{
				estaMorto = true;
				SoundHelper.Play("morte.wav");
				frameGameOver.IsVisible = true;
				labelGameOver.Text = $"Você passou \n por {pontuacao} \n canos!";
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}

	}

	void OnGameOverClicked(object s, TappedEventArgs e)
	{
		frameGameOver.IsVisible = false;
		SoundHelper.Play("comeco.wav");
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		estaMorto = false;

		canoBaixo.TranslationX = -larguraJanela;
		canoCima.TranslationX = -larguraJanela;
		imagemPersonagem.TranslationY = 0;
		imagemPersonagem.TranslationX = 0;
		pontuacao = 0;

		GerenciarCanos();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		larguraJanela = width;
		alturaJanela = height;

		if(height > 0)
		{
			canoCima.HeightRequest = height;
			canoBaixo.HeightRequest = height;
		}
	}

	void GerenciarCanos()
	{

		canoCima.TranslationX -= velocidade;
		canoBaixo.TranslationX -= velocidade;
		if (canoBaixo.TranslationX < -larguraJanela)
		{
			canoBaixo.TranslationX = 20;
			canoCima.TranslationX = 20;

			var alturaMaxima = -(canoBaixo.HeightRequest * 0.2);
			var alturaMinima = -(canoBaixo.HeightRequest * 0.8);

			canoCima.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			canoBaixo.TranslationY = canoCima.HeightRequest + canoCima.TranslationY + aberturaMinima;

			pontuacao++;
			SoundHelper.Play("ponto.wav");
			labelPontuacao.Text = "Canos: " + pontuacao.ToString("D3");

			if(pontuacao % 4 == 0)
				velocidade++;
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
		var maxY = alturaJanela / 2 - imagemChao.HeightRequest;

		if (imagemPersonagem.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColizao()
	{
		return (!estaMorto && (VerificaColizaoChao() || VerificaColizaoTeto() || VerificaColizaoCano()));
	}

	bool VerificaColizaoCano()
	{
		if(VerificaColisaoCanoBaixo() || VerificaColisaoCanoCima())
			return true;
		else 
			return false;
	}

	void AplicaPulo()
	{
		imagemPersonagem.TranslationY -= forcaPulo;
		tempoPulando++;

		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
		SoundHelper.Play("pulo.wav");
	}

	void OnGridClicked(object s, TappedEventArgs args)
	{
		estaPulando = true;
	}

	bool VerificaColisaoCanoCima()
	{
		//Posição horizontal
		var posicaoHPardal = larguraJanela - 50 - (imagemPersonagem.WidthRequest / 2);
		//Posição vertical
		var posicaoVPardal = (alturaJanela / 2) - (imagemPersonagem.HeightRequest / 2) + imagemPersonagem.TranslationY;

		if (posicaoHPardal >= Math.Abs(canoCima.TranslationX) - canoCima.WidthRequest &&
		posicaoHPardal <= Math.Abs(canoCima.TranslationX) + canoCima.WidthRequest &&
		posicaoVPardal <= canoCima.HeightRequest + canoCima.TranslationY)
			return true;
		else
			return false;
	}

	bool VerificaColisaoCanoBaixo()
	{
		var posHPomba = larguraJanela - 50 - imagemPersonagem.WidthRequest / 2;
		var posVPomba = (alturaJanela / 2) + (imagemPersonagem.HeightRequest / 2) + imagemPersonagem.TranslationY;
		var yMaxCano = canoCima.HeightRequest + canoCima.TranslationY + aberturaMinima;
		if (posHPomba >= Math.Abs(canoCima.TranslationX) - canoCima.WidthRequest &&
		 	posHPomba <= Math.Abs(canoCima.TranslationX) + canoCima.WidthRequest &&
		 	posVPomba >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}