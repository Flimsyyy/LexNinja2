using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class AngrySnakeBite()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<PoisonPower>(7), new NinjutsuVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.AddGeneratedCardToCombat(CreateClone(), PileType.Discard, Owner),
            1f
        );
        if (Ninjutsu(choiceContext))
        {
            NinjaAudio.Play("res://LexNinja2/audio/AngrySnakeBite.mp3");
            VfxCmd.PlayOnCreatureCenter(play.Target!, "vfx/vfx_bite");
            await CommonActionsExtensions.Apply<PoisonPower>(choiceContext, this, play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"AngrySnakeBite.png".BigCardImagePath();
    public override string PortraitPath => $"AngrySnakeBite.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/AngrySnakeBite.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
