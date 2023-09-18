namespace LoanApproval.Console.Domain.Models;

public class Loan
{
    public decimal Amount { get; set; }
    
    public int Term { get; set; }
    
    public LoanStatus Status { get; set; }
}
