using assigment_1.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assigment_1.Models
{
    public class Series : IEquatable<Series>
    {
        int seriesId;
        string firsAirDate;
        string name;
        string originCountry;
        string originalLanguage;
        string overview;
        float popularity;
        string posterPath;
        int numOfUsers;


        public int SeriesId { get => seriesId; set => seriesId = value; }
        public string FirsAirDate { get => firsAirDate; set => firsAirDate = value; }
        public string Name { get => name; set => name = value; }
        public string OriginCountry { get => originCountry; set => originCountry = value; }
        public string OriginalLanguage { get => originalLanguage; set => originalLanguage = value; }
        public string Overview { get => overview; set => overview = value; }
        public float Popularity { get => popularity; set => popularity = value; }
        public string PosterPath { get => posterPath; set => posterPath = value; }
        public int NumOfUsers { get => numOfUsers; set => numOfUsers = value; }

        public int Insert()
        {
            DataServices ds = new DataServices();
            ds.InsertSeries(this);
            return 1;
        }

        public List<Series> GetSeriesList(int userId)
        {
            DataServices ds = new DataServices();
            return ds.GetSeriesList(userId);

        }

        public List<Series> GetAllSeriesList()
        {
            DataServices ds = new DataServices();
            return ds.GetAllSeriesList();

        }

        public bool Equals(Series s)
        {
            return this.SeriesId == s.SeriesId;
        }

        public List<Series> getRecommendSeriesForUser(int userid)
        {
            DataServices ds = new DataServices();
            return ds.getRecommendSeriesForUser(userid);
        }
    }
}