using Ejercicio2_4PMO2.Models;
using Ejercicio2_4PMO2.Views;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ejercicio2_4PMO2
{
    public partial class MainPage : ContentPage
    {
        String pathVideo = "";
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btngrabar_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No hay Camara", "No hay camara Disponible.", "OK"); return;
            }
            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                Name = "VideoProgramacion.mp4",
                Directory = "VideosTarea"
            });
            if (file == null)
                return;
            await DisplayAlert("Video Guardado", "Su video se ha guardado exitosamente en su telefono", "OK");
            videoV.Source = file.Path;
            pathVideo = file.Path;
        }

        private async void btnsalvar_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(pathVideo))
            {
                var respuesta = await App.BaseDatos.guardaVideos(new Video { path = pathVideo });
                if (respuesta == 1)
                {
                    await DisplayAlert("Guardado Exitoso", "Se ha enviado el video a SQLite", "OK");
                    videoV.Source = null;
                    pathVideo = "";
                }
                else
                {
                    await DisplayAlert("Error", "Algo fallo al guardar en SQLite", "OK");
                }
            }
            else
            {
                await DisplayAlert("Video Inexistente", "Por favor graba un video", "OK");
            }
        }

        private async void btnlista_Clicked(object sender, EventArgs e)
        {
            var listado = await App.BaseDatos.GetListVid();//LLENAR LA LISTAA
            if (listado != null)
            {
                if (listado.Count() > 0)
                {
                    await Navigation.PushAsync(new ListVideos());
                }
                else
                {
                    await DisplayAlert("Listado Vacio", "No hay videos en lista", "OK");
                }
            }
            else
            {
                await DisplayAlert("Listado Vacio", "No existen videos", "OK");
            }
        }
    }
}
