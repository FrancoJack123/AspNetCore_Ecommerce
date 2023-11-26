namespace _005_SistemaEcommerce.Models
{
    public class DetallePedido
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal precioTotal { get; set; }
        public string nombreProdu { get; set; }
        public string fotoProdu { get; set; }
    }
}
