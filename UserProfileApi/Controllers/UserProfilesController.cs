using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserProfileApi.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.OData;

namespace UserProfileApi.Controllers
{
    public class UserProfilesController : ODataController
    {
        UserProfilesContext db = new UserProfilesContext();

        private bool UserProfileExists(string userName)
        {
            return db.Users.Any(u => u.UserName == userName);
        }

        private bool UserProfileExists(int key)
        {
            return db.Users.Any(u => u.Id == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery]
        public IQueryable<UserProfile> Get()
        {
            return db.Users;
        }

        [EnableQuery]
        public SingleResult<UserProfile> Get([FromODataUri] int key)
        {
            IQueryable <UserProfile> result = db.Users.Where(u => u.Id == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public SingleResult<UserProfile> Get([FromODataUri] string userName)
        {
            IQueryable<UserProfile> result = db.Users.Where(u => u.UserName == userName);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(UserProfile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Users.Add(profile);
            await db.SaveChangesAsync();
            return Created(profile);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<UserProfile> profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Users.FindAsync(key);
            if (entity != null)
            {
                return NotFound();
            }

            profile.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!UserProfileExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);

        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var profile = await db.Users.FindAsync(key);
            if (profile == null)
            {
                return NotFound();
            }
            db.Users.Remove(profile);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
