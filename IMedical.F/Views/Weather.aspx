<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Weather.aspx.cs" Inherits="IMedical.F.Views.Weather" Async="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>IMedical Tec</title>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .table-modern {
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

            .table-modern th {
                background-color: #f8f9fa;
                font-weight: 600;
            }

        .pagination li {
            margin: 0 5px;
        }

        .pagination a {
            border-radius: 5px !important;
        }

        .hidden {
            display: none;
        }

        .btn-news {
            padding: 0.25rem 0.5rem;
            font-size: 0.8rem;
        }
    </style>
</head>
<body style="background-color: #f5f5f5; padding: 20px;">
    <form id="form1" runat="server">

        <div style="max-width: 1000px; margin: 0 auto;">
            <h2 style="color: #333; margin-bottom: 20px;">🌤️ Clima por Ciudad</h2>
            <asp:Button ID="btnVerHistorial" runat="server" Text="🕘 Ver Historial" CssClass="btn btn-secondary mb-3" OnClick="btnVerHistorial_Click" />
            <div class="input-group mb-3">
                <asp:TextBox ID="txtBuscarCiudad" runat="server" CssClass="form-control" placeholder="Buscar ciudad..." />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
            </div>
            <asp:GridView ID="CityGrid" runat="server" AutoGenerateColumns="False"
                CssClass="table table-modern table-hover">
                <Columns>
                    <asp:BoundField DataField="City" HeaderText="Ciudad" />
                    <asp:BoundField DataField="Current_Weather.Temp" HeaderText="Temp. (°C)" />
                    <asp:BoundField DataField="Current_Weather.Humidity" HeaderText="Humedad (%)" />
                    <asp:BoundField DataField="Current_Weather.Country" HeaderText="País" />
                    <asp:BoundField DataField="Current_Weather.Condition" HeaderText="Condición" />
                    <asp:TemplateField HeaderText="Noticias">
                        <ItemTemplate>
                            <button type="button" class="btn btn-info btn-sm btn-news"
                                onclick='showNewsModal(
            <%# Newtonsoft.Json.JsonConvert.SerializeObject(new { Name = Eval("City") }) %>,
            <%# Newtonsoft.Json.JsonConvert.SerializeObject(((List<IMedicalB.Model.NewsItem>)Eval("News"))?.FirstOrDefault()) %>,
            "<%# Eval("Current_Weather.Condition") %>"
        )'>
                                Ver Noticia
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>


            <div class="row mt-3">
                <div class="col">
                    <nav aria-label="Page navigation">
                        <ul class="pagination justify-content-center" id="pagination"></ul>
                    </nav>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="HiddenCity" runat="server" />
        <asp:HiddenField ID="HiddenCondition" runat="server" />

        <!-- Modal para Noticias -->
        <div class="modal fade" id="newsModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalNewsTitle"></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-4" id="newsImageContainer">
                                <img id="modalNewsImage" src="" class="img-fluid rounded mb-3" style="max-height: 200px; object-fit: cover;" onerror="this.style.display='none'">
                            </div>
                            <div class="col-md-8">
                                <p><strong>Autor:</strong> <span id="modalNewsAuthor"></span></p>
                                <p><strong>Fecha de publicación:</strong> <span id="modalNewsDate"></span></p>
                                <p><strong>Descripción:</strong> <span id="modalNewsDesc"></span></p>
                                <p><strong>Enlace:</strong> <a id="modalNewsUrl" href="#" target="_blank">Ver noticia completa</a></p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        <asp:Button ID="btnActualizar" runat="server" Text="Registrar" CssClass="btn btn-primary" OnClick="btnActualizar_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal para Hostoria -->
        <div class="modal fade" id="historyModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-scrollable">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">📜 Historial de Ciudades</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">
                        <asp:Literal ID="litHistorial" runat="server"></asp:Literal>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </form>


    <script>
        // Paginación
        document.addEventListener('DOMContentLoaded', function () {
            const table = document.querySelector('#<%= CityGrid.ClientID %>');
            const rows = table.querySelectorAll('tbody tr');
            const itemsPerPage = 10;
            const pageCount = Math.ceil(rows.length / itemsPerPage);
            const pagination = document.getElementById('pagination');

            rows.forEach((row, index) => {
                row.classList.toggle('hidden', index >= itemsPerPage);
            });

            if (pageCount > 1) {
                for (let i = 1; i <= pageCount; i++) {
                    const li = document.createElement('li');
                    li.className = 'page-item';
                    if (i === 1) li.classList.add('active');

                    const a = document.createElement('a');
                    a.className = 'page-link';
                    a.href = '#';
                    a.textContent = i;

                    a.addEventListener('click', function (e) {
                        e.preventDefault();
                        pagination.querySelectorAll('.page-item').forEach(item => {
                            item.classList.remove('active');
                        });
                        this.parentNode.classList.add('active');

                        const start = (i - 1) * itemsPerPage;
                        const end = start + itemsPerPage;

                        rows.forEach((row, index) => {
                            row.classList.toggle('hidden', index < start || index >= end);
                        });
                    });

                    li.appendChild(a);
                    pagination.appendChild(li);
                }
            }
        });

        function showNewsModal(cityObj, newsItemJson, condition) {
            // Parsear los objetos
            const city = typeof cityObj === 'string' ? JSON.parse(cityObj) : cityObj;
            const newsItem = typeof newsItemJson === 'string' ? JSON.parse(newsItemJson) : newsItemJson;

            // Establecer valores del modal
            document.getElementById('modalNewsTitle').textContent = newsItem?.Title || "Noticia de " + city.Name;
            document.getElementById('modalNewsAuthor').textContent = newsItem?.Author || "Autor desconocido";
            document.getElementById('modalNewsDesc').textContent = newsItem?.Description || "No hay descripción disponible";

            // Manejar la imagen
            const newsImage = document.getElementById('modalNewsImage');
            if (newsItem?.UrlToImage) {
                newsImage.src = newsItem.UrlToImage;
                newsImage.style.display = 'block';
                document.getElementById('newsImageContainer').style.display = 'block';
            } else {
                newsImage.style.display = 'none';
                document.getElementById('newsImageContainer').style.display = 'none';
            }

            // Formatear fecha
            if (newsItem?.PublishedAt) {
                const date = new Date(newsItem.PublishedAt);
                document.getElementById('modalNewsDate').textContent = date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
            } else {
                document.getElementById('modalNewsDate').textContent = "Fecha no disponible";
            }

            // Manejar enlace
            const newsUrl = document.getElementById('modalNewsUrl');
            if (newsItem?.Url) {
                newsUrl.href = newsItem.Url;
                newsUrl.style.display = 'inline-block';
            } else {
                newsUrl.style.display = 'none';
            }

            // Guardar valores en campos ocultos
            document.getElementById('<%= HiddenCity.ClientID %>').value = city.Name;
            document.getElementById('<%= HiddenCondition.ClientID %>').value = condition;

            // Mostrar modal
            const modal = new bootstrap.Modal(document.getElementById('newsModal'));
            modal.show();
        }
    </script>
</body>
</html>
