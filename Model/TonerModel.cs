using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class TonerModel : BaseModel
    {
        public string TonerNo { get; set; }
        public string TonerDateTime { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string CallStatus { get; set; }
        public string ContactPerson { get; set; }
    }
}
