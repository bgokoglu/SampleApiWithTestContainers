namespace Common.Core.BusinessRules;

public interface IBusinessRule
{
    bool IsMet();
    string Error { get; }
}