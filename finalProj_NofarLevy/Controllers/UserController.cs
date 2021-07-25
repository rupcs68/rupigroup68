using assigment_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace assigment_1.Controllers
{
    public class UserController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            User user = new User();
            List<User> userList = user.Get();
            return userList;
        }

        // GET api/<controller>/myemail@gmail.com

        public HttpResponseMessage Get(string email, string password)
        {
            User user = new User();
            user = user.Get(email, password);
            if (user.Email == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Email address or Password is incorrect");

            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }



        // GET api/<controller>/5
        public List<User> Get(int managerId)
        {
            User user = new User();
            return user.GetUsersExsaptM(managerId);
            
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] User user)
        {
           int i= user.Insert();
            if (i == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User Email allready exist");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);

        }

        // PUT api/<controller>/5
        public User Put([FromBody]User u)
        {
            return u.updateDetails();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


    }
}