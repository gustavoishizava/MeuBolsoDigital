using System;
namespace MBD.Core.Entities
{
    public abstract class GuidEntityKey
    {
        public Guid Id { get; private set; }
        
        protected GuidEntityKey()
        {
            Id = Guid.NewGuid();
        }
    }
}