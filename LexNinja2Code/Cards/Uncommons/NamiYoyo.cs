using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class NamiYoyo() : LexNinja2Card(4, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Science];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/NamiYoyo.mp3");
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(
            NBolasVfx.Create(Owner.Creature, play.Target!)!
        );
        await CommonActionsExtensions.Apply<PoisonPower>(choiceContext, this, play);
        await Cmd.Wait(0.25f);

        var poisonPower = play.Target!.GetPower<PoisonPower>();
        if (poisonPower == null)
        {
            return;
        }
        var enemy = CombatState!.HittableEnemies.ToList();
        var target = Owner.RunState.Rng.CombatTargets.NextItem(enemy);
        await PowerCmd.Apply<PoisonPower>(
            choiceContext,
            target!,
            poisonPower.Amount,
            Owner.Creature,
            this
        );
    }

    /*public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == base.Owner && CombatManager.Instance.History.CardPlaysFinished.Any((CardPlayFinishedEntry e) => e.RoundNumber == base.CombatState.RoundNumber - 1 && e.CardPlay.Card == this))
        {
            CardPile? pile = base.Pile;
            if (pile == null || pile.Type != PileType.Hand)
            {
                await CardPileCmd.Add(this, PileType.Hand);
            }
        }
    }*/

    /*protected override PileType GetResultPileType()
    {
        PileType resultPileType = base.GetResultPileType();
        return resultPileType != PileType.Discard ? resultPileType : PileType.Hand;
    }*/

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"NamiYoyo_p.png".BigCardImagePath();
    public override string PortraitPath => $"NamiYoyo.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/NamiYoyo.png".CardImagePath();
}
