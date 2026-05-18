using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using LexNinja2.LexNinja2Code.Api.Extensions;

namespace LexNinja2.LexNinja2Code.Powers;

public abstract class LexNinja2Power : CustomPowerModel
{
    //Loads from LexNinja2/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}
