using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class HeavenCross() : LexNinja2Card(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>(), HoverTipFactory.FromKeyword(NinjaKeyword.Renshu)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/HeavenCross.mp3");
        new NinjutsuVar(3).ResetToBase();
        await PowerCmd.Apply<HeavenCrossPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            null
        );
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Innate);
    }

    public override string CustomPortraitPath => $"HeavenCross_p.png".BigCardImagePath();
    public override string PortraitPath => $"HeavenCross.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/HeavenCross.png".CardImagePath();
}
