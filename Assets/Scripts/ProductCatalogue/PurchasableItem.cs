class PurchasableItem
{
    public string Name { get; private set; }
    public int Price { get; private set; } // Price in USD in cents
    public string Description { get; private set; }

    protected PurchasableItem(string name, int price, string description)
    {
        Name = name;
        Price = price;
        Description = description;
    }
}