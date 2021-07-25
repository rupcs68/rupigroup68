using assigment_1.Models;
using assigment_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace assigment_1.Controllers
{
    public class EpisodesController : ApiController
    {
        // GET api/<controller>?userId = 5
        public IEnumerable<Episode> Get()
        {
            Episode ep = new Episode();
            return ep.GetAllEpisods();
           
        }

        // GET api/<controller>?userId = 1&seriesId = 1234
        public IEnumerable<Episode> Get(int userId ,int seriesId)
        {
            Episode ep = new Episode();
            List<Episode> epList = ep.GetEpList(userId, seriesId);
            return epList;
        }

        // POST api/<controller>
        public Episode Post([FromBody] Episode ep)
        {
            ep.Insert();
            return ep;
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpDelete]
        [Route("api/Episodes/DeletefromUserLikeEpisode/{userid}/{epid}")]
        public int DeletefromUserLikeEpisode(int userid, int epid)
        {
            Episode ep = new Episode();
            return ep.DeletefromUserLikeEpisode(userid, epid);
        }
    }
}