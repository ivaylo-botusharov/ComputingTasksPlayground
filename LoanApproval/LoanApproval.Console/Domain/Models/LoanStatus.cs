namespace LoanApproval.Console.Domain.Models;

public enum LoanStatus
{
    PaidOff,
    PaidOffWithDelays,
    Ongoing,
    OngoingWithDelays,
    Defaulted
}
