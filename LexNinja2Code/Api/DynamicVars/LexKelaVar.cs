using BaseLib.Extensions;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Api.DynamicVars;

public class LexKelaVar : SecondaryResourceVar
{
    public const string Key = "Kela";

    public LexKelaVar(decimal baseValue)
        : base(Key, LexKela.Id, baseValue)
    {
        DynamicVarExtensions.DynamicVarTips[this] = _ => LexKela.HoverTip();
    }
}
