using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class SandDefendHand()
    : LexNinja2NinjutsuCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(8, ValueProp.Move),
            new NinjutsuVar(1),
            new PowerVar<SandWall>(NinjaHelper.GetValueByChallengeMode(5, 6)),
        ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<SandWall>(), LexKela.HoverTip()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Hand];
    public override bool GainsBlock => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/SandDefendHand.mp3");
        await CommonActions.CardBlock(this, play);
        if (!Ninjutsu(play))
        {
            return;
        }
        await CommonActions.ApplySelf<SandWall>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars.Power<SandWall>().UpgradeValueBy(NinjaHelper.GetValueByChallengeMode(2, 3));
    }

    public override string CustomPortraitPath => $"SandDefendHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"SandDefendHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/SandDefendHand.png".CardImagePath();
}
