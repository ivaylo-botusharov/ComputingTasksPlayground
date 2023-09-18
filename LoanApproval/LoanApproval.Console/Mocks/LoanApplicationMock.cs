using LoanApproval.Console.Domain.Models;

namespace LoanApproval.Console.Mocks;

public static class LoanApplicationMock
{
    public static LoanApplication Create()
    {
        var loansHistory = new List<Loan>()
        {
            new()
            {
                Amount = 800,
                Term = 12,
                Status = LoanStatus.PaidOff
            },
            new()
            {
                Amount = 600,
                Term = 24,
                Status = LoanStatus.PaidOff
            }
        };

        var customer = new Customer(name: "John Doe", age: 35, loansHistory);
        var loanApplication = new LoanApplication(amount: 1500, term: 12, customer);

        return loanApplication;
    }
}
