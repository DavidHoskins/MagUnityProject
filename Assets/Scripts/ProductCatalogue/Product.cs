class Product : PurchasableItem
{
    public int Quantity { get; private set; } 

    public Product(string name, int price, string description, int quantity) : base(name, price, description)
    {
        Quantity = quantity;
    }
}