using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class BecomeNong() : LexNinja2Card(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override bool CanBeGeneratedInCombat => false;
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.SingleplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(
            this.Owner.Creature,
            "Cast",
            this.Owner.Character.CastAnimDelay
        );
        CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
        NinjaAudio.Play("res://LexNinja2/audio/BecomeNong.mp3");
        CardModel selectedCard = (
            await CardSelectCmd.FromHand(
                choiceContext,
                this.Owner,
                prefs,
                (Func<CardModel, bool>)null,
                (AbstractModel)this
            )
        ).FirstOrDefault<CardModel>();
        if (selectedCard == null)
        {
            selectedCard = (CardModel)null;
        }
        else
        {
            (
                await PowerCmd.Apply<BecomeNongPower>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    1,
                    this.Owner.Creature,
                    (CardModel)this
                )
            ).SetSelectedCard(selectedCard);
            selectedCard = (CardModel)null;
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"BecomeNong_p.png".BigCardImagePath();
    public override string PortraitPath => $"BecomeNong.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BecomeNong.png".CardImagePath();
}
