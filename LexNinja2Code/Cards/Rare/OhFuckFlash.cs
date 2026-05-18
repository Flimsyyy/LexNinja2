using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class OhFuckFlash() : LexNinja2Card(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new NinjutsuVar(3), new PowerVar<BufferPower>(1), new PowerVar<OhFuckFlashPower>(1)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/OhFuckFlash.mp3");
        await CommonActions.ApplySelf<BufferPower>(choiceContext, this);
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        NinjaAudio.Play("res://LexNinja2/audio/Flash.mp3");
        await CommonActions.ApplySelf<OhFuckFlashPower>(choiceContext, this);
        PlayerCmd.EndTurn(Owner, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Ninjutsu().UpgradeValueBy(-1);
    }

    public override string CustomPortraitPath => $"OhFuckFlash_p.png".BigCardImagePath();
    public override string PortraitPath => $"OhFuckFlash.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/OhFuckFlash.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
