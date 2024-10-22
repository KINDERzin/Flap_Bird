namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int Gravidade = 5; // Pixel/Milisegundo
	const int tempoEntreFrames = 25; // Milisegundos
	const int forcaPulo = 30;
	const int maxTempoPulando = 3;
	const int aberturaMinima = 80;

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
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		estaMorto = false;
		imagemPersonagem.TranslationY = 0;
		imagemPersonagem.TranslationX = 0;
		canoBaixo.TranslationX = -larguraJanela;
		canoCima.TranslationX = -larguraJanela;
		pontuacao = 0;
		GerenciarCanos();
	}

	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
	}

	void GerenciarCanos()
	{

		canoCima.TranslationX -= velocidade;
		canoBaixo.TranslationX -= velocidade;
		if (canoBaixo.TranslationX < -larguraJanela)
		{
			canoBaixo.TranslationX = 20;
			canoCima.TranslationX = 20;

			var alturaMaxima = -50;
			var alturaMinima = -canoBaixo.HeightRequest;

			canoCima.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			canoBaixo.TranslationY = canoCima.TranslationY + aberturaMinima + canoBaixo.HeightRequest;

			pontuacao++;
			labelPontuacao.Text = "Canos: " + pontuacao.ToString("D3");
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
		var maxY = alturaJanela / 2 - imagemChao.HeightRequest - 30;

		if (imagemPersonagem.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColizao()
	{
		if (!estaMorto)
		{
			if (VerificaColizaoTeto() || VerificaColizaoChao() || VerificaColisaoCanoCima() || VerificaColisaoCanoBaixo())
			{
				return true;
			}
		}

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
	}

	void OnGridClicked(object s, TappedEventArgs args)
	{
		estaPulando = true;
	}

	bool VerificaColisaoCanoCima()
	{
		//Posição horizontal
		var posicaoHPardal = (larguraJanela / 2) - (imagemPersonagem.WidthRequest / 2);
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
		//Posição horizontal
		var posicaoHPardal = (larguraJanela / 2) - (imagemPersonagem.WidthRequest / 2);
		//Posição vertical
		var posicaoVPardal = (alturaJanela / 2) - (imagemPersonagem.HeightRequest / 2) + imagemPersonagem.TranslationY;

		if (posicaoHPardal >= Math.Abs(canoBaixo.TranslationX) + canoBaixo.WidthRequest && 
		posicaoHPardal <= Math.Abs(canoBaixo.TranslationX) + canoBaixo.WidthRequest && 
		posicaoVPardal <= canoBaixo.HeightRequest + canoBaixo.TranslationY)
			return true;
		else
			return false;
	}

	
}