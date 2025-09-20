namespace TemporalWorkflowDemo.Models
{
    public class Item
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
