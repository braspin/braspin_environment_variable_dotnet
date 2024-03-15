using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.InteropServices;

namespace braspin
{
    namespace environment_variable
    {
        [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class EnvironmentVariable : Attribute
        {
            public string Name { get; set; }
            public object? Default { get; set; }
            public Type? Type { get; set; }
            public object? Enums { get; set; }
            public double? Min { get; set; }
            public double? Max { get; set; }

            public EnvironmentVariable(string name)
            {
                Name = name;
                Default = null;
            }

            public EnvironmentVariable(string name, long value)
            { 
                Name = name;
                Default = value;
                Type = typeof(long);
            }

            public EnvironmentVariable(string name, long value, long[] enums)
            {
                Name = name;
                Default = value;
                Type = typeof(long);
                Enums = enums;
            }

            public EnvironmentVariable(string name, long value, long min, long max)
            {
                Name = name;
                Default = value;
                Type = typeof(long);
                Min = min;
                Max = max;
            }

            public EnvironmentVariable(string name, string value)
            {
                Name = name;
                Default = value;
                Type = typeof(string);
            }

            public EnvironmentVariable(string name, string value, string[] enums)
            {
                Name = name;
                Default = value;
                Type = typeof(string);
                Enums = enums;
            }

            public EnvironmentVariable(string name, bool value)
            {
                Name = name;
                Default = value;
                Type = typeof(bool);
            }

            public EnvironmentVariable(string name, double value)
            {
                Name = name;
                Default = value;
                Type = typeof(double);
            }

            public EnvironmentVariable(string name, double value, double min, double max)
            {
                Name = name;
                Default = value;
                Type = typeof(double);
                Min = min;
                Max = max;
            }
        }

        public static class EnvironmentVariableExtensions
        {
            public static void ReadEnvironmentVariables<TEnvironmentVariable>(this Type type, ref TEnvironmentVariable ev) where TEnvironmentVariable : IEnvironmentVariable
            { 
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    var attribute = propertyInfo.GetCustomAttribute(typeof(EnvironmentVariable), false) as EnvironmentVariable;

                    if (attribute != null) 
                    {
                        string? value = System.Environment.GetEnvironmentVariable(attribute.Name);

                        Type? t = attribute.Type;

                        if(t == null)
                        {
                            t = propertyInfo.PropertyType;
                        }

                        if (value != null)
                        {
                            if (t == typeof(string))
                            {
                                if(attribute.Enums != null)
                                {
                                    string[] enums = (string[]) attribute.Enums;
                                    if(enums.Contains(value) == false)
                                    {
                                        throw new ArgumentException($"Value {value} not contains in {attribute.Name} variable");
                                    }
                                }

                                propertyInfo.SetValue(ev, value);
                            }
                            else if (t == typeof(double) || t == typeof(double?))
                            {
                                var v = double.Parse(value);

                                if (attribute.Max != null && attribute.Min != null)
                                {
                                    if (v < attribute.Min)
                                    {
                                        throw new ArgumentException($"Value {value} minor then {attribute.Min} in {attribute.Name} variable");
                                    }

                                    if (v > attribute.Max)
                                    {
                                        throw new ArgumentException($"Value {value} major then {attribute.Max} in {attribute.Name} variable");
                                    }
                                }

                                propertyInfo.SetValue(ev, v);
                            }
                            else if (t == typeof(bool) || t == typeof(bool?))
                            {
                                propertyInfo.SetValue(ev, bool.Parse(value));
                            }
                            else
                            {
                                var v = long.Parse(value);

                                if (attribute.Enums != null)
                                {
                                    long[] enums = (long[])attribute.Enums;
                                    if (enums.Contains(v) == false)
                                    {
                                        throw new ArgumentException($"Value {value} not contains in {attribute.Name} variable");
                                    }
                                }

                                if(attribute.Max != null && attribute.Min != null)
                                {
                                    if(v < attribute.Min)
                                    {
                                        throw new ArgumentException($"Value {value} minor then {attribute.Min} in {attribute.Name} variable");
                                    }

                                    if (v > attribute.Max)
                                    {
                                        throw new ArgumentException($"Value {value} major then {attribute.Max} in {attribute.Name} variable");
                                    }
                                }

                                propertyInfo.SetValue(ev, v);
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(ev, attribute.Default);
                        }
                    }
                }
            }

            internal static TEnvironmentVariable LoadEnvironmentVariable<TEnvironmentVariable>() where TEnvironmentVariable : IEnvironmentVariable
            {
                TEnvironmentVariable ev = Activator.CreateInstance(typeof(TEnvironmentVariable)) as TEnvironmentVariable ?? throw new ArgumentException("Class TEnvironmentVariable.cs inválida!");

                var type = ev.GetType();

                type.ReadEnvironmentVariables(ref ev);

                return ev;
            }

            public static TEnvironmentVariable AddEnvironmentVariable<TEnvironmentVariable>(this IServiceCollection services) where TEnvironmentVariable : IEnvironmentVariable
            {
                var env = LoadEnvironmentVariable<TEnvironmentVariable>();
                services.AddSingleton(env);
                return env;
            }
        }
        
    }
}
