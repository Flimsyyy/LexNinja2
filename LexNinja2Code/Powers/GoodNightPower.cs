using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace LexNinja2.LexNinja2Code.Powers;

public class GoodNightPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    private int flag = 0;

    public override string CustomPackedIconPath => "GoodNightPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "GoodNightPower84.png".BigPowerImagePath();

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> creatures,
        ICombatState combatState
    )
    {
        if (side != Owner.Side || Owner.Player == null)
            return;
        var player = Owner.Player;
        await CreatureCmd.Heal(Owner, HealRestSiteOption.GetHealAmount(player));
        Flash();
        await Hook.AfterRestSiteHeal(player.RunState, player, false);
        var rewards = new List<Reward>();
        Hook.ModifyRestSiteHealRewards(player.RunState, player, rewards, false);
        if (rewards.Count != 0 && CombatState.RunState.CurrentRoom is CombatRoom room)
        {
            foreach (var reward in rewards)
            {
                room.AddExtraReward(player, reward);
            }
        }
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
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
