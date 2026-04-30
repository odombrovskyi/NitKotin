using System.Text.Json;
using NitKotin.Models;

namespace NitKotin.Services;

public sealed class ConfigService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _configPath;

    public ConfigService(string? configPath = null)
    {
        _configPath = configPath ?? Path.Combine(AppContext.BaseDirectory, "nitkotin.config.json");
    }

    public string ConfigPath => _configPath;

    public SmokingConfig Load()
    {
        if (!File.Exists(_configPath))
        {
            return new SmokingConfig();
        }

        try
        {
            var json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<SmokingConfig>(json, SerializerOptions) ?? new SmokingConfig();
        }
        catch (JsonException)
        {
            return new SmokingConfig();
        }
        catch (IOException)
        {
            return new SmokingConfig();
        }
    }

    public void Save(SmokingConfig config)
    {
        var directory = Path.GetDirectoryName(_configPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(config, SerializerOptions);
        File.WriteAllText(_configPath, json);
    }
}