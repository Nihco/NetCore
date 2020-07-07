using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Propins.Models;
using RestSharp;

namespace Propins.Controllers
{
    public class HomeController : Controller
    {
        private UsuarioModel usuarioModel = new UsuarioModel();

        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = GetDataJson();

            ViewBag.usuarios = usuarios;
            return View();
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostData(string id, string nombre, string apellido, string profesion, string email)
        {
            var client = new RestClient("http://arsene.azurewebsites.net/User");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            request.AddParameter("apellido", apellido);
            request.AddParameter("email", email);
            request.AddParameter("nombre", nombre);
            request.AddParameter("profesion", profesion);
            request.AddParameter("id", id);
            client.Execute(request);
            return RedirectToAction("Index");
        }


        public UsuarioModel GetDataById(string id)
        {
            var client = new RestClient("http://arsene.azurewebsites.net/User?id=" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            IRestResponse response = client.Execute(request);
            List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Content);
            UsuarioModel usuario = usuarios.First();
            return usuario;
        }


        public IActionResult Update(string id)
        {
            ViewBag.usuario = GetDataById(id);
            return View();
        }

        [HttpPost]
        public IActionResult ModificarData(string id, string nombre, string apellido, string profesion, string email)
        {
            UsuarioModel usuario = MapData(id, nombre, apellido, profesion, email);
            string objectJson = JsonConvert.SerializeObject(usuario);
            var client = new RestClient("http://arsene.azurewebsites.net/User/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            request.AddParameter("application/json", objectJson, ParameterType.RequestBody);
            client.Execute(request);
            return RedirectToAction("Index");
        }

        public UsuarioModel MapData(string id, string nombre, string apellido, string profesion, string email)
        {
            usuarioModel.id = id;
            usuarioModel.nombre = nombre;
            usuarioModel.apellido = apellido;
            usuarioModel.profesion = profesion;
            usuarioModel.email = email;
            return usuarioModel;
        }

        public IActionResult Eliminar(string id)
        {
            var client = new RestClient("http://arsene.azurewebsites.net/User/" + id);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            client.Execute(request);
            return RedirectToAction("Index");
        }

        public List<UsuarioModel> GetDataJson()
        {
            var client = new RestClient("http://arsene.azurewebsites.net/User");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", "ARRAffinity=b7654489a1cd63362ae25dd5c2faf0e50d856890f952add19099d48cf360ebc6");
            IRestResponse response = client.Execute(request);

            List<UsuarioModel> usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(response.Content);
            return usuarios;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}