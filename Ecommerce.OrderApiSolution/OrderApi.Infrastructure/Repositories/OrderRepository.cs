using eCommerce.SharedLibrary.LogsException;
using eCommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = await context.Orders.AddAsync(entity);

                await context.SaveChangesAsync();

                return order.Entity.Id > 0
                    ? new Response(true, "Order placed successfully.")
                    : new Response(false, "Failed to place order.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while placing order.");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await FindByIdAsync(entity.Id);
                if (order == null)
                {
                    return new Response(false, "Order not found.");
                }

                context.Orders.Remove(order);
                await context.SaveChangesAsync();

                return new Response(true, "Order deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while placing order.");
            }
        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                if (order is not null) return order;

                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while placing order.");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();

                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occured while retreving order.");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.FirstOrDefaultAsync(predicate);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while placing order.");
            }
        }

        public async Task<IEnumerable<Order>> GetORdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();

                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                throw new Exception("Error occurred while placing order.");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                if (!await context.Orders.AnyAsync(o=>o.Id==entity.Id))
                    return new Response(false, "Order not found.");

                context.Orders.Update(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Order updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);

                return new Response(false, "Error occurred while placing order.");
            }
        }
    }
}
