using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace LexNinja2.LexNinja2Code.Powers;

public class ScarePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "ScarePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "ScarePower84.png".BigPowerImagePath();

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        if (player != Owner.Player)
            return;
        NinjaAudio.Play("res://LexNinja2/audio/吓我一跳释放忍术.mp3");
        for (var i = 0; i < Amount; i++)
        {
            var card = CardFactory
                .GetDistinctForCombat(
                    Owner.Player,
                    Owner
                        .Player.Character.CardPool.GetUnlockedCards(
                            Owner.Player.UnlockState,
                            Owner.Player.RunState.CardMultiplayerConstraint
                        )
                        .Where(
                            c => c.Tags.Contains(NinjaTags.Ninjutsu)
                        ),
                    1,
                    Owner.Player.RunState.Rng.CombatCardGeneration
                )
                .FirstOrDefault();
            switch (card)
            {
                case null:
                    return;
                case LexNinja2Card lexNinjaCard:
                    lexNinjaCard.SetLexkelaToFreeUntilPlayed();
                    break;
            }

            // if (!(Owner.HasPower<WePeacePower>()&&card.Type==CardType.Attack))
            // {
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(), null);
            // }
        }
    }
}
