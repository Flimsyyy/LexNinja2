using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class MobiusLoopSnake()
    : LexNinja2Card(3, CardType.Skill, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(7)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Snake];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var card = LastCard();
        if (card == null)
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/MobiusLoopSnake.mp3");
        NinjaAudio.Play("res://LexNinja2/audio/Mobius.mp3", 0.15f);
        await Cmd.Wait(1f);
        // if (LastCard().Tags.Contains(NinjaTags.Ninjutsu))
        // {
        //     await PowerCmd.Apply<FreeNinjutsuPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, this);
        // }
        await CardCmd.AutoPlay(choiceContext, card.CreateDupe(), null);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"MobiusLoopSnake.png".BigCardImagePath();
    public override string PortraitPath => $"MobiusLoopSnake.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/MobiusLoopSnake.png".CardImagePath();

    public override async Task AfterPlayerTurnStart(
        PlayerChoiceContext choiceContext,
        Player player
    )
    {
        var pile = Pile;
        if ((pile != null ? (pile.Type != PileType.Exhaust ? 1 : 0) : 1) != 0 || player != Owner)
            return;
        await CardCmd.AutoPlay(choiceContext, this, Owner.Creature);
    }

    private CardModel? LastCard()
    {
        var card = CombatManager
            .Instance.History.CardPlaysFinished.LastOrDefault(
                delegate(CardPlayFinishedEntry e)
                {
                    var flag =
                        e.CardPlay.Card.Owner == Owner
                        && !e.CardPlay.Card.Tags.Contains(NinjaTags.Snake);
                    // if (flag2)
                    // {
                    //     CardType type = e.CardPlay.Card.Type;
                    //     bool flag3 = (uint)(type - 1) <= 1u;
                    //     flag2 = flag3;
                    // }
                    return flag && !e.CardPlay.Card.IsDupe;
                }
            )
            ?.CardPlay.Card;
        return card;
    }

    // public override async Task AfterCardDrawn(
    //     PlayerChoiceContext choiceContext,
    //     CardModel card,
    //     bool fromHandDraw)
    // {
    //     if (Ninjutsu())
    //     {
    //         await CardPileCmd.AutoPlayFromDrawPile(choiceContext, Owner, 1, CardPilePosition.Top, false);
    //     }
    // }
}
