using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class RacingAgainstMessi()
    : LexNinja2NinjutsuCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new CardsVar(2), new ExtraCards(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [LexKela.HoverTip()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/RacingAgainstMessi.mp3");
        await CommonActions.Draw(this, choiceContext);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this || !Ninjutsu(cardPlay))
        {
            return;
        }
        await NinjaHelper.DrawExtra(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraCard().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"RacingAgainstMessi_p.png".BigCardImagePath();
    public override string PortraitPath => $"RacingAgainstMessi.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/RacingAgainstMessi.png".CardImagePath();
}
