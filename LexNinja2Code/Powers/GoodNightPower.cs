using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

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
        if (side != Owner.Side)
            return;
        await CreatureCmd.Heal(Owner, Amount);
        Flash();
        if (Owner.Player.GetRelic<RegalPillow>() != null)
        {
            Owner.Player.GetRelic<RegalPillow>().Flash();
            await CreatureCmd.Heal(Owner, 15);
        }
        //if地狱
        if (Owner.Player.GetRelic<DreamCatcher>() != null)
        {
            Owner.Player.GetRelic<DreamCatcher>().Flash();
            AbstractRoom currentRoom = base.CombatState.RunState.CurrentRoom;
            if (currentRoom is CombatRoom combatRoom)
            {
                combatRoom.AddExtraReward(
                    base.Owner.Player,
                    new CardReward(
                        CardCreationOptions.ForRoom(base.Owner.Player, combatRoom.RoomType),
                        3,
                        base.Owner.Player
                    )
                );
            }
        }

        if (Owner.Player.GetRelic<StoneHumidifier>() != null)
        {
            Owner.Player.GetRelic<StoneHumidifier>().Flash();
            await CreatureCmd.GainMaxHp(Owner, 5);
        }

        if (Owner.Player.GetRelic<TinyMailbox>() != null)
        {
            Owner.Player.GetRelic<TinyMailbox>().Flash();
            await PotionCmd.TryToProcure(
                PotionFactory
                    .CreateRandomPotionInCombat(
                        base.Owner.Player,
                        base.Owner.Player.RunState.Rng.CombatPotionGeneration
                    )
                    .ToMutable(),
                base.Owner.Player
            );
            await PotionCmd.TryToProcure(
                PotionFactory
                    .CreateRandomPotionInCombat(
                        base.Owner.Player,
                        base.Owner.Player.RunState.Rng.CombatPotionGeneration
                    )
                    .ToMutable(),
                base.Owner.Player
            );
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
