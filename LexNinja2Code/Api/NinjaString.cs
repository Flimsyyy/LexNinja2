namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaString
{
    public static string AudioPath(this string path)
    {
        return Path.Join(MainFile.ModId, "audio", path);
    }
}
