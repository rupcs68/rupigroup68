using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using assigment_1.Models.DAL;


namespace assigment_1.Models.DAL
{
    public class User : IEquatable<User>
    {
        int userId;
        string firsName;
        string lastName;
        string email;
        string password;
        string phoneNumber;
        string gender;
        int birthYear;
        string favCategory;
        string address;
        bool isManager;

        public string FirsName { get => firsName; set => firsName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Gender { get => gender; set => gender = value; }
        public int BirthYear { get => birthYear; set => birthYear = value; }
        public string FavCategory { get => favCategory; set => favCategory = value; }
        public string Address { get => address; set => address = value; }
        public int UserId { get => userId; set => userId = value; }
        public bool IsManager { get => isManager; set => isManager = value; }

        public List<User> Get()
        {
            DataServices ds = new DataServices();
            return ds.GetAllUsers();
             
        }

        public User Get(string email, string password)
        {
            DataServices ds = new DataServices();
            return  ds.GetUserByEmail(email,password);
          
        }

        public List<User> GetUsersExsaptM(int managerId)
        {
            DataServices ds = new DataServices();
            return  ds.GetUsersExeptManager(managerId);

        }

        public int Insert()
        {
            DataServices ds = new DataServices();
            return ds.InsertUser(this);
             
        }


        public bool Equals(User u)
        {
            return this.Email == u.Email;
        }

        public User updateDetails()
        {
            DataServices ds = new DataServices();
            return ds.updateDetails(this);
        }
    }


}