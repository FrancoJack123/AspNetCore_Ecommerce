namespace _005_SistemaEcommerce.Models
{
    public class Producto
    {
        public int productoId { get; set; }
        public string nombreProduc { get; set; }
        public string descripcionProdu { get; set; }
        public string foto { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public int categoriaId { get; set; }
        public string categoriaName { get; set; }
    }
}
