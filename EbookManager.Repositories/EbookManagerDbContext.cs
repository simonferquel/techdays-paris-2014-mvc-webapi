using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Catalog;
using EbookManager.Domain.Users;

namespace EbookManager.Repositories
{
    public class EbookManagerDbContext : DbContext
    {
        static EbookManagerDbContext()
        {
            Database.SetInitializer(new NullDatabaseInitializer<EbookManagerDbContext>());
        }

        public EbookManagerDbContext()
            : base("EbookManagerDatabase")
        {
            
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<UserLogin> UserLogins { get; set; }
        public IDbSet<UserClaim> UserClaims { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public IDbSet<Ebook> Ebooks { get; set; }
        public IDbSet<EbookPart> EbookParts { get; set; } 
    }
}
