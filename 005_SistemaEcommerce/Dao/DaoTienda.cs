using _005_SistemaEcommerce.Models;
using System.Data.SqlClient;

namespace _005_SistemaEcommerce.Dao
{
    public class DaoTienda
    {
        private readonly IConfiguration configuracion;

        private string cad_cn = "";

        public DaoTienda(IConfiguration _config)
        {
            configuracion = _config;
            cad_cn = configuracion.GetConnectionString("cn1");
        }

        public List<Producto> ListarProducto()
        {
            List<Producto> list = new List<Producto>();

            SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "ListarProductosConCategorias");
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
    }
}
