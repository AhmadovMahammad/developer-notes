namespace Chapter8
{
    public record Customer
    {
        public Guid CustomerID { get; init; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; init; } = string.Empty;
    }
}