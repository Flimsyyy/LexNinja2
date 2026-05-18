using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Powers;

public class Radiation : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("dian", 0)];

    public override string CustomPackedIconPath => "Radiation32.png".PowerImagePath();
    public override string? CustomBigIconPath => "Radiation84.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != this.Owner.Side)
            return;
        Flash();
        decimal Radiation = (decimal)Owner.MaxHp * Amount / 100;
        await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner, Radiation, false);
    }

    private decimal CalculateMaxHp()
    {
        DynamicVars["dian"].BaseValue = 25 * Amount;
        return DynamicVars["dian"].BaseValue;
    }
}
