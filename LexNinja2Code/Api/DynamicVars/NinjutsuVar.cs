using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api.DynamicVars;

public class NinjutsuVar : DynamicVar
{
    public const string Key = "Renshu";

    public NinjutsuVar(decimal baseValue)
        : base(Key, baseValue)
    {
        this.WithTooltip(Key.ToUpperInvariant());
    }

    public AbstractModel? GetOwner() => _owner;
}
