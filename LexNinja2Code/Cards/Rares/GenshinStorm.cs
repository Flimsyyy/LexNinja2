using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Commons;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class GenshinStorm()
    : LexNinja2Card(2, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<HolyLittleStorm>(true), HoverTipFactory.FromPower<Lexkela>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [NinjaKeyword.Hand, NinjaKeyword.Blade, CardKeyword.Exhaust];
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsuX();
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.MultiplayerOnly;
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override bool HasLexKelaCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/GenshinStorm.mp3");
        IEnumerable<Creature> players =
            from c in CombatState?.GetTeammatesOf(Owner.Creature)
            where c is { IsAlive: true, IsPlayer: true }
            select c;
        var amount = ResolveLexkelaXValue();
        foreach (var player in players)
        {
            if (player == Owner.Creature)
                continue;
            var card = CombatState?.CreateCard<HolyLittleStorm>(player.Player!);
            if (card != null)
            {
                CardCmd.Upgrade(card);
                card.AddKeyword(CardKeyword.Exhaust);
                card.AddKeyword(CardKeyword.Ethereal);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
            }
            if (player != Owner.Creature)
            {
                await PowerCmd.Apply<Lexkela>(
                    choiceContext,
                    player.Player!.Creature,
                    amount,
                    Owner.Creature,
                    this
                );
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"GenshinStorm.png".BigCardImagePath();
    public override string PortraitPath => $"GenshinStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GenshinStorm.png".CardImagePath();
}
