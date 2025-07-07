namespace E_TicaretNew.Domain.Enums.OrderEnum;

public enum OrderStatus
{
    Pending,     // Sifariş verildi, gözləyir təsdiq üçün
    Processing,  // Seller tərəfindən işlənir
    Shipped,     // Göndərildi
    Delivered,   // Alıcı tərəfindən qəbul edildi
    Cancelled    // Ləğv edildi
}
