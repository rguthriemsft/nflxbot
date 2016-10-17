using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace UserProfileApi.Models
{
    public class UsersContext : DbContext   
    {
        public UsersContext() : base("name=UsersContext") {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}