using assigment_1.Models;
using finalProj_NofarLevy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace finalProj_NofarLevy.Controllers
{
    public class MessagesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public List<Message> Get(int seriesId, int currentUser)
        {
            Message m = new Message();
            return m.GetMsgByTvid(seriesId, currentUser);
        }

        // POST api/<controller>
        public int Post([FromBody]Message msg)
        {
            return msg.Insert();
        }

        // PUT api/<controller>/5
        public int Put(int perendMsgID, [FromBody]Message msg)
        {
            return msg.InsertComment(perendMsgID);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }




        [HttpPost]
        [Route("api/Messages/DoLike/{msgId}/{userId}")]
        public int DoLike(int msgId, int userId)
        {
            Message m = new Message();
            return m.DoLike(msgId, userId);

        }

        [HttpDelete]
        [Route("api/Messages/DoDislike/{msgId}/{userId}")]
        public int DoDislike(int msgId, int userId)
        {
            Message m = new Message();
            return m.DoDislike(msgId, userId);
        }

    }
}