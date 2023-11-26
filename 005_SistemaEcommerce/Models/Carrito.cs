namespace _005_SistemaEcommerce.Models
{
    public class Carrito
    {
        public int carritoId { get; set; }
        public int clienteId { get; set; }
        public int productoId { get; set; }
        public string productoName { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
    }
}
