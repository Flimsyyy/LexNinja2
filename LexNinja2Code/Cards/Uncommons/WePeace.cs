using BaseLib.Extensions;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class WePeace() : LexNinja2Card(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<IntangiblePower>(1), new NinjutsuVar(1), new PowerVar<WePeacePower>(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<IntangiblePower>(), HoverTipFactory.FromPower<Lexkela>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (!await Ninjutsu(choiceContext, play))
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/WePeace.mp3");
        await CommonActions.ApplySelf<WePeacePower>(choiceContext, this);
        if (!Owner.HasPower<IntangiblePower>())
        {
            await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
        }
        await CardCmd.Exhaust(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Ethereal);
    }

    public override string CustomPortraitPath => $"WePeace_p.png".BigCardImagePath();
    public override string PortraitPath => $"WePeace.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/WePeace.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
