namespace AzureNaming.Tool.Models
{
    public interface IPolicyRuleFactory
    {
        string GetNameValidationRules(List<PolicyRule> policies, char delimeter, PolicyEffects effect = PolicyEffects.Deny);
    }
}