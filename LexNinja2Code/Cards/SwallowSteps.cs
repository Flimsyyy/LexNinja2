using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Cards;

public class SwallowSteps() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal =>
        LastCard() != null && LastCard().Tags.Contains(NinjaTags.Ninjutsu);
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(1), new DynamicVar("Extra", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Player owner;
        PileType[] pileTypeArray;
        NinjaAudio.Play("res://LexNinja2/audio/FrzMudSwallow.mp3");
        if (LastCard() != null && LastCard().Tags.Contains(NinjaTags.Ninjutsu))
        {
            await MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.5f);
            NinjaAudio.Play("res://LexNinja2/audio/Running.mp3");
            await CardPileCmd.Draw(
                choiceContext,
                DynamicVars.Cards.BaseValue + DynamicVars["Extra"].BaseValue,
                Owner
            );
        }
        else
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
        // await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
        // do
        // {
        //     CardModel cardModel = await CardPileCmd.Draw(choiceContext, Owner);
        //     if (cardModel != null && !cardModel.Tags.Contains(NinjaTags.Ninjutsu) )
        //     {
        //         owner = Owner;
        //         pileTypeArray = new PileType[1]{ PileType.Hand };
        //     }
        //     else
        //         goto label_3;
        // }
        // while (CardPile.GetCards(owner, pileTypeArray).Count<CardModel>() < 10);
        // goto label_6;
        // label_3:
        // return;
        // label_6:;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Extra"].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"SwallowSteps_p.png".BigCardImagePath();
    public override string PortraitPath => $"SwallowSteps.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SwallowSteps.png".CardImagePath();

    private CardModel LastCard()
    {
        CardModel card = CombatManager
            .Instance.History.CardPlaysFinished.LastOrDefault(
                delegate(CardPlayFinishedEntry e)
                {
                    bool flag = e.CardPlay.Card.Owner == base.Owner;
                    bool flag2 = flag;
                    // if (flag2)
                    // {
                    //     CardType type = e.CardPlay.Card.Type;
                    //     bool flag3 = (uint)(type - 1) <= 1u;
                    //     flag2 = flag3;
                    // }
                    return flag2 && !e.CardPlay.Card.IsDupe;
                }
            )
            ?.CardPlay.Card;
        return card;
    }
}
