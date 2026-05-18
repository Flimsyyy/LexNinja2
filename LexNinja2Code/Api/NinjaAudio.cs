using Godot;

namespace LexNinja2.LexNinja2Code.Api;

public static class NinjaAudio
{
    private static readonly Dictionary<string, AudioStream> _cache = new();
    private static readonly Dictionary<string, List<AudioStreamPlayer>> _activePlayers = new();

    // 基础播放（音量）
    public static void Play(string path, float volume = 1f)
    {
        PlayInternal(path, volume, 1f, false);
    }

    // 可调音调播放
    public static void Play(string path, float volume, float pitch)
    {
        PlayInternal(path, volume, pitch, false);
    }

    // 循环播放（可调音量、音调）
    public static void PlayLooped(string path, float volume = 1f, float pitch = 1f)
    {
        PlayInternal(path, volume, pitch, true);
    }

    private static void PlayInternal(string path, float volume, float pitch, bool loop)
    {
        if (!_cache.TryGetValue(path, out var stream))
        {
            stream = GD.Load<AudioStream>(path);
            if (stream != null)
                _cache[path] = stream;
        }

        if (stream == null)
        {
            GD.PrintErr($"Failed to load audio stream at path: {path}");
            return;
        }

        var player = new AudioStreamPlayer
        {
            Stream = stream,
            VolumeDb = ToDb(volume),
            PitchScale = pitch,
            Autoplay = true,
        };

        var tree = Engine.GetMainLoop() as SceneTree;
        tree?.Root.AddChild(player);

        // 登记播放器以便停止
        if (!_activePlayers.ContainsKey(path))
            _activePlayers[path] = new List<AudioStreamPlayer>();
        _activePlayers[path].Add(player);

        if (loop)
        {
            // 循环播放：每次播放结束后重新播放
            player.Finished += () =>
            {
                if (GodotObject.IsInstanceValid(player) && !player.Playing)
                    player.Play();
            };
        }
        else
        {
            // 非循环：播放完毕后自动清理
            player.Finished += () =>
            {
                if (_activePlayers.TryGetValue(path, out var list))
                {
                    list.Remove(player);
                    if (list.Count == 0)
                        _activePlayers.Remove(path);
                }
                player.QueueFree();
            };
        }
    }

    // 立即停止
    public static void Stop(string path)
    {
        StopInternal(path, 0f);
    }

    // 淡出停止
    public static void Stop(string path, float fadeDuration)
    {
        StopInternal(path, fadeDuration);
    }

    private static void StopInternal(string path, float fadeDuration)
    {
        if (!_activePlayers.TryGetValue(path, out var list))
            return;

        foreach (var player in list.ToList())
        {
            // 断开所有事件，避免循环重新触发
            player.Finished += null;

            if (fadeDuration > 0f)
            {
                Tween tween = player.CreateTween();
                tween.TweenMethod(
                    Callable.From<float>(v => player.VolumeDb = v),
                    player.VolumeDb,
                    -80f,
                    fadeDuration
                );
                tween.TweenCallback(
                    Callable.From(() =>
                    {
                        player.Stop();
                        player.QueueFree();
                    })
                );
            }
            else
            {
                player.Stop();
                player.QueueFree();
            }
        }
        _activePlayers.Remove(path);
    }

    public static void StopAll(float fadeDuration = 0f)
    {
        // 先取得所有正在播放的路径，避免遍历时修改字典
        var paths = _activePlayers.Keys.ToList();
        foreach (var path in paths)
        {
            Stop(path, fadeDuration);
        }
    }

    private static float ToDb(float volume)
    {
        if (volume <= 0f)
            return -80f;
        return Mathf.LinearToDb(volume);
    }
}
