using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Api.DynamicVars;

public class NinjutsuVar : SecondaryResourceVar
{
    public const string Key = "RenShu";

    public NinjutsuVar(decimal baseValue)
        : base(Key, LexKela.Id, baseValue)
    {
        this.WithTooltip(_ => HoverTipFactory.FromKeyword(NinjaKeyword.RenShu));
    }
}
