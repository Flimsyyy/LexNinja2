using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class GodAndBuddha() : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GodAndBuddha.mp3");
        var food = CardFactory
            .GetDistinctForCombat(
                Owner,
                from c in ModelDb
                    .CardPool<TokenCardPool>()
                    .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                where (c.Tags.Contains(NinjaTags.Food))
                select c,
                1,
                Owner.RunState.Rng.CombatCardGeneration
            )
            .FirstOrDefault();
        if (food != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(food, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    public override string CustomPortraitPath => $"GodAndBuddha_p.png".BigCardImagePath();
    public override string PortraitPath => $"GodAndBuddha.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GodAndBuddha.png".CardImagePath();
}
