using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace LexNinja2.LexNinja2Code.Character;

[RegisterCharacter]
public class LexNinja2
    : ModCharacterTemplate<LexNinja2CardPool, LexNinja2RelicPool, LexNinja2PotionPool>
{
    public const string CharacterId = "LexNinja2";
    public static readonly Color Color = new("252525");

    public override Color NameColor => Color;
    public override Color EnergyLabelOutlineColor => Color;
    public override Color MapDrawingColor => Color;

    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 68;
    public override int StartingGold => 99;

    public override CharacterAssetProfile AssetProfile =>
        CharacterAssetProfiles.Merge(
            CharacterAssetProfiles.Ironclad(),
            new CharacterAssetProfile(
                Scenes: new CharacterSceneAssetSet(
                    VisualsPath: "res://LexNinja2/scenes/NinjaCharacter.tscn",
                    MerchantAnimPath: "res://LexNinja2/scenes/Ninja_merchant.tscn",
                    RestSiteAnimPath: "res://LexNinja2/scenes/Ninja_rest_site.tscn"
                ),
                Ui: new CharacterUiAssetSet(
                    IconTexturePath: "Ninja2.png".CharacterUiPath(),
                    IconPath: "res://LexNinja2/scenes/Ninja_icon.tscn",
                    CharacterSelectBgPath: "res://LexNinja2/scenes/Ninja_bg.tscn",
                    CharacterSelectIconPath: "char_select_Ninja.png".CharacterUiPath(),
                    CharacterSelectLockedIconPath: "char_select_char_name_locked.png".CharacterUiPath(),
                    MapMarkerPath: "map_marker_char_name.png".CharacterUiPath()
                ),
                Audio: new CharacterAudioAssetSet(
                    DeathSfx: "res://LexNinja2/audio/Cry.mp3",
                    // CharacterSelectSfx: "res://LexNinja2/audio/pick.mp3",
                    CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
                ),
                Multiplayer: new CharacterMultiplayerAssetSet(
                    ArmPointingTexturePath: "Point.png".CharacterUiPath(),
                    ArmRockTexturePath: "Rock.png".CharacterUiPath(),
                    ArmScissorsTexturePath: "Scissor.png".CharacterUiPath(),
                    ArmPaperTexturePath: "Paper.png".CharacterUiPath()
                ),
                VanillaRelicVisualOverrides:
                [
                    new CharacterVanillaRelicVisualOverride(
                        CharacterOwnedVanillaRelicModelId.YummyCookie,
                        new RelicAssetProfile("YummyCookie.png".RelicImagePath())
                    ),
                ]
            )
        );

    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 1f;

    public override bool RequiresEpochAndTimeline => false;

    protected override NCreatureVisuals? TryCreateCreatureVisuals() =>
        RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(
            AssetProfile.Scenes!.VisualsPath!
        );

    public override List<string> GetArchitectAttackVfx() =>
        [
            "vfx/vfx_attack_blunt",
            "vfx/vfx_heavy_blunt",
            "vfx/vfx_attack_slash",
            "vfx/vfx_bloody_impact",
            "vfx/vfx_rock_shatter",
        ];
}
