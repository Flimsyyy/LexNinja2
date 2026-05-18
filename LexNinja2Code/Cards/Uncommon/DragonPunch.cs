using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class DragonPunch()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(18, ValueProp.Move), new CardsVar(2), new NinjutsuVar(1)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DragonPunch.mp3");
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_thrash", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (!Ninjutsu(choiceContext))
        {
            return;
        }
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"DragonPunch_p.png".BigCardImagePath();
    public override string PortraitPath => $"DragonPunch.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DragonPunch.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
