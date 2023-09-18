namespace LoanApproval.Console.Domain.Models;

public class LoanApplication
{
    public LoanApplication(decimal amount, int term, Customer customer)
    {
        this.Amount = amount;
        this.Term = term;
        this.Customer = customer;
        this.IsApproved = false;
    }

    public decimal Amount { get; set; }

    public int Term { get; set; }

    public Customer Customer { get; set; }

    public bool IsApproved { get; set; }
}
