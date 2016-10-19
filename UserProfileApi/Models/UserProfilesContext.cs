using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace UserProfileApi.Models
{
    public class UserProfilesContext : DbContext   
    {
        public UserProfilesContext() : base("name=UserProfilesContext") {
        }

        public DbSet<UserProfile> Users { get; set; }
    }
}