namespace _005_SistemaEcommerce.Models
{
    public class Pedido
    {
        public int pedidoId { get; set; }
        public int clienteId { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string codigoPostal { get; set; }
        public decimal precioTottal { get; set; }
        public DateTime fecha { get; set; }
    }
}
