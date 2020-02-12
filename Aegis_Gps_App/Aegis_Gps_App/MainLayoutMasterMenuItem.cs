using System;

namespace Aegis_Gps_App
{

    public class MainLayoutMasterMenuItem
    {
        public MainLayoutMasterMenuItem()
        {
            TargetType = typeof(MainLayoutMasterMenuItem);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string PageName { get; set; }

        public Type TargetType { get; set; }
    }
}