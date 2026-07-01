using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using STS2RitsuLib.Patching.Models;

namespace LexNinja2.LexNinja2Code.Api.Patch;

public class NinjaSelectPatch : IPatchMethod
{
    public static string PatchId => "ninja_select";
    public static string Description => "play sound when selecting ninjas";
    public static bool IsCritical => false;

    public static ModPatchTarget[] GetTargets() =>
        [new(typeof(NCharacterSelectScreen), "SelectCharacter")];

    public static void Prefix(
        NCharacterSelectButton charSelectButton,
        CharacterModel characterModel
    )
    {
        if (characterModel != ModelDb.Character<Character.LexNinja2>())
            return;
        Cmd.Wait(0.3f);
        NinjaAudio.Play("res://LexNinja2/audio/pick.mp3");
    }
}
