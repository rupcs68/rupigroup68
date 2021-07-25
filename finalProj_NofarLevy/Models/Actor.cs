using assigment_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finalProj_NofarLevy.Models
{
    public class Actor
    {
        int actorId;
        string actorName;
        string gender;
        string characterName;
        List<string> characters;
        string seriesName;
        string imgUrl;
        string currentUserId;

        public int ActorId { get => actorId; set => actorId = value; }
        public string ActorName { get => actorName; set => actorName = value; }
        public string Gender { get => gender; set => gender = value; }
        public List<string> Characters { get => characters; set => characters = value; }
        public string SeriesName { get => seriesName; set => seriesName = value; }
        public string CharacterName { get => characterName; set => characterName = value; }
        public string ImgUrl { get => imgUrl; set => imgUrl = value; }
        public string CurrentUserId { get => currentUserId; set => currentUserId = value; }

        public Actor()
        {
            Characters = new List<string>();
        }

        public int Insert()
        {
            DataServices ds = new DataServices();
            return ds.InsertActor(this);
        }

        public List<Actor> getAllActorsByUser(int userId)
        {
            DataServices ds = new DataServices();
            return ds.getAllActorsByUser(userId);
        }
    }
}