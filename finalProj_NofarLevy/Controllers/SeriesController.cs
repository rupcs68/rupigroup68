using assigment_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace assigment_1.Controllers
{
    public class SeriesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Series> Get()
        {
            Series s = new Series();
            return s.GetAllSeriesList();
        }

        // GET api/<controller>/5
        public List<Series> Get(int userId)
        {
            Series s = new Series();
            return s.GetSeriesList(userId);
        }

        [HttpGet]
        [Route("api/Series/getRecommendSeriesForUser/{userid}")]
        public List<Series> getRecommendSeriesForUser(int userid)
        {
            Series s = new Series();
            return s.getRecommendSeriesForUser(userid);
        }

        // POST api/<controller>
        public int Post([FromBody]Series s)
        {
            s.Insert();
            return 1;
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