namespace _2026_Csh_Advanced.sprint11_Reflection;

public class OrderDto
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    [OrderRange(0.01, 50000)]
    public double TotalPrice { get; set; }
    [SensitiveData]
    public string CardNumber { get; set; }
    [SensitiveData]
    public string CustomerPhone { get; set; }
    public DateTime OrderDate { get; set; }

    public OrderDto()
    {
      OrderId = 0;
      CustomerName = "";
      TotalPrice = 0;
      CardNumber = "";
      CustomerPhone = "";
      OrderDate = DateTime.Now;
    }
    public OrderDto(int orderId, string customerName, double totalPrice, string cardNumber, string customerPhone, DateTime orderDate)
    {
      OrderId = orderId;
      CustomerName = customerName;
      TotalPrice = totalPrice;
      CardNumber = cardNumber;
      CustomerPhone = customerPhone;
      OrderDate = orderDate;
    }
}