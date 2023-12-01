using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace braspin
{
    namespace environment_variable
    {
        [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class EnvironmentVariable : Attribute
        {
            public string Name { get; set; }
            public object? Default { get; set; }
            public Type Type { get; set; }

            public EnvironmentVariable(string name, Type type)
            {
                if (type == typeof(int) || type == typeof(short))
                {
                    throw new ArgumentException($"Type {type.Name} not supported, must be using long type");
                }

                if(type == typeof(float))
                {
                    throw new ArgumentException($"Type {type.Name} not supported, must be using double type");
                }

                Name = name;
                Default = null;
                Type = type;
            }


            public EnvironmentVariable(string name, int value)
            {
                Name = name;
                Default = value;
                Type = typeof(int);
            }

            public EnvironmentVariable(string name, long value)
            {
                Name = name;
                Default = value;
                Type = typeof(long);
            }

            public EnvironmentVariable(string name, string value)
            {
                Name = name;
                Default = value;
                Type = typeof(string);
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

                        Type t = attribute.Type;

                        if (value != null)
                        {
                            if (t == typeof(string))
                            {
                                propertyInfo.SetValue(ev, value);
                            }
                            else if (t == typeof(double) || t == typeof(double?))
                            {
                                propertyInfo.SetValue(ev, double.Parse(value));
                            }
                            else if (t == typeof(bool) || t == typeof(bool?))
                            {
                                propertyInfo.SetValue(ev, bool.Parse(value));
                            }
                            else
                            {
                                propertyInfo.SetValue(ev, long.Parse(value));
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(ev, attribute.Default);
                        }
                    }
                }
            }

            public static void AddEnviromentVariable<TEnvironmentVariable>(this IServiceCollection services) where TEnvironmentVariable : IEnvironmentVariable
            {
                TEnvironmentVariable ev = Activator.CreateInstance(typeof(TEnvironmentVariable)) as TEnvironmentVariable ?? throw new ArgumentException("Class TEnvironmentVariable.cs inválida!");

                var type = ev.GetType();

                type.ReadEnvironmentVariables(ref ev);

                services.AddSingleton(ev);
            }
        }
        
    }
}
