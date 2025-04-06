using MauiAppTempoRafonha.Models;
using MauiAppTempoRafonha.Services;
using System.Net;

namespace MauiAppTempoRafonha
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n" +
                                         $"Clima: {t.description} \n" +
                                         $"Vento: {t.speed} m/s \n" +
                                         $"Visibilidade: {t.visibility} metros \n";

                        lbl_res.Text = dados_previsao;

                    }
                    else
                    {
                        // Se GetPrevisao retornar null, vamos verificar o status code diretamente
                        HttpResponseMessage response = await DataService.GetResponseStatus(txt_cidade.Text);

                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            lbl_res.Text = "Cidade não encontrada. Verifique o nome digitado.";
                        }
                        else if (!response.IsSuccessStatusCode)
                        {
                            lbl_res.Text = $"Erro ao obter a previsão: {response.StatusCode}";
                        }
                        else
                        {
                            lbl_res.Text = "Sem dados de previsão. Tente novamente mais tarde.";
                        }
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
