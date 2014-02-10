using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Users;
using Microsoft.AspNet.Identity;

namespace EbookManager.Repositories
{
    //Implémentation de la partie "stockage" de Identity Core
    public class UsersRepository : IUserLoginStore<User>, IUserClaimStore<User>, IUserRoleStore<User>, 
        IUserPasswordStore<User>, IUserSecurityStampStore<User>, 
        IUserStore<User>, IDisposable
    {
        private readonly EbookManagerDbContext db;

        public UsersRepository(EbookManagerDbContext db)
        {
            this.db = db;
        }

        #region IUserStore

        public async Task CreateAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            this.db.Users.Add(user);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this.db.Entry(user).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            this.db.Entry(user).State = EntityState.Deleted;
            await this.db.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return await this.db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await this.db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        } 

        #endregion

        #region IUserLoginStore
        public async Task AddLoginAsync(User user, UserLoginInfo login)
        {
            this.db.Entry(user).State = EntityState.Modified;

            if (user.Logins == null)
            {
                user.Logins = new List<UserLogin>();
            }

            user.Logins.Add(new UserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            });

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            var userLogin = await this.db.UserLogins.FirstOrDefaultAsync(u => u.LoginProvider == login.LoginProvider 
                && u.ProviderKey == login.ProviderKey && u.UserId == user.Id);

            if (userLogin != null)
            {
                this.db.UserLogins.Remove(userLogin);
                await this.db.SaveChangesAsync();
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            var query = this.db.UserLogins.Where(u => u.UserId == user.Id);
            var results = await query.ToListAsync();

            return results.Select(u => new UserLoginInfo(u.LoginProvider, u.ProviderKey)).ToList();
        }

        public async Task<User> FindAsync(UserLoginInfo login)
        {
            var userLogin = await this.db.UserLogins.Include("User").FirstOrDefaultAsync(u => u.LoginProvider == login.LoginProvider && u.ProviderKey == login.ProviderKey);
            if (userLogin != null)
            {
                return userLogin.User;
            }

            return null;
        } 

        #endregion

        #region IUserClaimStore
        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            var query = this.db.UserClaims.Where(u => u.UserId == user.Id);
            var results = await query.ToListAsync();

            return results.Select(u => new Claim(u.ClaimType, u.ClaimValue)).ToList();
        }

        public async Task AddClaimAsync(User user, Claim claim)
        {
            this.db.Entry(user).State = EntityState.Modified;

            if (user.Claims == null)
            {
                user.Claims = new List<UserClaim>();
            }

            user.Claims.Add(new UserClaim()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                UserId = user.Id
            });

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveClaimAsync(User user, Claim claim)
        {
            var userClaim = await this.db.UserClaims.FirstOrDefaultAsync(c => c.ClaimType == claim.Type && c.UserId == user.Id && c.ClaimValue == claim.Value);
            if (userClaim != null)
            {
                this.db.Entry(userClaim).State = EntityState.Deleted;
                await this.db.SaveChangesAsync();
            }
        } 
        #endregion

        #region IUserRoleStore
        public async Task AddToRoleAsync(User user, string role)
        {
            this.db.Entry(user).State = EntityState.Modified;

            if (user.Roles == null)
            {
                user.Roles = new List<UserRole>();
            }

            user.Roles.Add(new UserRole()
            {
                RoleId = role,
                UserId = user.Id
            });

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveFromRoleAsync(User user, string role)
        {
            var userRole = await this.db.UserRoles.FirstOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == role);
            if (userRole != null)
            {
                this.db.Entry(userRole).State = EntityState.Deleted;
                await this.db.SaveChangesAsync();
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var query = this.db.UserRoles.Where(u => u.UserId == user.Id);
            var results = await query.ToListAsync();

            return results.Select(r => r.RoleId).ToList();
        }

        public async Task<bool> IsInRoleAsync(User user, string role)
        {
            var userRoles = await GetRolesAsync(user);
            return userRoles.Contains(role);
        } 
        #endregion

        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(User user)
        {
            var dbUser = await this.db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (dbUser != null)
            {
                return dbUser.PasswordHash;
            }

            return string.Empty;
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            var dbUser = await this.db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (dbUser != null)
            {
                return !string.IsNullOrEmpty(dbUser.PasswordHash);
            }

            return false;
        } 
        #endregion

        #region IUserSecurityStampStore
        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;
        }

        public async Task<string> GetSecurityStampAsync(User user)
        {
            var dbUser = await this.db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (dbUser != null)
            {
                return dbUser.SecurityStamp;
            }

            return string.Empty;
        } 
        #endregion

        public async Task<List<User>> ListAsync()
        {
            return await this.db.Users.OrderBy(u => u.UserName).ToListAsync();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
