namespace Model
{
    public class DocketModel : BaseModel
    {
        public string DocketNo { get; set; }
        public string DocketDateTime { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string CallStatus { get; set; }
        public string ContactPerson { get; set; }
        public string AssignedEngineerName { get; set; }
        public string IsCallAttended { get; set; }
    }
}
