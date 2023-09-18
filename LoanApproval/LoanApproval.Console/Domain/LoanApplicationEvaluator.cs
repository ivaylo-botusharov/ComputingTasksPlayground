namespace LoanApproval.Console.Domain;

using System.Linq.Expressions;
using LoanApproval.Console.Domain.Models;
using LoanApproval.Console.Infrastructure;
using LoanApproval.RulesEngine;

public class LoanApplicationEvaluator
{
    public bool ShouldApplicationBeApproved(LoanApplication loanApplication)
    {
        var rulesRepository = new RulesRepository();
        IEnumerable<Rule> rules = rulesRepository.GetRules();

        var customTypeProvider = new CustomTypeProvider();
        customTypeProvider.RegisterCustomTypesFromAssembly(typeof(LoanStatus).Assembly);

        var dslParser = new DSLParser<LoanApplication>();

        Expression<Func<LoanApplication, bool>> combinedExpression = dslParser.Parse(rules, customTypeProvider);
        Func<LoanApplication, bool> compiledPredicate = combinedExpression.Compile();

        bool isApproved = compiledPredicate(loanApplication);

        return isApproved;
    }
}
