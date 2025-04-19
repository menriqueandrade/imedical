using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMedicalB.Model;
using Newtonsoft.Json;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace IMedical.F.Views
{
    public partial class Weather : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(async () => {
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
                    string url = "https://localhost:7206/api/Weather/cities";
                    var json = await client.DownloadStringTaskAsync(url);
                    return JsonConvert.DeserializeObject<List<CityInfo>>(json) ?? new List<CityInfo>();
                }
                catch (WebException)
                {
                    // Puedes agregar logging del error aquí
                    return new List<CityInfo>();
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string ciudad = HiddenCity.Value;
            string condicion = HiddenCondition.Value;

            // Ahora podés usar estos valores como necesites
            // Por ejemplo:
            Console.WriteLine($"Actualizando ciudad: {ciudad}, condición: {condicion}");
        }

    }
}