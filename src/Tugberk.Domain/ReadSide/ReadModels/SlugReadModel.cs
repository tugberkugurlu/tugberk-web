using System;

namespace Tugberk.Domain.ReadSide.ReadModels
{
    public class SlugReadModel
    {
        public string Path { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}