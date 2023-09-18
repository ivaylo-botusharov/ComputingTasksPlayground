using LoanApproval.RulesEngine;

namespace LoanApproval.Console.Infrastructure;

public class RulesRepository
{
    public IEnumerable<Rule> GetRules()
    {
        var rules = new List<Rule>()
        {
            new Rule()
            {
                Name = "Minimum Age",
                Condition = "Customer.Age >= 18"
            },
            new Rule()
            {
                Name = "Maximum amount",
                Condition = "Amount <= 2000m"
            },
            new Rule()
            {
                Name = "Without defaults",
                Condition = "Customer.LoansHistory.All(x => x.Status != LoanStatus.Defaulted)"
            }
        };

        return rules;
    }
}
