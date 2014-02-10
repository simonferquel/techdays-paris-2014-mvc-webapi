using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EbookManager.Domain.Catalog;
using EbookManager.Domain.Users;

namespace EbookManager.Repositories
{
	public class CatalogRepository
	{
		private readonly EbookManagerDbContext db;
		public CatalogRepository(EbookManagerDbContext db)
		{
			this.db = db;
		}

		public async Task<List<Ebook>> LoadCatalogAsync()
		{
			return await this.db.Ebooks.Include("Parts").ToListAsync();
		}

        public async Task<List<Ebook>> LoadCatalogWithoutPartsAsync()
        {
            return await this.db.Ebooks.ToListAsync();
        }

		public async Task<List<Ebook>> LoadUserCatalog(string userName)
		{
			var user = await this.db.Users.Include("Ebooks").FirstOrDefaultAsync(u => u.UserName == userName);
			if(user == null)
				throw new InvalidOperationException("The user does not exists");

			var ebooksIds = user.Ebooks.Select(e => e.EbookId).ToArray();
			var ebooks = await this.db.Ebooks.Where(e => ebooksIds.Contains(e.Id)).ToListAsync();

			return ebooks;
		}

        public async Task<List<Tuple<Ebook, int>>> LoadUserCatalogWithPartCount(string userName)
        {
            var user = await this.db.Users.Include("Ebooks").FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
                throw new InvalidOperationException("The user does not exists");

            var ebooksIds = user.Ebooks.Select(e => e.EbookId).ToArray();
            var ebooks = await this.db.Ebooks.Where(e => ebooksIds.Contains(e.Id))
                .Select(b=>new {Ebook = b, PartCount = b.Parts.Count()}).ToListAsync();

            return ebooks.Select(b=>Tuple.Create(b.Ebook, b.PartCount)).ToList();
        }

		public async Task BuyEbookAsync(string userId, Guid ebookId)
		{
			var user = await this.db.Users.Include("Ebooks").FirstOrDefaultAsync(u => u.Id == userId);
			if (user != null)
			{
				user.Ebooks.Add(new UserEbook(){ EbookId = ebookId, UserId =  user.Id });
				await this.db.SaveChangesAsync();
			}
		}

		public async Task AddEbookAsync(Ebook ebook)
		{
			this.db.Ebooks.Add(ebook);
			await this.db.SaveChangesAsync();
		}

		public async Task<Ebook> GetEbookAsync(Guid ebookId)
		{
			return await this.db.Ebooks.Include("Parts").FirstOrDefaultAsync(e => e.Id == ebookId);
		}

		public async Task UpdateBookAsync(Ebook ebook)
		{
			this.db.Entry(ebook).State = EntityState.Modified;
			await this.db.SaveChangesAsync();
		}

		public EbookPart GetEbookPart(Guid ebookId, int position)
		{
			return this.db.EbookParts.FirstOrDefault(e => e.EbookId == ebookId && e.Position == position);
		}

		public async Task DeleteEbookAsync(Guid ebookId)
		{
			var ebook = await GetEbookAsync(ebookId);
			if (ebook != null)
			{
				foreach (var part in ebook.Parts.ToList())
				{
					this.db.EbookParts.Remove(part);
				}

				this.db.Ebooks.Remove(ebook);

				await this.db.SaveChangesAsync();
			}
		}

		public async Task<bool> UserOwnsBookAsync(string userId, Guid ebookId)
		{
		    var user = await this.db.Users.Include("Ebooks").FirstOrDefaultAsync(u => u.Id == userId);
		    if (user == null)
		        return false;

		    return user.Ebooks.Any(e => e.EbookId == ebookId);
		}
	}
}
