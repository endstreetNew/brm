using Microsoft.AspNetCore.Identity;
using Sassa.eForms;
using Sassa.eForms.Models;
using Sassa.eServices.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


namespace Sassa.eServices.Admin.Services
{
    public class SassaUserStore
    { 
        private readonly API Api;
        private SassaUser user;

        public SassaUserStore()
        {
            //Api = new API(clientFactory.CreateClient("UserService"));
        }

        //public async Task<SassaUser> SaveSassaUser(SassaUser item)
        //{
        //    string apicall = "api/Users";
        //    return await Api.PostRequest(apicall, item).ConfigureAwait(false);
        //}
        public async Task<SassaUser> GetSassaUser(string item)
        {
            try
            {

                string apicall = "api/Users/UserName";
                apicall = $"{apicall}/{item}";
                user = await Api.GetResult<SassaUser>(apicall).ConfigureAwait(false);
                return user;
            }
            catch
            {
                return new SassaUser();
            }

        }

        public List<SassaUser> SearchUsers(string searchString)
        {
            //string apicall = "api/Users/Search";
            //apicall = $"{apicall}/{item}";
            //return await Api.GetResult<List<SassaUser>>(apicall).ConfigureAwait(false);

            //UsersController controller = new UsersController(new SassaServicesContext());
            //return await controller.GetUsers(item);
            using (SassaServicesContext _context = new SassaServicesContext())
            {
                return _context.Users.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString) || u.IdNo.Contains(searchString) || u.CellNumber.Contains(searchString)).ToList();
            }
        }
        public async Task DeleteUser(SassaUser user)
        {
            //string apicall = "api/Users";
            //apicall = $"{apicall}/{user.Id}";
            //await Api.Delete(apicall).ConfigureAwait(false);

            //UsersController controller = new UsersController(new SassaServicesContext());
            //await controller.DeleteUser(user.Id);
            using (SassaServicesContext _context = new SassaServicesContext())
            {
                SassaUser User = await _context.Users.FindAsync(user.Id);
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }


        }

        public async Task UpdateUser(SassaUser item)
        {
            //string apicall = "api/Users";
            //apicall = $"{apicall}/{item.Id}";
            //await Api.PutRequest<SassaUser>(apicall,item).ConfigureAwait(false);
            using (SassaServicesContext _context = new SassaServicesContext())
            {
                SassaUser User = await _context.Users.FindAsync(user.Id);
                user.PasswordHash = item.PasswordHash;
                user.CellNumberConfirmed = item.CellNumberConfirmed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SassaUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            //if(user == null)
            //{
            //    return new SassaUser();
            //}

            string apicall = "api/Users/Email";
            apicall = $"{apicall}/{normalizedEmail}";
            user = await Api.GetResult<SassaUser>(apicall).ConfigureAwait(false);
            return user;
        }

        public Task SetPasswordHashAsync(SassaUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public async Task DeletePendingVerification(SassaUser user)
        {
            string apicall = "api/Users/Email";
            apicall = $"{apicall}/{user.Email}";
            user = await Api.GetResult<SassaUser>(apicall).ConfigureAwait(false);
            if (user == null)
            {
                return;
            }
            if (!user.IsCellConfirmed)
            {
                apicall = $"api/Users/{user.Id}";
                await Api.Delete(apicall).ConfigureAwait(false);
            }
        }
    }
}

