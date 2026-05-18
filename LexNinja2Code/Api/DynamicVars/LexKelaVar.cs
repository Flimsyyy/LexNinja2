using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Api.DynamicVars;

public class LexKelaVar : DynamicVar
{
    public const string Key = "Kela";

    public LexKelaVar(decimal baseValue)
        : base(Key, baseValue)
    {
        this.WithTooltip(Key.ToUpperInvariant());
    }
}
