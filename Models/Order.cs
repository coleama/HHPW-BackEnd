namespace HHPW_BackEnd.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DatePlaced { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public decimal Tip { get; set; }
        public User User { get; set; }
        public ICollection<MenuItem> Items { get; set; }
    }
}
