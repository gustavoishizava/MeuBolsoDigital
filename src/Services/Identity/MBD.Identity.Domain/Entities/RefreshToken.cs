using System;
using MBD.Core.Entities;

namespace MBD.Identity.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid Token { get; private set; }
        public int ExpiresIn { get; private set; }
        public DateTime? RevokedOn { get; private set; }

        public DateTime ExpiresAt => CreatedAt.AddSeconds(ExpiresIn);
        public bool IsExpired => DateTime.Now > ExpiresAt;
        public bool IsRevoked => RevokedOn != null;
        public bool IsValid => !IsExpired && !IsRevoked;

        internal RefreshToken(Guid userId, int expiresIn)
        {
            UserId = userId;
            ExpiresIn = expiresIn;
            Token = Guid.NewGuid();
            RevokedOn = null;
        }

        public void Revoke()
        {
            if(IsRevoked)
                return;

            RevokedOn = DateTime.Now;
        }
    }
}