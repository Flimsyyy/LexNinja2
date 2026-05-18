using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class MummyMummy() : LexNinja2Card(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Power", 3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<Lexkela>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/MummyMummy.mp3");
        await PowerCmd.Apply<MummyPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Power"].BaseValue,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"MummyMummy_p.png".BigCardImagePath();
    public override string PortraitPath => $"MummyMummy.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/MummyMummy.png".CardImagePath();
}
