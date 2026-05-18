using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class GetAllHands() : LexNinja2Card(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(2), new PowerVar<GetAllHandsPower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GetAllHands.mp3");
        await CommonActions.ApplySelf<GetAllHandsPower>(choiceContext, this);
        // if (Ninjutsu(choiceContext))
        // {
        //     for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        //     {
        //         CardModel card = CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Keywords.Contains(NinjaKeyword.Hand))), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
        //         if (card == null)
        //             return;
        //         await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, true);
        //     }
        // }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    // protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();

    public override string CustomPortraitPath => $"GetAllHands_p.png".BigCardImagePath();
    public override string PortraitPath => $"GetAllHands.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GetAllHands.png".CardImagePath();
}
