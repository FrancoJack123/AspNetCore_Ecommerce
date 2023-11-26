namespace _005_SistemaEcommerce.Models
{
    public class Cliente
    {
        public int clienteId { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? correo { get; set; }
        public string? contrasenia { get; set; }
        public string? telefono { get; set; }
    }
}
