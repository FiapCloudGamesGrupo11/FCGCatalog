using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Exceptions;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IApplicationDbContext _context;
        public OrderRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Create(Order order)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(ord => ord.UserId == order.UserId && ord.GameId == order.GameId);

            if (existingOrder != null)
                throw new OrderAlreadyExistsException("Este jogo já foi adquirido.");

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task UpdateStatus(Guid userId, Guid gameId, int status)
        {
            var userGame = await _context.Orders.FirstOrDefaultAsync(ord => ord.UserId == userId && ord.GameId == gameId);

            if (userGame is null)
                throw new Exception($"UserGame não encontrado: userId={userId}, gameId={gameId}");

            if (status == 1)
                userGame.ApproveStatus();
            else
                userGame.RejectedStatus();

            await _context.SaveChangesAsync();
        }
    }
}