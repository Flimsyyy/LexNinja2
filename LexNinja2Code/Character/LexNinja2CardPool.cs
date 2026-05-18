using BaseLib.Abstracts;
using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;

namespace LexNinja2.LexNinja2Code.Character;

public class LexNinja2CardPool : CustomCardPoolModel
{
    public override string Title => LexNinja2.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/NINJAOrb.png".ImagePath();
    public override string TextEnergyIconPath => "charui/energyOrb.png".ImagePath();

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override Color ShaderColor => new(0.33f, 0.33f, 0.33f);

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load LexNinja2/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("252525");

    public override bool IsColorless => false;
}
