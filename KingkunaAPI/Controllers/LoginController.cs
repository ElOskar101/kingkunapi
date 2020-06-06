using KingkunaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace KingkunaAPI.Controllers{
    //OK here we go
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController {

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing() {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser() {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate([FromBody]User user) {
            Model db = new Model();

            User auth = new User();

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            auth = db.User.Where(s => s.Username == user.Username
                                           && s.Pwd == user.Pwd).FirstOrDefault();

            if (auth != null) {
                var token = TokenGeneratorController.GenerateTokenJwt(user.Username);
                user.UserID = auth.UserID;
                user.Name = auth.Name;
                user.Pwd = auth.Pwd;
                user.Token = token;
                user.Image = auth.Image;
                return Ok(user);
            } else
                return Unauthorized();
        }
    }
}
