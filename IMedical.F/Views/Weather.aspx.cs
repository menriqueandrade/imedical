using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMedicalB.Model;
using Newtonsoft.Json;
using System.Net;
using System.Web.UI;
using IMedicalB.Dto;
using System.Linq;
using IMedical.F.Model;
using System.Configuration;


namespace IMedical.F.Views
{
    public partial class Weather : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(async () =>
                {
                    var cities = await GetCitiesFromApi();
                    CityGrid.DataSource = cities;
                    CityGrid.DataBind();
                }));
            }
        }

        private async Task<List<CityInfo>> GetCitiesFromApi()
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    string url = ConfigurationManager.AppSettings["ApiUrl_Cities"];
                    var json = await client.DownloadStringTaskAsync(url);
                    return JsonConvert.DeserializeObject<List<CityInfo>>(json) ?? new List<CityInfo>();
                }
                catch (WebException)
                {
                    return new List<CityInfo>();
                }
            }
        }


        private async Task<List<CityHistoryDto>> GetCityHistoryFromApi()
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;

                try
                {
                    string url = ConfigurationManager.AppSettings["ApiUrl_Consult"];
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string body = "{}"; 

                    var json = await client.UploadStringTaskAsync(url, "POST", body);

                    
                    var response = JsonConvert.DeserializeObject<CityHistoryResponse>(json);
                    return response?.History ?? new List<CityHistoryDto>();
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<CityHistoryDto>(); 
                }
            }
        }

     
        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            string ciudad = HiddenCity.Value;
            string condicion = HiddenCondition.Value;

            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    string formato = ConfigurationManager.AppSettings["ApiUrl_UpdateCity"];
                    string url = string.Format(formato, Uri.EscapeDataString(ciudad));
                    var json = await client.DownloadStringTaskAsync(url);

                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected async void btnVerHistorial_Click(object sender, EventArgs e)
        {
            var historial = await GetCityHistoryFromApi();

            if (historial.Any())
            {
                litHistorial.Text = "<ul class='list-group'>";
                foreach (var item in historial)
                {
                    litHistorial.Text += $"<li class='list-group-item'><strong>{item.City}</strong>: {item.Info}</li>";
                }
                litHistorial.Text += "</ul>";
            }
            else
            {
                litHistorial.Text = "<div class='alert alert-info'>No hay historial disponible.</div>";
            }

            // Mostrar modal con JavaScript
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarHistorial", @"
    window.addEventListener('DOMContentLoaded', function() {
        var modal = new bootstrap.Modal(document.getElementById('historyModal'));
        modal.show();
    });
", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            string ciudadBuscada = txtBuscarCiudad.Text.Trim();

            if (!string.IsNullOrEmpty(ciudadBuscada))
            {
                RegisterAsyncTask(new PageAsyncTask(async () =>
                {
                    var ciudades = await SearchCityApi(ciudadBuscada);
                    CityGrid.DataSource = ciudades;
                    CityGrid.DataBind();
                }));
            }
            else
            {

                RegisterAsyncTask(new PageAsyncTask(async () =>
                {
                    var cities = await GetCitiesFromApi();
                    CityGrid.DataSource = cities;
                    CityGrid.DataBind();
                }));
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarCiudad.Text = string.Empty;
            
            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                var cities = await GetCitiesFromApi();
                CityGrid.DataSource = cities;
                CityGrid.DataBind();
            }));
        }

        private async Task<List<CityInfo>> SearchCityApi(string name)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    
                    string ciudadCodificada = Uri.EscapeDataString(name);
                    string formato = ConfigurationManager.AppSettings["ApiUrl_SearchCity"];
                    string url = string.Format(formato, ciudadCodificada);

                    var json = await client.DownloadStringTaskAsync(url);
                    var city = JsonConvert.DeserializeObject<CityInfo>(json);

                    
                    return city != null ? new List<CityInfo> { city } : new List<CityInfo>();
                }
                catch (WebException ex)
                {
                    // Manejo de errores específicos
                    if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Ciudad no encontrada
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "alert('La ciudad especificada no fue encontrada.');", true);
                    }
                    else
                    {
                        // Otro tipo de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "alert('Error al conectar con el servicio de búsqueda.');", true);
                    }

                    return new List<CityInfo>();
                }
            }
        }
    }

    }