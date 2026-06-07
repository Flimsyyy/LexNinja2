using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class BecomeNong() : LexNinja2Card(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BecomeNongPower>(1)];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await NinjaAnim.TriggerCastAnim(this);
        NinjaAudio.Play("res://LexNinja2/audio/BecomeNong.mp3");
        if (base.IsUpgraded)
        {
            var card = await CommonActions.SelectSingleCard(
                this,
                SelectionScreenPrompt,
                choiceContext,
                PileType.Hand
            );
            if (card != null)
            {
                var power = await CommonActions.ApplySelf<BecomeNongPower>(choiceContext, this);
                power?.SetSelectedCard(card);
            }
            return;
        }
        CardPile pile = PileType.Hand.GetPile(base.Owner);
        CardModel? card2 = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (card2 != null)
        {
            var power = await CommonActions.ApplySelf<BecomeNongPower>(choiceContext, this);
            power?.SetSelectedCard(card2);
        }
    }

    protected override void OnUpgrade() { }

    public override string CustomPortraitPath => $"BecomeNong_p.png".BigCardImagePath();
    public override string PortraitPath => $"BecomeNong.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BecomeNong.png".CardImagePath();
}
