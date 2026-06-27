using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
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
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        [HoverTipFactory.FromPower<PoisonPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/NamiYoyo.mp3");
        var instance = NCombatRoom.Instance;
        instance?.CombatVfxContainer.AddChildSafely(
            NBolasVfx.Create(Owner.Creature, play.Target!)!
        );
        if (!NinjaConfigsPage.IsChallengeMode())
        {
            await CommonActions.Apply<PoisonPower>(choiceContext, this, play);
        }
        await Cmd.Wait(0.25f);

        var poisonPower = play.Target!.GetPower<PoisonPower>();
        if (poisonPower != null)
        {
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
        if (NinjaConfigsPage.IsChallengeMode())
        {
            await CommonActions.Apply<PoisonPower>(choiceContext, this, play);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Poison.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => $"NamiYoyo_p.png".BigCardImagePath();
    public override string PortraitPath => $"NamiYoyo.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/NamiYoyo.png".CardImagePath();
}
