using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class ItalySilverSlash()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(4, ValueProp.Move), new RepeatVar(3), new CardsVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<LanBlade>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/ItalySilverSlash.mp3");
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.5f);
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .TargetingRandomOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        for (int i = 0; i < DynamicVars.Cards.BaseValue; ++i)
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(
                    (CardModel)CombatState.CreateCard<LanBlade>(Owner),
                    PileType.Draw,
                    Owner,
                    CardPilePosition.Random
                )
            );
        await MegaCrit.Sts2.Core.Commands.Cmd.Wait(0.5f);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => "ItalySilverSlash.png".BigCardImagePath();
    public override string PortraitPath => "ItalySilverSlash.png".CardImagePath();
    public override string BetaPortraitPath => "beta/ItalySilverSlash.png".CardImagePath();
}
