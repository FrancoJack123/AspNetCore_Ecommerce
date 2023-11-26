using _005_SistemaEcommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace _005_SistemaEcommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration configuracion;

        private string cad_cn = "";

        public LoginController(IConfiguration _config)
        {
            configuracion = _config;
            cad_cn = configuracion.GetConnectionString("cn1");
        }

        public string RegistroCliente(Cliente cl)
        {
            string mensaje = "";

            try
            {
                SqlHelper.ExecuteNonQuery(cad_cn, "RegistrarNuevoCliente", cl.nombre, cl.apellido, cl.correo, cl.contrasenia, cl.telefono);
                mensaje = "Su Cuenta ha sido creada con exito";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;
        }

        public Cliente InicioSesion(string correo, string contrasenia)
        {
            Cliente cl = null;
            try
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(cad_cn, "InicioSesion", correo, contrasenia); 
                while (reader.Read())
                {
                    cl = new Cliente()
                    {
                        clienteId = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        apellido = reader.GetString(2), 
                        correo = reader.GetString(3),
                        contrasenia = reader.GetString(4),
                        telefono = reader.GetString(5)
                    };
                }

            }
            catch(Exception e) 
            {
                cl = null;
            }

            return cl;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contrasenia)
        {
            Cliente cl = InicioSesion(correo, contrasenia);
            if(cl != null)
            {
                TempData["message"] = "Bienvenido: Usuario " + cl.nombre + " " + cl.apellido;
                string nombre = cl.nombre;
                string id = cl.clienteId.ToString();
                HttpContext.Session.SetString("idCliente", id);
                HttpContext.Session.SetString("Cliente", nombre);
                return RedirectToAction("Index", "Tienda");
            }
            else
            {
                TempData["message"] = "Correo o contraseña Incorrectos";
                return RedirectToAction("Login", "Login");
            }
            
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Cliente cl)
        {
            TempData["Message"] = RegistroCliente(cl);

            return RedirectToAction("Login");
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Tienda");
        }
    }
}
