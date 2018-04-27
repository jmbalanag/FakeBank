using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Accounts
{
    public interface IAccountInformationService
    {
        bool IsAccountExists(Guid accountId);
    }
}
