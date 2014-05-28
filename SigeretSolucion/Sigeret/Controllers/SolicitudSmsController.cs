﻿using Sigeret.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace Sigeret.Controllers
{
    [AllowAnonymous]
    public class SolicitudSmsController : BaseController
    {
        public ActionResult prueba2()
        {
            string connetionString = null;
            SqlConnection cnn ;
            connetionString = "Data Source=68c7080b-6f16-4426-8740-a3380103fe1e.sqlserver.sequelizer.com;Initial Catalog=db68c7080b6f1644268740a3380103fe1e;User ID=wnopohjgnhykfkjt;Password=zm6vRN8NVNfnXuH6YSwtJcGUGWaoupff5iMScf4nG3X4khzkRbGCcdixMkW4NSsF;MultipleActiveResultSets=True";
            //Server=;Database=;User ID=;Password=;
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
               
               string queryString = "SELECT * FROM  [dbo].[UserProfile]";
               SqlCommand command = new SqlCommand(queryString, cnn);
               
             
               SqlDataReader reader = command.ExecuteReader();
               
               try
               {
                   while (reader.Read())
                   {

                       ViewBag.error = reader;
                   }
               }
               finally
               {
                   // Always call Close when done reading.
                   reader.Close();
               }





                cnn.Close();



            }
            catch (Exception ex)
            {
             ViewBag.error="Can not open connection ! "+ex.InnerException;
            }

            return View();
        }

        /*
        public ActionResult prueba(){        

            return View();
        }
        [HttpPost]
        public ActionResult prueba(string fecha,string horaInicio, string horaFin)
        {
            DateTime obj = new DateTime();
            obj = DateTime.Parse(fecha);

            fecha = obj.ToString("yyyy-MM-dd");

            var query = db.Database.SqlQuery<int>("EXEC EquiposDisponibles {0},{1},{2}", fecha, horaInicio, horaFin).ToList();

            return View();
        }
         */
        //
       // GET: /SolicitudSms/
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
   
        public ActionResult Index(string body, string From)
        {
            string opcion ="";             
            if (Session["opcion"] != null) { opcion =(String)Session["opcion"]; }

         


            string sender = "2766011354";


            string solicitud = "";
            string respuesta;

            //si la longitud del mensaje enviado es mayor a 2 entonces procedemos a sacar una subcadena para verificar si es una solicitud
            if (body.Length > 2)
            {
                solicitud = body.Substring(0, 2);
            }

            if (solicitud == "**")
            {
                //enviamos la solicitud al metodo ProcesarSolicitud
                respuesta = ProcesarSolicitud(body, "5088863180");

            }
            else if (opcion == "de")
            {
                respuesta= getCodigoEquipos(body);
                Session["opcion"] = "";

            }
            else
            {
                
                opcion = opcion + body;
                Session["opcion"] = opcion; 


                switch (opcion)
                {
                        
                    case "ayuda":
                        respuesta = "\n1 Nueva Solicitud\n2 Equipos\n3 NipSMS\n4 Salones";
                        Session["opcion"] = "";
                        break;

                    case "":
                        respuesta = "\n1 Nueva Solicitud\n2 Equipos\n3 NipSMS\n4 Salones";
                        break;

                    case "menu":
                        respuesta = "\n1 Nueva Solicitud\n2 Equipos\n3 NipSMS\n4 Salones";
                        Session["opcion"] = "";
                        break;
                    
                    case "1":
                        respuesta = "\n1 Nueva Solicitud\n2 Equipos\n3 NipSMS\n4 Salones";
                        break;


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    case "11":
                        respuesta = "\nMenu Solicitud\n1 Formato Nueva Solicitud\n2 Formato Fecha\n3 Fomato Hora\n4 Cancelar Solicitud";
                        break;
                                            
                    case "12":
                        respuesta = "\nEquipos\n1 Codigos Equipos\n2 Descripcion Equipo";
                        break;

                    case "13":
                        respuesta = "\nNipSms \nCodigo utilizado para confirmar la solicitud\n Esta disponible via web o personalmente.\n1 mas detalles";//Codigo para confirmar la solicitud que sera entregado al usuario
                        //al crear su cuenta y luego cada vez q haga una solicitud cuando pase a entregar el equipo se le entregara este codigo personal
                        
                        break;

                    case "14":
                        respuesta = "\n"+getSalones(null);
                        Session["opcion"] = "";
                        break;

                    case "121":
                        respuesta ="\n"+ getCodigoEquipos(null);//Funcion para devolver todos lo equipos disponibles por modelos
                        Session["opcion"] = "";
                        break;

                    case "122":
                        respuesta = "\nEnvie el Codigo Equipo: Ejmp:001";
                        Session["opcion"] = "de";
                        break;

                    case "131":
                        respuesta = "\nNipSMS\n Codigo de 4 digitos generado al momento de crear su cuenta.\nDebe proporcionarlo para una solicitud SMS.\nEj. 9999";
                        Session["opcion"] = "";
                        break;

                    case "111":
                        respuesta = "\nFormato Solicitud:\n *fecha*horaInicio*horaFin*NipSMS*IdSalon*CodigoEquipo1*cantidad*CodigoEquipo2*cantidad*";
                        Session["opcion"] = "";
                        break;

                    case "112":
                        respuesta = "\nFormato Fecha\nDia/Mes/año \nEjemplo 24/12/1999";
                        Session["opcion"] = "";
                        break;
                    
                    case "113":
                        respuesta = "\nFormato Hora\n24H Ejemplo \n07:00  \n20:00 \nhora fin mayor a la hora inicio";
                        Session["opcion"] = "";
                        break;

                    case "114":
                        respuesta = "\nCancelar Solicitud\n C*codigoSolicitud\nEjemplo C*2301 ";
                        Session["opcion"] = "";
                        break;
                    default:
                        respuesta = "\nNo se Reconoce la Instruccion\n 1 Menu Principal";
                        Session["opcion"] = "";
                        break;

                }
            }

            // var twilio = new TwilioRestClient("AC7329769855ac2319f51129e29352294c","30b5abfcedeec6ec14586780e880fc88");
           //  var sms = twilio.SendSmsMessage(sender,From,respuesta);

         //   return Content(sms.Sid);
           ViewBag.resp = respuesta+" Opcion="+opcion;
           ViewBag.leng = respuesta.Length;
           return View();
        }





        public string ProcesarSolicitud(string solicitud,string telefono)
        {
            try
            {
                //Validando que el usuario este registrado para proceder con la solicitud
                int codigoUsuario = 0;         

                if (db.Contactoes.SingleOrDefault(c => c.Descripcion == telefono)!=null)
                {
                    codigoUsuario = db.Contactoes.SingleOrDefault(c => c.Descripcion == telefono).IdUserProfile;
                    
                    //Validando que el usurario solo pueda Registrar 5 solucitudes maximas por sms cada semestre

                    var solicitudesSMS = from s in db.SolicitudSms
                                         join contacto in db.Contactoes on s.IdContacto equals contacto.Id
                                         join sol in db.Solicituds on s.IdSolicitud equals sol.Id

                                         where contacto.IdUserProfile == codigoUsuario && sol.Fecha >= EntityFunctions.AddMonths(sol.Fecha, -3)
                                         && sol.Fecha <= EntityFunctions.AddMonths(sol.Fecha, 3)

                                         select s.Id;

                    if (solicitudesSMS.Count() > 8)
                    {
                        return "Usted ha excedido el maximo de solicitud sms por semestre.";
                    }
                    
                }
                else
                {
                    return "Su numero No esta registrado como Contacto de algun usuario.\n Registrelo o cree una cuenta. ";
                }
                string fecha;
                string horaInicio;
                string horaFin;
                string equiposStr;
                int lugar = 0;
                string nipSMS = null;
                List<ModeloEquipo> modelosDisponibles = new List<ModeloEquipo>();
                List<Equipo> equiposDisponibles = new List<Equipo>();
                
                //almacena la nueva Solicitud
                Solicitud nuevaSolicitud = new Solicitud();
                //almacena la nueva solicitud sms
                SolicitudSm nuevaSolSms = new SolicitudSm();

                //Seleccionando los id de las Solicitudes anteriores para obtener la ultima
                var SolicitudsId = db.Solicituds.Select(s => s.Id).ToList();



                fecha = solicitud.Substring(2, 10);
                horaInicio = solicitud.Substring(13, 5);
                horaFin = solicitud.Substring(19, 5);
                nipSMS = solicitud.Substring(25,4);
                equiposStr = solicitud.Substring(30, solicitud.Length - 30);

                //validando que el nipsms enviado sea igual al que esta registrado y pertenece al usuario para confirmar la solicitud
                if (db.UsuarioNipSms.SingleOrDefault(n => n.IdUserProfile==codigoUsuario).Nip!=nipSMS)
                {
                    return "NipSMS invalido. \nFavor vuelva a intentarlo.\nPuede Solicitarlo via web.";
                }

               try
                {
                    nuevaSolicitud.Fecha = DateTime.Parse(fecha);
                    nuevaSolicitud.HoraInicio = TimeSpan.Parse(horaInicio);
                    nuevaSolicitud.HoraFin = TimeSpan.Parse(horaFin);
                    nuevaSolicitud.Descripcion = "Solicitud SMS";
                    nuevaSolicitud.IdUserProfile = codigoUsuario;
                    nuevaSolicitud.IdEstatusSolicitud = 3;
                }
                catch 
                {
                    return "Error en el formato de la hora o la fecha";
                }
      

                //Obteniendo los equipos disponibles a partir de la fecha horaInicio y horaFin de la solicitudSms
                equiposDisponibles = EquiposDisponibles(fecha, horaInicio, horaInicio);

                if (equiposDisponibles.Count == 0)
                {
                    return "No hay equipos disponibles para esta fecha";
                }


                List<Tuple<string, string>> equipos = new List<Tuple<string, string>>();

                int cont = 50;
                int indice = 0;
                int indice2 = 0;

                for (int i = 0; i < equiposStr.Length; i++)
                {
                    //sacando el id del salon para agregarlo a la solicitud
                    if (equiposStr.ElementAt(i) == '*' && cont == 50)
                    {
                        lugar =Int32.Parse( equiposStr.Substring(0, i ));
                        equiposStr = equiposStr.Substring(i + 1, equiposStr.Length - i - 1);
                        cont=0;
                        i = 0;
                        
                        //validando que el salon sea valido
                        if (db.AulaEdificios.SingleOrDefault(a => a.Id ==lugar)!=null)
                        {
                            nuevaSolicitud.IdLugar = lugar;
                        }else{

                            return "Error con el codigo Lugar.\nVuelva a intentarlo.\nEnviar Lu para saber mas.";
                        }
                    }
                    //sacando los equipos y la cantidad para agregarlos a la solicitud
                    if (equiposStr.ElementAt(i) == '*' && cont == 0)
                    {
                        indice2 = i;
                        cont++;
                    }
                    else if (equiposStr.ElementAt(i) == '*' && cont != 0)
                    {
                        equipos.Add(new Tuple<string, string>(equiposStr.Substring(indice, indice2), equiposStr.Substring((indice2) + 1, i - indice2 - 1)));

                        indice = 0;
                        cont = 0;

                        equiposStr = equiposStr.Substring(i + 1, equiposStr.Length - i - 1);
                        i = 0;

                    }


                }

               
                
                
                //Verificando si el modelo seleccionado esta disponible con relacion a los modelos disponibles por fecha y hora
                foreach (var item in equipos)
                {
                    int cantidadXModelo=equiposDisponibles.Where(e => e.IdModelo ==Int32.Parse(item.Item1)).Count();

                    if (cantidadXModelo==0)
                    {
                        return "Equipo " + item.Item1 + " no esta disponible.\nSolicitud cancelada\n Vuelva a intentarlo";
                    }
                    else if(cantidadXModelo<Int32.Parse(item.Item2))
                    {
                        
                        return "Cantidad Equipo " + item.Item1 + " solo hay "+cantidadXModelo+" disponible.\nSolicitud cancelada\n Vuelva a intentarlo";
                    }
                    else
                    {
                        int cantidad=Int32.Parse(item.Item2);
                        int cantidadXModelo2 = equiposDisponibles.Where(e => e.IdModelo == Int32.Parse(item.Item1)).Count();

                        foreach (var equipo in equiposDisponibles)
                        {
                            if (cantidad > 0 && equipo.IdModelo == Int32.Parse(item.Item1) && cantidad<=cantidadXModelo2)
                            {
                                SolicitudEquipo nuevo=new SolicitudEquipo();

                                if (SolicitudsId.Count() > 0)
                                {
                                    nuevo.IdSolicitud = SolicitudsId.Max() + 1;
                                }
                                else
                                {
                                    nuevo.IdSolicitud = 1;
                                }
                                
                                nuevo.idEquipo =equipo.Id;
                                //Actualizamos el estatus del equipo para que se refleje su primer uso en caso de que no haya sido utilizado
                                db.Equipoes.SingleOrDefault(e => e.Id == equipo.Id).IdEstatusEquipo = 1;
                                nuevaSolicitud.SolicitudEquipoes.Add(nuevo);

                                cantidad--;
                            }

                        }
                    }

                }

                
                nuevaSolSms.IdContacto = db.Contactoes.SingleOrDefault(c => c.Descripcion == telefono).Id;
                

                try
                {
                    db.Solicituds.Add(nuevaSolicitud);
                    db.SaveChanges();
                    //Sacando el ultimo id registrado de la solicitud para asignarlo a la solicitudSms
                    nuevaSolSms.IdSolicitud= db.Solicituds.Select(s=>s.Id).Max();
                    db.SolicitudSms.Add(nuevaSolSms);
                    db.SaveChanges();
                    return "Solicitud Procesada.\nSu codigo:" + nuevaSolSms.IdSolicitud + "\n!Gracias!\n!Favor guardar Codigo!";
                }
                catch (DbEntityValidationException e)
                {
                    return e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
                }

            }
            catch(Exception e)
            {

                return "Error procesando su solicitud Revise el formato";
            }

        }


        public string getCodigoEquipos(string ce){

            try
            {
                string respuesta = null;
                if (ce == null)
                {
                    respuesta = "\nCodigos Equipos";

                    foreach (var modelo in db.ModeloEquipoes)
                    {

                        respuesta = respuesta + "\n" + modelo.Nombre + "=" + modelo.Id;
                    }


                    return respuesta;
                }
                else
                {
                    
                    int codigoEquipo = Int32.Parse(ce);
                    respuesta = db.ModeloEquipoes.SingleOrDefault(e => e.Id == codigoEquipo).Descripcion;

                    return respuesta;
                }
            }
            catch 
            {
                return "Problema al procesar el equipo !verifique Formato instruccion!";
            }
        }

        public string getSalones(string cs)
        {

            try
            {
                string respuesta = null;
                if (cs == null)
                {
                    respuesta = "\nCodigos Salones";

                    foreach (var modelo in db.AulaEdificios)
                    {

                        respuesta = respuesta + "\n" + modelo.Aula + "=" + modelo.Id;
                    }


                    return respuesta;
                }
                else
                {

                    int codigoEquipo = Int32.Parse(cs);
                    respuesta = db.ModeloEquipoes.SingleOrDefault(e => e.Id == codigoEquipo).Descripcion;

                    return respuesta;
                }
            }
            catch
            {
                return "Problema al procesar el equipo !verifique Formato instruccion!";
            }
        }




        //Consultando los equipos disponibles por modelos
        public List<Equipo> EquiposDisponibles(string fecha, string horaInicio, string horaFin)
        {

            DateTime fechaObj = new DateTime();
            fechaObj = DateTime.Parse(fecha);
            fecha = fechaObj.ToString("yyyy-MM-dd");

            //Almacena los equipos que estan disponibles para ser solicitado
            List<Equipo> EquiposDisponibles = new List<Equipo>();

            //Consultando los equipos que podrian estar disponible para la solicitud
            EquiposDisponibles = db.Equipoes.Where(e => e.IdEstatusEquipo == 5 || e.IdEstatusEquipo == 1).ToList();

            //Consultando los equipos que no han sido solicitado para la fecha indicada para eliminarlos
            //de la lista de equipos disponibles


            var query = db.Database.SqlQuery<int>("EXEC EquiposNoDisponibles {0},{1},{2}", fecha, horaInicio, horaFin);

            //Agregando los equipos a la lista de equipos disponibles

            foreach (var item in query)
            {

                EquiposDisponibles.Remove(EquiposDisponibles.SingleOrDefault(e => e.Id == item));

            }
            





            return  EquiposDisponibles;
        }


        public DateTime formatoFecha(DateTime fecha, int meses)
        {

           return fecha.AddMonths(meses);
        }

    }
}
