using System;
using System.Collections.Generic;
using System.Text;

namespace Aegis_Gps_App
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
