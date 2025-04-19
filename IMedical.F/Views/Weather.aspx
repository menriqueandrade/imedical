<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Weather.aspx.cs" Inherits="IMedical.F.Views.Weather" Async="true" %>

<!DOCTYPE html>
<html>
<head>
    <title>Clima por Ciudad</title>
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
    onclick='showNewsModal("<%# Eval("City") %>", "<%# ((List<IMedicalB.Model.NewsItem>)Eval("News"))?.FirstOrDefault()?.Author %>", "<%# ((List<IMedicalB.Model.NewsItem>)Eval("News"))?.FirstOrDefault()?.Title %>", "<%# ((List<IMedicalB.Model.NewsItem>)Eval("News"))?.FirstOrDefault()?.Description %>", "<%# Eval("Current_Weather.Condition") %>")'>
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
        <div class="modal fade" id="newsModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalNewsTitle"></h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p><strong>Autor:</strong> <span id="modalNewsAuthor"></span></p>
                        <p><strong>Descripción:</strong> <span id="modalNewsDesc"></span></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        <asp:Button ID="Button1" runat="server" Text="🔄 Actualizar Datos" CssClass="btn btn-primary mb-3" OnClick="btnActualizar_Click" />

                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Modal para Noticias -->


    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        // Paginación (código existente)
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

        // Función para mostrar el modal de noticias
        function showNewsModal(city, author, title, description, condition) {
            document.getElementById('modalNewsTitle').textContent = title || "Noticia de " + city;
            document.getElementById('modalNewsAuthor').textContent = author || "Desconocido";
            document.getElementById('modalNewsDesc').textContent = description || "No hay descripción disponible";

            // Setear los valores ocultos
            document.getElementById('<%= HiddenCity.ClientID %>').value = city;
            document.getElementById('<%= HiddenCondition.ClientID %>').value = condition;

            const modal = new bootstrap.Modal(document.getElementById('newsModal'));
            modal.show();
        }
    </script>
</body>
</html>
