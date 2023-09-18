using LoanApproval.Console.Domain;
using LoanApproval.Console.Domain.Models;
using LoanApproval.Console.Mocks;

LoanApplication loanApplication = LoanApplicationMock.Create();

var loanApplicationEvaluator = new LoanApplicationEvaluator();
loanApplication.IsApproved = loanApplicationEvaluator.ShouldApplicationBeApproved(loanApplication);

Console.WriteLine($"Loan application approved: {loanApplication.IsApproved}");