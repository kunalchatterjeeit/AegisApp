using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class BaseModel
    {
        public int UserId { get; set; }
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public string DeviceId { get; set; }
    }
}
