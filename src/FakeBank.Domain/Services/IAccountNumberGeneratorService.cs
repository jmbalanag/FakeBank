using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Services
{
    public interface IAccountNumberGeneratorService
    {
        string GenerateAccountNumber();
    }
}
