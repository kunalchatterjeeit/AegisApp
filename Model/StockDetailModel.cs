namespace Model
{
    public class StockDetailModel : BaseModel
    {
        public string ItemId { get; set; }
        public string ItemType { get; set; }
        public string AssetLocationId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
