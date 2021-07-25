using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using assigment_1.Models.DAL;

namespace assigment_1.Models
{
    public class Episode: IEquatable<Episode>
    {
        int episodeID;
        int seriesID;
        int userId;
        string seriesName;
        string episodeName;
        int seasonNum;
        string imgURL;
        string description;
        int numOfUsers;


        public string SeriesName { get => seriesName; set => seriesName = value; }
        public string EpisodeName { get => episodeName; set => episodeName = value; }
        public int SeasonNum { get => seasonNum; set => seasonNum = value; }
        public string ImgURL { get => imgURL; set => imgURL = value; }
        public string Description { get => description; set => description = value; }
        public int EpisodeID { get => episodeID; set => episodeID = value; }
        public int SeriesID { get => seriesID; set => seriesID = value; }
        public int UserId { get => userId; set => userId = value; }
        public int NumOfUsers { get => numOfUsers; set => numOfUsers = value; }

        public int Insert()
        {
            DataServices ds = new DataServices();
            ds.InsertEpisode(this);
            return 1;
        }


        public List<Episode> GetEpList(int userId, int seriesId)
        {
            DataServices ds = new DataServices();
            return ds.GetEpListByTv(userId, seriesId);
          
        }

        public List<Episode> GetAllEpisods()
        {
            DataServices ds = new DataServices();
            return ds.GetAllEpisods();
         
        }

        public bool Equals(Episode ep)
        {
            return this.SeriesName == ep.SeriesName && this.EpisodeName == ep.EpisodeName && this.SeasonNum == ep.SeasonNum;
        }

        public int DeletefromUserLikeEpisode(int userid,int epid)
        {
            DataServices ds = new DataServices();
            return ds.DeletefromUserLikeEpisode(userid, epid);
        }

    }
}