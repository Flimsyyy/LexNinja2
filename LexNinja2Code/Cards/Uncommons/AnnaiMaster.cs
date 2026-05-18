using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class AnnaiMaster() : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<Lexkela>(),
            HoverTipFactory.FromKeyword(NinjaKeyword.Renshu),
            HoverTipFactory.FromKeyword(NinjaKeyword.FreeNinjutsu),
        ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/AnnaiMaster.mp3");
        var card = CardFactory
            .GetDistinctForCombat(
                Owner,
                Owner
                    .Character.CardPool.GetUnlockedCards(
                        Owner.UnlockState,
                        Owner.RunState.CardMultiplayerConstraint
                    )
                    .Where(c => c.Tags.Contains(NinjaTags.Ninjutsu)),
                1,
                Owner.RunState.Rng.CombatCardGeneration
            )
            .FirstOrDefault();
        if (card == null)
            return;
        card.SetToFreeThisTurn();
        card.AddKeyword(NinjaKeyword.FreeNinjutsu);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"AnnaiMaster_p.png".BigCardImagePath();
    public override string PortraitPath => $"AnnaiMaster.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/AnnaiMaster.png".CardImagePath();
}
