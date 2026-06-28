using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class PastHasGoneHand() : LexNinja2Card(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(4, ValueProp.Move), new LexKelaVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust), LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/PastHasGoneHand.mp3");
        var discard = PileType.Discard.GetPile(Owner).Cards;
        var max = discard.Count;
        var exhaustAmount = 0;
        while (discard.Count != 0)
        {
            if (exhaustAmount >= max)
            {
                TalkCmd.Play(
                    new LocString("cards", "LEX_NINJA2_CARD_PAST_HAS_GONE_HAND.talk_failed"),
                    Owner.Creature,
                    VfxColor.Black,
                    VfxDuration.Standard
                );
                break;
            }
            exhaustAmount++;
            await LexKela.Gain(this);
            await CommonActions.CardBlock(this, play);
            await CardCmd.Exhaust(choiceContext, discard[0], skipVisuals: true);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"PastHasGoneHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"PastHasGoneHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/PastHasGoneHand.png".CardImagePath();
}
