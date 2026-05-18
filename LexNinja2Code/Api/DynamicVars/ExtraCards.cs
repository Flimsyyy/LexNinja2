using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Api.DynamicVars;

public class ExtraCards(decimal amount) : DynamicVar(Key, amount)
{
    public const string Key = "ExtraCards";
}