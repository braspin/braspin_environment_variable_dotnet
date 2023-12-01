# Environment Variable Dotnet

### Declare class AppSettings.cs inheriting IEnvironmentVariable

```

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_BOOLEAN", true)]
    public bool Boolean { get; set; } = false;

    [EnvironmentVariable("VARIABLE_LONG", 10000000)]
    public long Long { get; set; }

    [EnvironmentVariable("VARIABLE_STRING", "Title")]
    public string? String { get; set; }

    [EnvironmentVariable("VARIABLE_DOUBLE", 5.1)]
    public double Double { get; set; }
}

```

### Add Startup.cs

```

public void ConfigureServices(IServiceCollection services)
{
    services.AddEnviromentVariable<AppSettings>();
    //...
}

```

### Using in yours Controllers

```
//...

private readonly AppSettings _config;

public WeatherForecastController(AppSettings config)
{
    _config = config;
}

```

