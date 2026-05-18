using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class SwallowSteps() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            var card = NinjaHelper.LastCard(this);
            return card != null && card.Tags.Contains(NinjaTags.Ninjutsu);
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(1), new ExtraCards(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FrzMudSwallow.mp3");
        await CommonActions.Draw(this, choiceContext);
        var lastCard = NinjaHelper.LastCard(this);
        if (lastCard == null || !lastCard.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return;
        }
        await Cmd.Wait(0.5f);
        NinjaAudio.Play("res://LexNinja2/audio/Running.mp3");
        await NinjaHelper.DrawExtra(this, choiceContext);
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
        DynamicVars.ExtraCard().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"SwallowSteps_p.png".BigCardImagePath();
    public override string PortraitPath => $"SwallowSteps.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SwallowSteps.png".CardImagePath();
}
