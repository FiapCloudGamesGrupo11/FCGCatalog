using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();

            return User;
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByEmailPass(string email, string pass)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                p => p.Email == email &&
                p.Password == pass
                );

            return user;
        }

        public async Task<User> Update(User entidade)
        {
            _context.Users.Update(entidade);
            await _context.SaveChangesAsync();

            return entidade;
        }


        //public async Task Update(User entidade)
        //{
        //    await _generalRepo.Update(entidade);
        //}

        //public async Task Delete(Guid id)
        //{
        //    await _generalRepo.Delete(id);
        //}

        //public async Task<int> GetTotalCount()
        //{
        //    return await _generalRepo.CountTotal();
        //}
    }
}
