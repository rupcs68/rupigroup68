using finalProj_NofarLevy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace assigment_1.Models.DAL
{


    public class DataServices
    {
        //==============================================users data serice==========================================================//

        //add new user    
        public int InsertUser(User u)
        {
            int isManagerBit = 0;
            if (u.IsManager)
                isManagerBit = 1;

            string strCommand = "INSERT INTO Users_2021_nofar_levy ([firsName],[lastName],[email] ,[phoneNumber] ,[gender],[birthYear],[favCategory],[adress],[password],[isManager]) VALUES('" + u.FirsName + "', '" + u.LastName + "', '" + u.Email + "', '" + u.PhoneNumber + "', '" + u.Gender + "', '" + u.BirthYear + "', '" + u.FavCategory + "', '" + u.Address + "', '" + u.Password + "'," + isManagerBit + "); ";
            return ExecuteSqlCommand(strCommand);
        }

        public User updateDetails(User u)
        {
            if (u.FirsName != "" || u.LastName != "" || u.Address != "" || u.FavCategory != "0")
            {
                string strCommand = " UPDATE Users_2021_nofar_levy SET ";
                if (u.FirsName != "")
                {
                    strCommand += "firsName= '" + u.FirsName + "', ";
                }
                if (u.LastName != "")
                {
                    strCommand += "lastName= '" + u.LastName + "', ";

                }
                if (u.Address != "")
                {
                    strCommand += "adress= '" + u.Address + "', ";

                }
                if (u.FavCategory != "0")
                {
                    strCommand += "favCategory= '" + u.FavCategory + "', ";

                }
                strCommand += "email='"+u.Email+"' WHERE userId=" + u.UserId;
                ExecuteSqlCommand(strCommand);
            }
            String selectSTR = "SELECT U.* FROM Users_2021_nofar_levy as U WHERE email = '" + u.Email + "' and inActive=1";
            List<User> usersList = GetUsers(selectSTR);
            if (usersList.Count > 0)
            {
                return usersList[0];

            }
            return new User();

        }

        public User GetUserByEmail(string email, string password)
        {
            String selectSTR = "SELECT U.* FROM Users_2021_nofar_levy as U WHERE email = '" + email + "' and [password] = '" + password + "' and inActive=1";
            List<User> usersList = GetUsers(selectSTR);
            if (usersList.Count > 0)
            {
                return usersList[0];

            }
            return new User();
        }
        public List<User> GetUsersExeptManager(int managerId)
        {
            String selectSTR = "select * from users_2021_nofar_levy where userid != " + managerId + " and inActive=1";
            return GetUsers(selectSTR);
        }

        public List<User> GetAllUsers()
        {
            String selectSTR = "SELECT U.* FROM Users_2021_nofar_levy where  inActive=1";
            return GetUsers(selectSTR);

        }

        //general get for all get users function
        public List<User> GetUsers(string selectSTR)
        {
            List<User> userList = new List<User>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    User user = new User();
                    bool ismanagerBool = false;
                    if (Convert.ToInt32(dr["IsManager"]) == 1)
                        ismanagerBool = true;

                    user.FirsName = (string)dr["firsName"];
                    user.LastName = (string)dr["lastName"];
                    user.Email = (string)dr["email"];
                    user.Password = (string)dr["password"];
                    user.PhoneNumber = (string)dr["phoneNumber"];
                    user.Gender = (string)dr["gender"];
                    user.BirthYear = Convert.ToInt32(dr["birthYear"]);
                    user.FavCategory = (string)dr["favCategory"];
                    user.Address = (string)dr["adress"];
                    user.UserId = Convert.ToInt32(dr["userid"]);
                    user.IsManager = ismanagerBool;

                    userList.Add(user);
                }
                return userList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //get only usersId list
        public List<int> GetusersIdList(string selectStr)
        {
            List<int> usersList = new List<int>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectStr, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    try
                    {
                        int userid = Convert.ToInt32(dr["userid"]);
                        usersList.Add(userid);
                    }
                    catch (Exception e)
                    {

                    }

                }
                return usersList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }


        //==============================================Series data service==========================================================//

        //insert new series    
        public int InsertSeries(Series s)
        {
            string strCommand = "INSERT INTO Series_2021 ([TVid],[firsAirDate],[name],[originCountry],[originalLanguage],[overview],[popularity],[posterPath]) VALUES('" + s.SeriesId + "','" + s.FirsAirDate + "','" + s.Name + "','" + s.OriginCountry + "','" + s.OriginalLanguage + "','" + s.Overview + "','" + s.Popularity + "','" + s.PosterPath + "');";
            return ExecuteSqlCommand(strCommand);
        }

        public List<Series> GetSeriesList(int userId)
        {
            String selectSTR = "select * from Series_2021 ser inner join (SELECT  distinct s.TVid FROM Episodes_2021 ep inner join userSaveEpisode_2021_nofar_levy u on ep.EPid = u.EPid inner join Series_2021 s on ep.TVid = s.TVid WHERE u.inActive=1 and ep.inActive=1 and s.inActive=1 and u.userId = " + userId + ") as s on s.TVid=ser.TVid where inActive=1";
            return GetSeries(selectSTR);

        }
        public List<Series> GetAllSeriesList()
        {
            String selectSTR = "select * from Series_2021 AS E1 inner join (select E.TVid ,COUNT(distinct UE.USERID) as numOfUsers from Episodes_2021 as E inner join userSaveEpisode_2021_nofar_levy as UE on E.EPid=UE.EPid group by E.TVid ) AS E2 on E1.TVid= E2.TVid where inActive=1";
            return GetSeries(selectSTR);

        }

        //get recomended series list for user by outer users with common series when the users with the largest common series number gets priority
        public List<Series> getRecommendSeriesForUser(int userid)
        {
            List<Series> recSeriesList;
            //return top 5 list of user id that have the largest number of common series with other users
            string selectSTR = "select  top 5 T2.UserId, count (T2.TVid) as numOfSameSeries ";
            selectSTR += "from (select S.TVid , U.UserId from userSaveEpisode_2021_nofar_levy as U inner join Episodes_2021 as E on E.EPid=U.Epid ";
            selectSTR += "inner join Series_2021 as S on E.TVid= S.TVid ";
            selectSTR += "where U.inActive=1 and S.inActive=1 and E.inActive=1 ";
            selectSTR += "group by S.TVid,U.UserId having U.UserId=" + userid + ") as T1 ";
            selectSTR += "left outer join (select S2.TVid , U2.UserId ";
            selectSTR += "from userSaveEpisode_2021_nofar_levy as U2 inner join Episodes_2021 as E2 on E2.EPid=U2.Epid ";
            selectSTR += "inner join Series_2021 as S2 on E2.TVid= S2.TVid ";
            selectSTR += "where U2.inActive=1 and S2.inActive=1 and E2.inActive=1 ";
            selectSTR += "group by S2.TVid,U2.UserId having U2.UserId !=" + userid + ") as T2 on  T1.TVid=T2.TVid ";
            selectSTR += "WHERE T2.UserId is not null group by T2.UserId  order by numOfSameSeries DESC";
            List<int> outerUsersList = GetusersIdList(selectSTR);
            if (outerUsersList.Count > 0)
            {
                //get top 10 of recommended series for the user
                selectSTR = "select  distinct  top 10 S.TVID, S.firsairdate, S.name, S.originCountry, S.originalLanguage,cast(S.overview as nvarchar(max))as overview ,S.popularity ,cast(S.posterPath as nvarchar(max)) as posterPath ";
                selectSTR += "from userSaveEpisode_2021_nofar_levy as U inner join Episodes_2021 as E on E.EPid=U.Epid ";
                selectSTR += "inner join Series_2021 as S on E.TVid= S.TVid ";
                selectSTR += "where S.TVid not in (select S2.tvid ";
                selectSTR += "from userSaveEpisode_2021_nofar_levy as U2 inner join Episodes_2021 as E2 on E2.EPid=U2.Epid ";
                selectSTR += "inner join Series_2021 as S2 on E2.TVid= S2.TVid ";
                selectSTR += "where U2.UserId =" + userid + ") and  U.inActive=1 and S.inActive=1 and E.inActive=1 and(";
                foreach (int u in outerUsersList)
                {
                    selectSTR += "U.UserId=" + u + "or ";
                }
                selectSTR += "U.UserId=0)";
                recSeriesList = GetSeries(selectSTR);
            }
            else//case of no common users
            {
                recSeriesList = new List<Series>();
            }
            return recSeriesList;
        }

        //general get for all get series function
        public List<Series> GetSeries(string selectSTR)
        {
            List<Series> seriesList = new List<Series>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Series s = new Series();
                    s.SeriesId = Convert.ToInt32(dr["TVid"]);
                    s.FirsAirDate = "" + dr["firsAirDate"];
                    s.Name = (string)dr["name"];
                    s.OriginCountry = (string)dr["originCountry"];
                    s.OriginalLanguage = (string)dr["originalLanguage"];
                    s.Overview = (string)dr["overview"];
                    s.Popularity = (float)Convert.ToDouble(dr["popularity"]);
                    s.PosterPath = (string)dr["posterPath"];
                    try
                    {
                        s.NumOfUsers = Convert.ToInt32(dr["numOfUsers"]);
                    }
                    catch
                    {
                       
                    }

                    seriesList.Add(s);

                }

                return seriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //==============================================Episode data service==========================================================//

        public int InsertEpisode(Episode e)
        {
            string strCommand = "INSERT INTO Episodes_2021 " + "([TVid],[EPid],[name],[seasonNumber],[imgURL],[description]) Values('" + e.SeriesID + "','" + e.EpisodeID + "','" + e.EpisodeName + "','" + e.SeasonNum + "','" + e.ImgURL + "','" + e.Description + "')";
            ExecuteSqlCommand(strCommand);
            strCommand = "INSERT INTO userSaveEpisode_2021_nofar_levy " + "([EPid],[userid])" + "Values(" + e.EpisodeID + "," + e.UserId + ")";
            ExecuteSqlCommand(strCommand);
            strCommand = "UPDATE userSaveEpisode_2021_nofar_levy SET inActive = 1 WHERE userid="+ e.UserId + " and epid=" + e.EpisodeID;
            return ExecuteSqlCommand(strCommand);

        }

        public int DeletefromUserLikeEpisode(int userid, int epId)
        {
            string strCommand = "UPDATE userSaveEpisode_2021_nofar_levy SET inActive = 0 WHERE userid=" + userid + " and epid=" + epId;
           return ExecuteSqlCommand(strCommand);
        }

        public List<Episode> GetAllEpisods()
        {
            String selectSTR = "select * from Episodes_2021 AS E1 inner join (select E.EPid ,COUNT(UE.USERID) as numOfUsers from Episodes_2021 as E inner join userSaveEpisode_2021_nofar_levy as UE on E.EPid=UE.EPid where E.inActive=1 and UE.inActive=1 group by E.EPid ) AS E2 on E1.EPid= E2.EPID where e1.inActive=1";
            return GetEpList(selectSTR);

        }

        public List<Episode> GetEpListByTv(int userID, int seriesId)
        {
            String selectSTR = "SELECT ep.* ,s.name as tvName FROM Episodes_2021 ep inner join userSaveEpisode_2021_nofar_levy u on ep.EPid = u.EPid inner join Series_2021 s on ep.TVid = s.TVid WHERE u.userId =" + userID + " and s.TVid=" + seriesId+ " and ep.inActive=1 and u.inActive=1";
            return GetEpList(selectSTR);
        }



        public List<Episode> GetEpList(string selectSTR)
        {
            List<Episode> epList = new List<Episode>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Episode ep = new Episode();
                    ep.EpisodeName = (string)dr["name"];
                    ep.SeasonNum = Convert.ToInt32(dr["seasonNumber"]);
                    ep.ImgURL = (string)dr["imgURL"];
                    ep.Description = (string)dr["description"];
                    ep.EpisodeID = Convert.ToInt32(dr["EPid"]);
                    ep.SeriesID = Convert.ToInt32(dr["TVid"]);
                    try
                    {
                        ep.NumOfUsers = Convert.ToInt32(dr["numOfUsers"]);
                        ep.SeriesName = (string)dr["tvName"];
                    }
                    catch (Exception e)
                    {
                    }
                    epList.Add(ep);
                }

                return epList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //==============================================Actors data service==========================================================//

        
        public int InsertActor(Actor a)
        {
            string strCommand = "insert into actors_2021_nofar_levy(actorId,actorName,actorGender,imgurl) values(" + a.ActorId + ",'" + a.ActorName + "','" + a.Gender + "','" + a.ImgUrl + "')";
            ExecuteSqlCommand(strCommand);
            strCommand = "insert into actors_characters_2021_nofar_levy(actorId,seriesName,characterName) values(" + a.ActorId + ",'" + a.SeriesName + "','" + a.CharacterName + "')";
            ExecuteSqlCommand(strCommand);
            strCommand = "insert into User_Like_Actor_2021_nofar_levy(actorId,[userid]) values(" + a.ActorId + "," + a.CurrentUserId + ")";
            return ExecuteSqlCommand(strCommand);
        }



        public List<Actor> getAllActorsByUser(int userId)
        {
            string selectSTR = "select  A.* from actors_2021_nofar_levy as A inner join (select distinct * from User_Like_Actor_2021_nofar_levy)  as U on A.actorId=U.actorId where u.userid = " + userId+ " and A.inActive=1 and U.inActive=1";
            return GetActors(selectSTR);
        }

        public List<Actor> GetActors(string selectSTR)
        {
            List<Actor> actorsList = new List<Actor>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Actor a = new Actor();
                    a.ActorId = Convert.ToInt32(dr["actorId"]);
                    a.ActorName = (string)dr["actorName"];
                    a.Gender = (string)dr["actorGender"];
                    a.ImgUrl = (string)dr["imgUrl"];

                    actorsList.Add(a);

                }
                string selectCommentStr;
                foreach (Actor a in actorsList)
                {
                    selectCommentStr = "select C.characterName + ' in series: ' + C.seriesName as characterIn  from actors_2021_nofar_levy as A inner join actors_characters_2021_nofar_levy  as C on A.actorId=C.actorId where A.actorId =" + a.ActorId+" select C.characterName + ' in series: ' + C.seriesName as characterIn  from actors_2021_nofar_levy as A inner join actors_characters_2021_nofar_levy  as C on A.actorId=C.actorId where A.actorId =" + a.ActorId+ " and A.inActive=1 and C.inActive=1";
                    a.Characters = getCharactersForA(selectCommentStr);
                }

                return actorsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<string> getCharactersForA(string selectCommentStr)
        {
            List<string> CharactersNameList = new List<string>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectCommentStr, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    string s = (string)dr["characterIn"];

                    CharactersNameList.Add(s);

                }

                return CharactersNameList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //==============================================Messages data service==========================================================//

        public int InsertMsg(Message m)
        {
            string strCommand = "INSERT INTO Messages_2021_2_nofar_levy (msgText,useridMsg,tvid ) VALUES('" + m.Text + "'," + m.UserIdMsg + "," + m.SeriesId + ");";
            return ExecuteSqlCommand(strCommand);
        }

        public int InsertCommentToMsg(Message m, int perentId)
        {
            string strCommand = "INSERT INTO userCommentMsg_2021_3_nofar_levy (userid,msgid,commanttext ) VALUES(" + m.UserIdMsg + "," + perentId + ",'" + m.Text + "');";
            return ExecuteSqlCommand(strCommand);
        }




        public int DoLike(int msgId, int userId)
        {
            string strCommand = " insert into userLikeMsg_2021_2_nofar_levy (userid,msgid) values(" + userId + "," + msgId + ");";
            return ExecuteSqlCommand(strCommand);
        }

        public int DoDislike(int msgId, int userId)
        {
            string strCommand = " delete from userLikeMsg_2021_2_nofar_levy where userid = " + userId + " and msgid=" + msgId;
            return ExecuteSqlCommand(strCommand);
        }



        public List<Message> GetMsgByTvid(int seriesID, int currentUser)
        {
            string strCommand = "select M.* , U.firsName+' '+ U.lastName as userName, isNULL(L.userId,0) AS currentUser , isNull(C.numOfComment,0) as numOfComment, isNull(L2.numOfLikes,0) as numOfLikes ";
            strCommand += "from Messages_2021_2_nofar_levy as M inner join Users_2021_nofar_levy as U on M.useridmsg=U.Userid left outer join ";
            strCommand += "(select * from userLikeMsg_2021_2_nofar_levy where userid= " + currentUser + ")as  L on M.msgid=L.msgid left outer join  ";
            strCommand += "(select msgId , count(userid) as numOfComment from userCommentMsg_2021_3_nofar_levy ";
            strCommand += "group by msgId) as C on C.msgId = M.msgId left outer join ";
            strCommand += "(select msgId , count(userid) as numOfLikes ";
            strCommand += "from userLikeMsg_2021_2_nofar_levy  group by msgId) as L2 on L2.msgId = M.msgId ";
            strCommand += "where tvid=" + seriesID;
            return GetMsg(strCommand);
        }

        public List<Message> GetMsg(string selectSTR)
        {
            List<Message> msgList = new List<Message>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Message m = new Message();
                    m.MsgID = Convert.ToInt32(dr["msgid"]);
                    m.Text = (string)dr["msgText"];
                    m.Date = (DateTime)dr["msgDate"];
                    m.UserIdMsg = Convert.ToInt32(dr["useridmsg"]);
                    m.UserNamedMsg = (string)dr["userName"];


                    try
                    {
                        int currentUser = Convert.ToInt32(dr["currentUser"]);
                        m.CommentAmount = Convert.ToInt32(dr["numOfComment"]);
                        m.LikeAmount = Convert.ToInt32(dr["numOfLikes"]);

                        if (currentUser == 0)
                        {
                            m.CurrentUserLike = false;
                        }
                        else m.CurrentUserLike = true;
                    }
                    catch (Exception e)
                    {

                    }


                    msgList.Add(m);

                }
                string selectCommentStr;
                //get the comment for each message
                foreach (Message msg in msgList)
                {
                    selectCommentStr = "select M.* , U.firsName+' '+ U.lastName as userName  from userCommentMsg_2021_3_nofar_levy as M inner join Users_2021_nofar_levy as U on M.userid = U.Userid where msgid=" + msg.MsgID;
                    msg.Comments = getCommentForMsg(selectCommentStr);
                }

                return msgList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        public List<Message> getCommentForMsg(string selectStr)
        {
            List<Message> msgList = new List<Message>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file


                SqlCommand cmd = new SqlCommand(selectStr, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Message m = new Message();
                    m.MsgID = Convert.ToInt32(dr["msgid"]);
                    m.Text = (string)dr["commantText"];
                    m.Date = (DateTime)dr["msgDate"];
                    m.UserIdMsg = Convert.ToInt32(dr["userid"]);
                    m.UserNamedMsg = (string)dr["userName"];

                    msgList.Add(m);

                }

                return msgList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }



 //==============================================General function for Sql commands====================================================//


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }


        public int ExecuteSqlCommand(string commandStr)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            cmd = CreateCommand(commandStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object
            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 
            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }

    }


}