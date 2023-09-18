namespace LoanApproval.Console.Domain.Models;

public class Customer
{
    public Customer(string? name, int age, IEnumerable<Loan> loansHistory)
    {
        this.Name = name;
        this.Age = age;
        this.LoansHistory = loansHistory;
    }

    public string? Name { get; }
    
    public int Age { get; }

    public IEnumerable<Loan> LoansHistory { get; } = Enumerable.Empty<Loan>();
}
