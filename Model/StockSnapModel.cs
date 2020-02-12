namespace Model
{
    public class StockSnapModel : BaseModel
    {
        public string ItemName { get; set; }
        public string Location { get; set; }
        public string Quantity { get; set; }
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public string AssetLocationId { get; set; }

    }
}
