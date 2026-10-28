using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Domain.Entities.Credentials.ValueObjects
{
    public enum CredentialStatus
    {
        Active = 1,
        Blocked = 2,
        Revoked = 3,
    }
}
