using finalProj_NofarLevy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace finalProj_NofarLevy.Controllers
{
    public class ActorsController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public List<Actor> Get(int userId)
        {
            Actor a = new Actor();
            return a.getAllActorsByUser(userId);
        }

        // POST api/<controller>
        public int Post([FromBody]Actor a)
        {
           return a.Insert();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}