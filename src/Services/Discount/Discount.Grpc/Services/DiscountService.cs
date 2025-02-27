using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Coupon.Id) ?? 
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
        coupon.ProductName = request.Coupon.ProductName;
        coupon.Description = request.Coupon.Description;
        coupon.Amount = request.Coupon.Amount;
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();
        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} not found"));
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();
        return new DeleteDiscountResponse { Success = true };
    }
}
