using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LexNinja2.LexNinja2Code.Cards;

public class DisappointedAfterBocchi()
    : LexNinja2Card(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DisappointedAfterBocchi.mp3");
        await PowerCmd.Apply<GigantificationPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<DisappointedPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }

    public override string CustomPortraitPath =>
        $"DisappointedAfterBocchi_p.png".BigCardImagePath();
    public override string PortraitPath => $"DisappointedAfterBocchi.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DisappointedAfterBocchi.png".CardImagePath();
}
