namespace Model
{
    public class LoyaltyPointModel : BaseModel
    {
        public LoyaltyPointModel() { }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Point { get; set; }
        public string Reason { get; set; }
    }
}
