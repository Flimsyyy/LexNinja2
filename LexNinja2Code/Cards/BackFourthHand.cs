using LexNinja2.LexNinja2Code.Cards;
using LexNinja2.LexNinja2Code.Cmd;
using LexNinja2.LexNinja2Code.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class BackFourthHand()
    : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(8, ValueProp.Move), new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/BackFourthHand.mp3");
        BackFourthHand dredge = this;
        await CreatureCmd.TriggerAnim(
            dredge.Owner.Creature,
            "Cast",
            dredge.Owner.Character.CastAnimDelay
        );
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        int selectCount = Math.Min(
            dredge.DynamicVars.Cards.IntValue,
            10 - PileType.Hand.GetPile(dredge.Owner).Cards.Count
        );
        if (selectCount <= 0)
            return;
        IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add(
            await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                PileType.Discard.GetPile(dredge.Owner).Cards,
                dredge.Owner,
                new CardSelectorPrefs(dredge.SelectionScreenPrompt, selectCount)
            ),
            PileType.Hand
        );
        await CardCmd.Discard(
            choiceContext,
            await CardSelectCmd.FromHandForDiscard(
                choiceContext,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 2),
                (Func<CardModel, bool>)null,
                (AbstractModel)this
            )
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"BackFourthHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"BackFourthHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/BackFourthHand.png".CardImagePath();
}
