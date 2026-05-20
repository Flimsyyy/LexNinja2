using BaseLib.Utils;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards.Rares;

public class GetPeopleTax()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(15, ValueProp.Move), new GoldVar(12), new NinjutsuVar(2)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (await Ninjutsu(choiceContext, play))
        {
            NinjaAudio.Play("res://LexNinja2/audio/GetPeopleTax.mp3");
            var monsterPos = new Vector2?();
            foreach (var enemy in CombatState!.HittableEnemies)
            {
                if (TestMode.IsOff)
                    monsterPos = NCombatRoom.Instance!.GetCreatureNode(enemy)?.VfxSpawnPosition;
                if (monsterPos.HasValue)
                    VfxCmd.PlayVfx(
                        monsterPos.Value,
                        "vfx/vfx_coin_explosion_regular",
                        NCombatRoom.Instance?.CombatVfxContainer
                    );
                await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
            }
        }

        await CommonActions
            .CardAttack(this, play, vfx: "vfx/vfx_attack_slash", tmpSfx: "heavy_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
        DynamicVars.Gold.UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"GetPeopleTax_p.png".BigCardImagePath();
    public override string PortraitPath => $"GetPeopleTax.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/GetPeopleTax.png".CardImagePath();

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
