# Environment Variable Dotnet

Repository Nuget: https://www.nuget.org/packages/braspin_environment_variable_dotnet

## Install Nuget Package

`` dotnet add package braspin_environment_variable_dotnet --version 0.1.6 ``

### Example class AppSettings.cs inheriting IEnvironmentVariable

```csharp

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

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddEnvironmentVariable<AppSettings>();
    //...
}

```

### Using in yours Controllers

```csharp
//...

private readonly AppSettings _config;

public WeatherForecastController(AppSettings config)
{
    _config = config;
}

```

## Annotation Types

### Boolean variable without default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_BOOLEAN")]
    public bool Boolean { get; set; };
}

``` 

### Boolean variable with default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_BOOLEAN", true)]
    public bool Boolean { get; set; };
}

``` 

### Long variable without default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_LONG")]
    public long Long { get; set; };
}

``` 

### Long variable with default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_LONG", 100)]
    public long Long { get; set; };
}

``` 

### Long variable with default, min and max value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_LONG", 100, 0, 200)]
    public long Double { get; set; };
}

``` 

### Double variable without default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_DOUBLE")]
    public double Double { get; set; };
}

``` 

### Double variable with default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_DOUBLE", 10.1)]
    public double Double { get; set; };
}

``` 

### Double variable with default, min and max value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_DOUBLE", 10.1, 0, 20)]
    public double Double { get; set; };
}

``` 

### String variable without default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_STRING")]
    public string String { get; set; };
}

``` 

### String variable with default value

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_STRING", "Title")]
    public string String { get; set; };
}

```

### String variable with default value and enums values

```csharp

public class AppSettings : IEnvironmentVariable
{
    [EnvironmentVariable("VARIABLE_LOG_LEVEL", "Info", new string[]{"Trace", "Debug", "Info", "Warning", "Error"})]
    public string LogLevel { get; set; };
}

```
