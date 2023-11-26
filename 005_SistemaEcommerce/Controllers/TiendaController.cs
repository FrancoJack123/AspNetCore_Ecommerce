using _005_SistemaEcommerce.Dao;
using _005_SistemaEcommerce.Models;
using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace _005_SistemaEcommerce.Controllers
{
    public class TiendaController : Controller
    {
        private readonly IConfiguration configuracion;

        private string cad_cn = "";

        public TiendaController(IConfiguration _config)
        {
            configuracion = _config;
            cad_cn = configuracion.GetConnectionString("cn1");
        }

        public List<Producto> ListarProductoFiltros(int idcategoria, string nombreprod)
        {
            List<Producto> list = new List<Producto>();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarProductosConCategorias", idcategoria, nombreprod);
            while (reader.Read())
            {
                Producto producto = new Producto()
                {
                    productoId = reader.GetInt32(0),
                    nombreProduc = reader.GetString(1),
                    descripcionProdu = reader.GetString(2),
                    precio = reader.GetDecimal(3),
                    stock = reader.GetInt32(4),
                    foto = reader.GetString(5),
                    categoriaId = reader.GetInt32(6),
                    categoriaName = reader.GetString(7)
                };
                list.Add(producto);
            }

            return list;
        }

        public Producto BuscarProducto(int idProducto)
        {
            Producto producto = new Producto();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "BuscarProducto", idProducto);
            while (reader.Read())
            {
                producto = new Producto()
                {
                    productoId = reader.GetInt32(0),
                    nombreProduc = reader.GetString(1),
                    descripcionProdu = reader.GetString(2),
                    precio = reader.GetDecimal(3),
                    stock = reader.GetInt32(4),
                    foto = reader.GetString(5),
                    categoriaId = reader.GetInt32(6),
                    categoriaName = reader.GetString(7)
                };
            }
            return producto;
        }

        public List<Categoria> ListarCategoria()
        {
            List<Categoria> list = new List<Categoria>();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarCategorias");
            while (reader.Read())
            {
                Categoria categoria = new Categoria()
                {
                    categoriaId = reader.GetInt32(0),
                    categoriaName = reader.GetString(1)
                };
                list.Add(categoria);
            }
            return list;
        }

        public void AgregarCarrito(int productoId, int clienteId, int cantidad)
        {
            SqlHelper.ExecuteNonQuery(cad_cn, "AgregarCarrito", productoId, clienteId, cantidad);
        }

        public void DeleteCarrito(int carritoId)
        {
            SqlHelper.ExecuteNonQuery(cad_cn, "EliminarCarrito", carritoId);
        }

        public string AgregarPedido(Pedido ped)
        {
            string idPedido = "";
            using (SqlConnection con = new SqlConnection(cad_cn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("RegistrarPedido", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClienteID", ped.clienteId);
                cmd.Parameters.AddWithValue("@Nombre", ped.nombre);
                cmd.Parameters.AddWithValue("@Apellido", ped.apellido);
                cmd.Parameters.AddWithValue("@Telefono", ped.telefono);
                cmd.Parameters.AddWithValue("@Direccion", ped.direccion);
                cmd.Parameters.AddWithValue("@CodigoPostal", ped.codigoPostal);
                cmd.Parameters.AddWithValue("@PrecioTotal", ped.precioTottal);
                idPedido = cmd.ExecuteScalar().ToString();
            }
            return idPedido;
        }

        public void AgregarDetallePedido(DetallePedido detaPed)
        {
            SqlHelper.ExecuteNonQuery(cad_cn, "InsertarDetallePedido", detaPed.PedidoId, detaPed.ProductoId, detaPed.cantidad, detaPed.precioTotal, detaPed.precioUnitario);
        }


        public string CantidadCarrito(int Idcliente)
        {
            string cantidad = "";
            using (SqlConnection con = new SqlConnection(cad_cn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("ObtenerCantidadCarrito", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClienteID", Idcliente);
                cantidad = cmd.ExecuteScalar().ToString();
            }
            return cantidad;
        }

        public List<Carrito> ListarCarrito(int clienteId)
        {
            List<Carrito> Listado = new List<Carrito>();
            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarProductosEnCarrito", clienteId);
            while (reader.Read())
            {
                Carrito carrito = new Carrito()
                {
                    carritoId = reader.GetInt32(0),
                    productoId = reader.GetInt32(1),
                    clienteId = reader.GetInt32(2),
                    cantidad = reader.GetInt32(3),
                    productoName = reader.GetString(4),
                    precio = reader.GetDecimal(5),
                    stock = reader.GetInt32(6)

                };
                Listado.Add(carrito);
            }

            return Listado;
        }

        public void LimpiarCarrito(int idCliente)
        {
            SqlHelper.ExecuteNonQuery(cad_cn, "EliminarElementosEnCarritoPorClienteID", idCliente);
        }

        public List<Pedido> ListarPedido(int clienteId)
        {
            List<Pedido> lista = new List<Pedido>();
            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarPedidosPorClienteID", clienteId);
            while (reader.Read())
            {
                Pedido ped = new Pedido()
                {
                    pedidoId = reader.GetInt32(0),
                    fecha = reader.GetDateTime(1),
                    precioTottal = reader.GetDecimal(2)
                };
                lista.Add(ped);
            }
            return lista;
        }

        public List<DetallePedido> ListarDetallePedido(int pedidoId)
        {
            List<DetallePedido> lista = new List<DetallePedido>();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarDetallesPedidoConProductoInfo", pedidoId);
            while (reader.Read())
            {
                DetallePedido detaPed = new DetallePedido()
                {
                    cantidad = reader.GetInt32(0),
                    nombreProdu = reader.GetString(1),
                    precioUnitario = reader.GetDecimal(2),
                    fotoProdu = reader.GetString(3),
                    precioTotal = reader.GetDecimal(4)
                };
                lista.Add(detaPed);
            }

            return lista;
        }


        public IActionResult Index(int idcategoria = 1, string nombre = "", int nro_pag = 0)
        {
            var listado = ListarProductoFiltros(idcategoria, nombre);
            int cant_total = listado.Count;
            int filas_pag = 4;

            int cant_paginas = 0;

            if (cant_total % filas_pag == 0)
                cant_paginas = cant_total / filas_pag;
            else
                cant_paginas = cant_total / filas_pag + 1;

            ViewBag.CANT_PAGINAS = cant_paginas;
            ViewBag.NRO_PAG = nro_pag;
            ViewBag.cantTotal = cant_total;

            var categoria = ListarCategoria();
            ViewBag.ListadoCategoria = categoria;
            ViewBag.NombreProducto = nombre;
            ViewBag.IdCategoria = idcategoria;

            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente") == null ? "0" : HttpContext.Session.GetString("idCliente"));
            string cantidad = CantidadCarrito(idCliente);
            HttpContext.Session.SetString("cantidad", cantidad);

            return View(listado.Skip(nro_pag * filas_pag).Take(filas_pag));
        }

        public IActionResult DetalleProducto(int id)
        {
            Producto producto = BuscarProducto(id);
            return View(producto);
        }

        [HttpGet]
        public IActionResult AgregarCarrito(int productoId, int cantidad)
        {
            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente"));
            AgregarCarrito(productoId, idCliente, cantidad);
            TempData["message"] = "El producto fue agregado con exito";
            return RedirectToAction("Index", "Tienda");
        }

        [HttpGet]
        public IActionResult EliminarCarrito(int idCarrito)
        {
            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente"));
            DeleteCarrito(idCarrito);
            return RedirectToAction("Carrito", "Tienda", new { id = idCliente });
        }

        public IActionResult Carrito()
        {
            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente") == null ? "0" : HttpContext.Session.GetString("idCliente"));
            if (idCliente != 0)
            {
                var listar = ListarCarrito(idCliente);

                string cantidad = CantidadCarrito(idCliente);
                HttpContext.Session.SetString("cantidad", cantidad);

                return View(listar);
            }
            else
            {
                TempData["message"] = "Debe Iniciar Sesion";
                return RedirectToAction("Index", "Tienda");
            }

        }

        [HttpPost]
        public IActionResult Carrito(Pedido ped)
        {
            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente"));
            ped.clienteId = idCliente;

            int idPedido = Int32.Parse(AgregarPedido(ped));

            List<DetallePedido> lista = new List<DetallePedido>();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarProductosEnCarrito", idCliente);
            while (reader.Read())
            {
                DetallePedido detaPed = new DetallePedido()
                {
                    PedidoId = idPedido,
                    ProductoId = reader.GetInt32(1),
                    cantidad = reader.GetInt32(3),
                    precioUnitario = reader.GetDecimal(5),
                    precioTotal = reader.GetInt32(3) * reader.GetDecimal(5)

                };
                lista.Add(detaPed);
            }

            for (int i = 0; i < lista.Count(); i++)
            {
                var detalle = lista[i];
                AgregarDetallePedido(detalle);
            }

            LimpiarCarrito(idCliente);

            TempData["message"] = "Su compra se ha realizado con Exito";

            return RedirectToAction("Index", "Tienda");
        }

        public IActionResult Pedido()
        {
            int idCliente = Int32.Parse(HttpContext.Session.GetString("idCliente") == null ? "0" : HttpContext.Session.GetString("idCliente"));
            if (idCliente != 0)
            {
                var lista = ListarPedido(idCliente);
                return View(lista);
            }
            else
            {
                TempData["message"] = "Debe Iniciar Sesion";
                return RedirectToAction("Index", "Tienda");
            }
        }

        public IActionResult DetallePedido(int pedidoId)
        {
            var lista = ListarDetallePedido(pedidoId);
            return View(lista);
        }

        [HttpPost]
        public IActionResult GenerarPdf(int pedidoId)
        {
            IEnumerable<DetallePedido> listado = ListarDetallePedido(pedidoId);
            string time = DateTime.Now.ToShortTimeString();
            string date = DateTime.UtcNow.ToShortDateString();

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    page.Header().ShowOnce().Row(row =>
                    {
                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().AlignRight().Text("Asp.Net Core").Bold().FontSize(14);
                            col.Item().AlignRight().Text($"Fecha: {date}").FontSize(9);
                            col.Item().AlignRight().Text($"Hora: {time}").FontSize(9);
                        });


                    });

                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().AlignCenter().PaddingBottom(10).Column(col =>
                        {
                            col.Item().AlignCenter().Text("Detalle Pedido").Bold().FontSize(25);
                        });

                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().PaddingVertical(6).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(3);

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#424242").Border(0.8f).BorderColor("#424242").Padding(2).AlignCenter().Text("Cantidad").FontColor("#fff");
                                header.Cell().Background("#424242").Border(0.8f).BorderColor("#424242").Padding(2).AlignCenter().Text("Nompbre del Producto").FontColor("#fff");
                                header.Cell().Background("#424242").Border(0.8f).BorderColor("#424242").Padding(2).AlignCenter().Text("Precio Unitario").FontColor("#fff");
                                header.Cell().Background("#424242").Border(0.8f).BorderColor("#424242").Padding(2).AlignCenter().Text("Precio Total").FontColor("#fff");


                            });

                            foreach (var item in listado)
                            {
                                tabla.Cell().Border(0.5f).BorderColor("#424242").Padding(2).AlignCenter().Text(item.cantidad.ToString()).FontSize(10);
                                tabla.Cell().Border(0.5f).BorderColor("#424242").Padding(2).AlignCenter().Text(item.nombreProdu.ToString()).FontSize(10);
                                tabla.Cell().Border(0.5f).BorderColor("#424242").Padding(2).AlignCenter().Text(item.precioUnitario.ToString()).FontSize(10);
                                tabla.Cell().Border(0.5f).BorderColor("#424242").Padding(2).AlignCenter().Text(item.precioTotal.ToString()).FontSize(10);
                            }

                        });

                    });


                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });

                });
            }).GeneratePdf();

            MemoryStream stream = new MemoryStream(data);

            Response.Headers["Content-Disposition"] = "inline; filename=Pedido.pdf";

            return new FileStreamResult(stream, "application/pdf");

        }
    }
}
