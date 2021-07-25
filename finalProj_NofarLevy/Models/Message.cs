using assigment_1.Models;
using assigment_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finalProj_NofarLevy.Models
{
    public class Message
    {
        int msgID;
        int seriesId;
        string text;
        DateTime date;
        int likeAmount;
        int userIdMsg;
        string userNamedMsg;
        int userLikeMsgId;
        List<Message> comments;
        bool currentUserLike;
        int commentAmount;

        public int MsgID { get => msgID; set => msgID = value; }
        public string Text { get => text; set => text = value; }
        public DateTime Date { get => date; set => date = value; }
        public int LikeAmount { get => likeAmount; set => likeAmount = value; }
        public int UserIdMsg { get => userIdMsg; set => userIdMsg = value; }
        public int UserLikeMsgId { get => userLikeMsgId; set => userLikeMsgId = value; }
        public int SeriesId { get => seriesId; set => seriesId = value; }
        public string UserNamedMsg { get => userNamedMsg; set => userNamedMsg = value; }
        public List<Message> Comments { get => comments; set => comments = value; }
        public bool CurrentUserLike { get => currentUserLike; set => currentUserLike = value; }
        public int CommentAmount { get => commentAmount; set => commentAmount = value; }

        public Message()
        {
            Comments = new List<Message>();
        }

        public List<Message> GetMsgByTvid(int seriesId, int currentUser)
        {
            DataServices ds = new DataServices();
            return ds.GetMsgByTvid(seriesId, currentUser);
        }

        public int Insert()
        {
            DataServices ds = new DataServices();
            return ds.InsertMsg(this);
             
        }

        public int InsertComment(int perentMsgId)
        {
            DataServices ds = new DataServices();
            return ds.InsertCommentToMsg(this, perentMsgId);
        }

        public int DoLike(int msgId, int userId)
        {
            DataServices ds = new DataServices();
            return ds.DoLike(msgId , userId);

        }


        public int DoDislike(int msgId, int userId)
        {
            DataServices ds = new DataServices();
            return ds.DoDislike(msgId, userId);

        }
    }
}