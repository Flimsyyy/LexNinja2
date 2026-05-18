using BaseLib.Abstracts;
using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Ancients;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Basics;

public class YiCut()
    : LexNinja2Card(0, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies),
        ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(2), new PowerVar<VulnerablePower>(1), new DamageVar(10, ValueProp.Move)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<VulnerablePower>(), HoverTipFactory.FromPower<Lexkela>()];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/YiCut.mp3");
        if (Ninjutsu(choiceContext))
        {
            await CommonActions
                .CardAttack(this, play, vfx: "vfx/vfx_giant_horizontal_slash", tmpSfx: "slash_attack.mp3")
                .Execute(choiceContext);
        }
        await CommonActionsExtensions.Apply<VulnerablePower>(choiceContext, this, play);
    }
    
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();

    protected override void OnUpgrade()
    {
        DynamicVars.Ninjutsu().UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => "YiCut_p.png".BigCardImagePath();
    public override string PortraitPath => "YiCut.png".CardImagePath();
    public override string BetaPortraitPath => "beta/YiCut.png".CardImagePath();

    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<ShadeCrossSlash>();
}
