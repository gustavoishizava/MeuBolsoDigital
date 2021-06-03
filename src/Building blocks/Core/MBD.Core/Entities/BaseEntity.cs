using System;
namespace MBD.Core.Entities
{
    public abstract class BaseEntity : GuidEntityKey
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }
    }
}