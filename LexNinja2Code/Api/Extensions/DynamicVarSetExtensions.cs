using LexNinja2.LexNinja2Code.Api.DynamicVars;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Api.Extensions;

public static class DynamicVarSetExtensions
{
    public static LexKelaVar LexKela(this DynamicVarSet vars)
    {
        return (LexKelaVar)vars[LexKelaVar.Key];
    }

    public static NinjutsuVar Ninjutsu(this DynamicVarSet vars)
    {
        return (NinjutsuVar)vars[NinjutsuVar.Key];
    }
}
