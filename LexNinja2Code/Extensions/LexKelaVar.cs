using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cmd;

public class LexKelaVar : DynamicVar
{
    public const string Key = "Kela";

    // 本地化键，这里设置为大写的Key，也就是"TEST-LEECH"
    public static readonly string LocKey = Key.ToUpperInvariant();

    public LexKelaVar(decimal baseValue)
        : base(Key, baseValue)
    {
        this.WithTooltip(LocKey);
    }
}
