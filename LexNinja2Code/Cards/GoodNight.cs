using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class GoodNight() : LexNinja2Card(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LexKelaVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GoodNight.mp3");
        float amount = Owner.Creature.MaxHp * 0.3f;
        decimal healPoint = (decimal)amount;
        await PowerCmd.Apply<GoodNightPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            healPoint,
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            this
        );
        PlayerCmd.EndTurn(Owner, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Kela"].UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"GoodNight_p.png".BigCardImagePath();
    public override string PortraitPath => $"GoodNight.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GoodNight.png".CardImagePath();
}
