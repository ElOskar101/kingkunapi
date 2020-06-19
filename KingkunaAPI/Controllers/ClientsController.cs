using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KingkunaAPI.Models;

namespace KingkunaAPI.Controllers {

    [AllowAnonymous]
    public class ClientsController : ApiController
    {
        private Model db = new Model();

        [Authorize]
        [HttpGet]
        // GET: api/Clients
        public IQueryable<Client> GetClient()
        {
            return db.Client;
        }

        // GET: api/Clients/5
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetClient(int id){
            //Client c = db.Client.Where(s => s.Status == id);

            List<Client> c = new List<Client>();

            var query  = db.Client
                .Where(s => s.Status == id)
                .Select(s => s);

            if (query == null)
                return NotFound();

            foreach (Client client in query)
                c.Add(client);           
          
           List<Client2> arrayClient2 = new List<Client2>();
            
            for (int i = 0; i < c.Count; i++) {

                Client2 c2 = new Client2();

                c2.ClientID = c[i].ClientID;
                c2.Name = c[i].Name;
                c2.Phone = c[i].Phone;
                c2.Status = c[i].Status;
                c2.DemoDays = c[i].DemoDays;
                c2.HireDate = ConvertDate(c[i].HireDate);
                c2.CancelDate = ConvertDate(c[i].CancelDate);
                c2.CreateAt = ConvertDate(c[i].CreateAt);
                c2.Amount = c[i].Amount;

                arrayClient2.Add(c2);
            }
                       


            return Ok(arrayClient2);
        }

        // PUT: api/Clients/5 Change status. status = 1 (client); status = 2 (old user)
        // if status = 1, then we need amount; the calcel date convert into hire date and the original cancel
        // date, will be cancel date + one month
        [Authorize]
        [HttpPut]
        public IHttpActionResult PutClient(int id, Client client){

            Client auth = db.Client.Where(s => s.ClientID == client.ClientID).FirstOrDefault();

            if (client.Status == 1) {
                auth.HireDate = DateTime.Now;
                auth.CancelDate = auth.HireDate.AddMonths(id);
                auth.Amount = client.Amount;
                auth.Status = client.Status;

            } else if (client.Status == 2)
                auth.Status = client.Status;
            

            db.Entry(auth).State = EntityState.Modified;

            try{
                db.SaveChanges();
            }

            catch (DbUpdateConcurrencyException){
                if (!ClientExists(id))
                    return NotFound();
                else throw;
                
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Clients Is this resourse we need to save new client with only three parameters
        [Authorize]
        [HttpPost]
        public IHttpActionResult PostClient(Client client){

            client.HireDate = DateTime.Now;
            client.CancelDate = DateTime.UtcNow.AddDays(client.DemoDays);
            client.CreateAt = DateTime.Now;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.ClientID }, client);
        }

        // DELETE: api/Clients/5
        [Authorize]
        [HttpDelete]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing){
            if (disposing)
                db.Dispose();
            
            base.Dispose(disposing);
        }

        private bool ClientExists(int id) {return db.Client.Count(e => e.ClientID == id) > 0;}

        private String ConvertDate(DateTime date) { // Return date in real format
            String month;

            month = ConverToStringMonth(date.Month); 

            return date.Day+" de "+month+" de "+date.Year;
        }

        private String ConverToStringMonth(int month) { // Switch between all months of the years

            switch (month) {
                case 1:
                    return ("Enero");
                case 2:
                    return ("Febrero");
                case 3:
                    return ("Marzo");
                case 4:
                    return ("Abri");
                case 5:
                    return ("Mayo");
                case 6:
                    return ("Junio");
                case 7:
                    return ("Julio");
                case 8:
                    return ("Agosto");
                case 9:
                    return ("Septiembre");
                case 10:
                    return ("Octubre");
                case 11:
                    return ("Noviembre");
                case 12:
                    return ("Diciembre");
                default:
                    return "false";
            }
        }
    }
}