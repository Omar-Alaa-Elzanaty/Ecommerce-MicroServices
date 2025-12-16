using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDto order) => new()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            PurchaseQuantity = order.PurchaseQuantity,
            OrderDate = order.OrderDate
        };

        public static (OrderDto?,IEnumerable<OrderDto>?)FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if(order is not null || order is null)
            {
                var singleOrder = new OrderDto(
                    Id: order!.Id,
                    ClientId: order.ClientId,
                    ProductId: order.ProductId,
                    PurchaseQuantity: order.PurchaseQuantity,
                    OrderDate: order.OrderDate
                );

                return (singleOrder, null);
            }

            if(orders is not null || order is null)
            {
                var _orders = orders!.Select(o => new OrderDto(
                    Id: o.Id,
                    ClientId: o.ClientId,
                    ProductId: o.ProductId,
                    PurchaseQuantity: o.PurchaseQuantity,
                    OrderDate: o.OrderDate
                ));
            }

            return (null, null);
        }
    }
}
