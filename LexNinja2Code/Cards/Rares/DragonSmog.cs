using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class DragonSmog() : LexNinja2Card(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<VulnerablePower>(99), new NinjutsuVar(2), new PowerVar<IntangiblePower>(1)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<ArtifactPower>(),
            HoverTipFactory.FromPower<IntangiblePower>(),
        ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DragonSmog.mp3");
        await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            if (enemy.HasPower<ArtifactPower>())
                await PowerCmd.Remove<ArtifactPower>(enemy);
        }
        await CommonActionsExtensions.ApplyToAllEnemies<VulnerablePower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Ninjutsu().UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => $"DragonSmog_p.png".BigCardImagePath();
    public override string PortraitPath => $"DragonSmog.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DragonSmog.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
