namespace Model
{
    public class AttendanceModel : BaseModel
    {
        public AttendanceModel() { }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IsMockedLocation { get; set; }
        public string AttendanceMode { get; set; }
        public string CurrentState { get; set; }
        public bool IsAttendanceBlocked { get; set; }
        public string AttendanceInDate { get; set; }
        public string AttendanceOutDate { get; set; }
        public string TotalWorkingHours { get; set; }
        public string IsLate { get; set; }
        public string IsLateReduced { get; set; }
        public string IsHalfDay { get; set; }
    }
}
