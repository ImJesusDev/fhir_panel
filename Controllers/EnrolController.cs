﻿
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
// Database connection
using AspStudio.Models;
using AspStudio.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net;


//using System.Web;
//using System.Web.Http;
using System.Net.Http.Headers;

// Json
using System.Text.Json;
using System.Text.Json.Serialization;

// Database connection



using System.IO;

using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspStudio.Controllers
{

    public class EnrolController : Controller
    {
        // Inyecta la instancia de MQTTnet (mqttClient) que fue creada como
        // servicio inyectable en StartUp.cs
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext dbContext;


        public EnrolController(ILogger<AccountController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;

        }


        [HttpGet]
        public ViewResult Index()
        {
            var dispositivos = dbContext.Empresas;

            List<dynamic> empresas = new List<dynamic>();
            dynamic empresa;

            try
            {
                foreach (var dispositivo in dispositivos)
                {
                    empresa = new ExpandoObject();
                    empresa.codigo = dispositivo.codigo;
                    empresa.descripcion = dispositivo.descripcion;
                    empresas.Add(empresa);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }
            
            System.Console.WriteLine(empresas);
            ViewBag.empresas = empresas;





            var regRegionales = dbContext.Regionales;

            List<dynamic> regionales = new List<dynamic>();
            dynamic regional;

            

            try
            {
                foreach (var registro in regRegionales)
                {
                    regional = new ExpandoObject();
                    regional.Codigo = registro.Codigo;
                    regional.Descripcion = registro.Descripcion;
                    regionales.Add(regional);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(regionales);
            ViewBag.regionales = regionales;




            
            var regInstalaciones = dbContext.Instalaciones;

            List<dynamic> instalaciones = new List<dynamic>();
            dynamic instalacion;

            try
            {
                
                    instalacion = new ExpandoObject();
                    instalacion.Codigo = 0;
                    instalacion.Descripcion = "";
                    instalaciones.Add(instalacion);
                
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(instalaciones);
            ViewBag.instalaciones = instalaciones;
            






            var texto = dbContext.TextosEnrolam;
            

            List<dynamic> textoEnr = new List<dynamic>();
            List<dynamic> textoEnr1 = new List<dynamic>();
            dynamic textos;

            try
            {
                foreach (var regtexto in texto)
                {
                    textos = new ExpandoObject();
                    textos.id = regtexto.id;
                    textos.Texto = regtexto.Texto;
                    textos.Fecha = regtexto.Fecha;
                    textos.Tipo = regtexto.Tipo;
                    textos.Version = regtexto.Version;
                    textos.Pregunta = regtexto.Pregunta;
                    textoEnr.Add(textos);
                }
                /*
                var results = from x in textoEnr
                              group new { x.Tipo } by x.Tipo;

                foreach (var xTipo in results)
                {

                    var txts = from s in dbContext.TextosEnrolam select s;

                    textos = textoEnr.Where(s => s.Tipo.Contains(xTipo));

                    foreach (var tipoVer in textos)
                    {



                    }

                }
                */
                //textoEnr1 = (List<dynamic>)textoEnr.OrderBy(x => x.Tipo);

                /*
                var results = from x in textoEnr
                              group new { x.Texto, x.Version } by x.Tipo;*/


            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(textoEnr);
            ViewBag.textoEnr = textoEnr;

            return View();
        }

        [HttpPost]
        [HttpGet]


        //[Route("api/[controller]")]

        











            public Object CreateEnrol([FromBody] EnrolData mensaje)
        {
            System.Console.WriteLine(mensaje);

            try
            {

                Models.EnrolData empleado = new Models.EnrolData();

                empleado.idlenel = mensaje.idlenel;
                empleado.firstName = mensaje.firstName;
                empleado.lastName = mensaje.lastName;
                empleado.documento = mensaje.documento;
                empleado.empresa = mensaje.empresa;
                empleado.Regional = mensaje.Regional;
                empleado.Instalacion = mensaje.Instalacion;
                empleado.Ciudad = mensaje.Ciudad;
                empleado.SSNO = "";
                empleado.IdStatus = "";
                empleado.Status = "";
                empleado.created = DateTime.Now;

                dbContext.EnrolDatas.Add(empleado);
                dbContext.SaveChanges();

                return Json(new { success = true });

            }

            catch (Exception ex)
            {
                //throw ex;
                return Json(new { success = false });

            }



        }





        public ActionResult IngresoEmployee(string idlenel, string name, string lastname, string documento, string empresa, Int16 regional, Byte instalacion, string Ciudad, string EMail)
        {



                Models.EnrolData empleado = new Models.EnrolData();

            empleado.idlenel = idlenel;
            empleado.firstName = name;
            empleado.lastName = lastname;
            empleado.documento = documento;
            empleado.empresa = empresa;
            empleado.Regional = regional;
            empleado.Instalacion = instalacion;
            empleado.Ciudad = Ciudad;
            empleado.SSNO = "";
            empleado.IdStatus = "";
            empleado.Status = "";
            empleado.created = DateTime.Now;

            var mensaje= CreateEnrol(empleado);

            //CreateEnrol(empleado);

            //dbContext.EnrolDatas.Add(empleado);
            //dbContext.SaveChanges();

                return RedirectToAction("Logout", "Account");

           


        }










        protected string ExportToImage(string JsonMsg)
        {
            // Convertir el string JSON a un objeto
            var mensaje = JsonConvert.DeserializeObject<dynamic>(JsonMsg);


            // extraer la informacion en formato base64 de la imagen (incluidas las cabeceras)
            string base64 = mensaje.datas.imageFile.ToString();

            // Crea un timestamp
            DateTime localDate = DateTime.Now;

            // Separa la cabecera de los datos de la imagen
            var image64 = base64.Substring(base64.LastIndexOf(',') + 1);

            //  Convierte la cadena base64 en un arreglo de bytes
            byte[] bytes = Convert.FromBase64String(image64);

            // Obtiene la fecha del mensaje (Esto es para el mensaje MQTT)
            var fecha = mensaje.datas.time.ToString();
            fecha.Replace("/", "_");
            System.Console.WriteLine(fecha);

            // Define el nombre del archivo a guardar (Nombre de la persona + id dispositivo + fecha)
            var nombre = (mensaje.datas.name != "") ? mensaje.datas.name : "Desconocido";
            var imageName = mensaje.device_id + " " + nombre + " " + mensaje.datas.temperature + '_' + fecha + ".jpg";
            System.Console.WriteLine(imageName);

            // Define la ruta del directorio y de la imagen
            var folderPath = "wwwroot/Registers/";
            var imagePath = folderPath + imageName;

            // Guarda la imagen en el sistema de archivos
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                try
                {
                    image.Save(imagePath, ImageFormat.Jpeg);  // Or Png
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine("Error saving " + imagePath + " in filesystem" + e.Message + e.StackTrace);
                }

            }

            return (imagePath);
        }

        [HttpGet]

        public JsonResult GetPersonDetails(int Id)
        {
            var InstalacQuery =
            from Instalac in dbContext.Instalaciones
            where Instalac.Regional == Id
            select new { Instalac.Codigo, Instalac.Descripcion };

            
            return Json(InstalacQuery);
        }

        public ActionResult refreshInstal(int Id)
        {
            /*
            var InstalacQuery =
            from Instalac in dbContext.Instalaciones
            where Instalac.Regional == Id
            select Instalac;*/

            var InstalacQuery =
            from Instalac in dbContext.Instalaciones
            where Instalac.Regional == Id
            select new { Instalac.Codigo, Instalac.Descripcion };

            ViewBag.instalaciones = InstalacQuery;
            /*
            var regInstalaciones = dbContext.Instalaciones;

            List<dynamic> instalaciones = new List<dynamic>();
            dynamic instalacion;

            try
            {
                foreach (var registro in regInstalaciones)
                {

                    if (registro.Regional == Id)
                    { 
                        instalacion = new ExpandoObject();
                        instalacion.Codigo = registro.Codigo;
                        instalacion.Descripcion = registro.Descripcion;
                        instalaciones.Add(instalacion);
                    }
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(instalaciones);
            ViewBag.instalaciones = instalaciones;
            */




            return View();
        }

        /*
         fecha de autorización,
hora de autorización, 
versión de texto presentada, 
responsable de la caprtura de datos,
ubicación*/



    }
}