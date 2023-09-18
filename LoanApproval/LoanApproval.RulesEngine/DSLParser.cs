namespace LoanApproval.RulesEngine;

using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Dynamic.Core.Exceptions;
using System.Linq.Expressions;

public class DSLParser<T>
{
    public Expression<Func<T, bool>> Parse(IEnumerable<Rule> rules, IDynamicLinkCustomTypeProvider customTypeProvider)
    {
        // Define a parameter for the loan application
        var loanAppParam = Expression.Parameter(typeof(T), "loanApp");
        var parsingConfig = new ParsingConfig { CustomTypeProvider = customTypeProvider };

        // Create a list to hold individual expressions
        IList<Expression<Func<T, bool>>> ruleExpressions = new List<Expression<Func<T, bool>>>();

        foreach (var rule in rules)
        {
            try
            {
                // Check if the rule starts with "not " to indicate negation
                if (rule.Condition.TrimStart().StartsWith("not ", StringComparison.OrdinalIgnoreCase))
                {
                    string? negatedCondition = rule.Condition.Substring(4).Trim(); // Remove "not" and trim whitespace

                    Expression? innerExpression = DynamicExpressionParser
                        .ParseLambda(parsingConfig, new[] { loanAppParam }, typeof(bool), negatedCondition)
                        .Body;

                    // Negate the inner expression
                    UnaryExpression? negatedExpression = Expression.Not(innerExpression);

                    ruleExpressions.Add(Expression.Lambda<Func<T, bool>>(negatedExpression, loanAppParam));
                }
                else
                {
                    // No negation, parse the rule condition as-is
                    Expression? ruleExpression = DynamicExpressionParser
                        .ParseLambda(parsingConfig, new[] { loanAppParam }, typeof(bool), rule.Condition)
                        .Body;

                    ruleExpressions.Add(Expression.Lambda<Func<T, bool>>(ruleExpression, loanAppParam));
                }
            }
            catch (ParseException ex)
            {
                throw new ArgumentException("Invalid condition: " + rule.Condition, ex);
            }
        }

        // Combine individual expressions using AndAlso
        var combinedExpression = ruleExpressions.Aggregate(
            (agg, next) => Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(agg.Body, next.Body),
                loanAppParam));

        return combinedExpression;
    }
}
