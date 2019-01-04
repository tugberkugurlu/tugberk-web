using System;

namespace Tugberk.Domain.ReadSide.ReadModels
{
    public class ChangeRecordReadModel 
    {
        public UserReadModel RecordedBy { get; set; }
        public DateTime RecordedOn { get; set; }
        public string IpAddress { get; set; }
    }
}
