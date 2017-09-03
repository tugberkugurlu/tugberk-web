using System;

namespace Tugberk.Domain
{
    public class ChangeRecord 
    {
        public User RecordedBy { get; set; }
        public DateTime RecordedOn { get; set; }
        public string IpAddress { get; set; }
    }
}
