using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class GoodNightPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    private int flag = 0;

    public override string CustomPackedIconPath => "GoodNightPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "GoodNightPower84.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        await CreatureCmd.Heal(Owner, Amount);
        Flash();
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
        {
            return;
        }
        if (flag == 0)
        {
            flag++;
            return;
        }
        await PowerCmd.Remove(this);
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType _)
    {
        return card.Owner != Owner.Player;
    }
}
