using NRules;
using NRules.RuleSharp;
using NRules.Samples.RuleSharp;

var repository = new RuleRepository();
repository.AddReference(typeof(LoanApplication).Assembly);

repository.Load(@"LoanApplicationApproval.rul");

var factory = repository.Compile();
var session = factory.CreateSession();

var loansHistory = new List<Loan>()
{
    new()
    {
        Amount = 1000,
        Term = 12,
        Status = LoanStatus.PaidOff
    }
};

var customer = new Customer(name: "John Doe", age: 32, loansHistory);

var loanApplication = new LoanApplication(amount: 2300, term: 12, customer);

session.Insert(loanApplication);

session.Fire();

Console.WriteLine($"Loan application approved: {loanApplication.IsApproved}");