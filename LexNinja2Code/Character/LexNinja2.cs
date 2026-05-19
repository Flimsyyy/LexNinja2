using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Basics;
using LexNinja2.LexNinja2Code.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Character;

public class LexNinja2 : PlaceholderCharacterModel
{
    public const string CharacterId = "LexNinja2";
    public override string CustomVisualPath => "res://LexNinja2/scenes/NinjaCharacter.tscn";
    public static readonly Color Color = new("252525");
    public override float CastAnimDelay => 1f;

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 68;

    public override IEnumerable<CardModel> StartingDeck =>
        [
            ModelDb.Card<StrikeNinja>(),
            ModelDb.Card<StrikeNinja>(),
            ModelDb.Card<StrikeNinja>(),
            ModelDb.Card<StrikeNinja>(),
            ModelDb.Card<DefendNinja>(),
            ModelDb.Card<DefendNinja>(),
            ModelDb.Card<DefendNinja>(),
            ModelDb.Card<DefendNinja>(),
            ModelDb.Card<YiCut>(),
            ModelDb.Card<ShakeShakeHands>(),
        ];

    public override IReadOnlyList<RelicModel> StartingRelics => [ModelDb.Relic<LotusBox>()];

    public override CardPoolModel CardPool => ModelDb.CardPool<LexNinja2CardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<LexNinja2RelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<LexNinja2PotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    //public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomIconTexturePath => "Ninja2.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath =>
        "char_select_Ninja.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath =>
        "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectBg => "res://LexNinja2/scenes/Ninja_bg.tscn";
    public override string CustomIconPath => "res://LexNinja2/scenes/Ninja_icon.tscn";
    public override string CustomMerchantAnimPath => "res://LexNinja2/scenes/Ninja_merchant.tscn";
    public override string CustomRestSiteAnimPath => "res://LexNinja2/scenes/Ninja_rest_site.tscn";

    //石头剪刀布
    public override string CustomArmPaperTexturePath => "Paper.png".CharacterUiPath();
    public override string CustomArmRockTexturePath => "Rock.png".CharacterUiPath();
    public override string CustomArmScissorsTexturePath => "Scissors.png".CharacterUiPath();
    public override string CustomArmPointingTexturePath => "Point.png".CharacterUiPath();

    //音效
    // public override string CustomAttackSfx => "blunt_attack.mp3";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    // public override string CustomDeathSfx => "res://LexNinja2/audio/Cry.mp3";
    public override string CharacterSelectSfx => "res://LexNinja2/audio/pick.mp3";
    // public override Task BeforeDeath(Creature creature)
    // {
    //     if (creature != this.creatu)
    //         return Task.CompletedTask;
    //     NinjaAudio.Play("res://LexNinja2/audio/Cry.mp3");
    //     return Task.CompletedTask;
    // }
    // SpireField<Godot.Node, Func<string[],bool?>> spireField =
    //     (SpireField<Godot.Node, Func<string[],bool?>>)AccessTools.Field(typeof(CustomAnimation), "_animHandler").GetValue(null);//object
    // spireField[this] = (_ => null);
}
