
using Aran.Panda.Common.Models;
using System.Reflection;

namespace Aran.Panda.Common.Models
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class XmlPathAttribute : Attribute
    {
        public string Path { get; }
        public XmlPathAttribute(string path)
        {
            Path = path;
        }
    }

    public class SettingsModel
    {
        public int Language { get; set; } = 1033;
        public bool AnnexObstalce { get; set; }
        public double Radius { get; set; }
        public string Airport { get; set; } = string.Empty;
        public SettingsCommonModel Interface { get; set; }
        public SettingsCommonModel Report { get; set; }
    }

    public class SettingsCommonModel
    {
        public SettingsUnitAndPrecisionModel Distance { get; set; }
        public SettingsUnitAndPrecisionModel ShortDistance { get; set; }
        public SettingsUnitAndPrecisionModel Height { get; set; }
        public SettingsUnitAndPrecisionModel Speed { get; set; }
        public SettingsUnitAndPrecisionModel DSpeed { get; set; }
        public SettingsUnitAndPrecisionModel Angle { get; set; }
        public SettingsUnitAndPrecisionModel Gradient { get; set; }
    }

    public class SettingsUnitAndPrecisionModel
    {
        public string Unit { get; set; } = string.Empty;
        public double Precision { get; set; }
    }
}



namespace TestProj
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //AlertConfig config = new AlertConfig()
            //    .SetType("Error")
            //    .SetAutoClose(TimeSpan.FromSeconds(4).Seconds);

            //Console.WriteLine($"Alert Type: {config.Type}, Auto Close: {config.AutoCloseDuration}s");

            //AlertConfig config = SweetAlertService.CreateAlert("Warning")
            //    .WithConfig(config =>
            //    {
            //        config.SetAutoClose(5);
            //    });

            //Console.WriteLine($"Alert Type: {config.Type}, Auto Close: {config.AutoCloseDuration}s");




            SettingsModel model = new SettingsModel();
            PrintClassDetails(model);
        }

        public static void PrintClassDetails(object obj, string indent = "")
        {
            if (obj is null) return;

            Type type = obj.GetType();

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Console.WriteLine($"{indent}  Property: {property.Name}, Type: {property.PropertyType.Name}");

                var attributes = property.GetCustomAttributes();
                foreach (Attribute attribute in attributes)
                {
                    Console.WriteLine($"{indent}    Attribute: {attribute.GetType().Name}");
                }

                if (!property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                {
                    object propertyValue = property.GetValue(obj);
                    PrintClassDetails(propertyValue, indent + "    ");
                }
            }
        }
    }
}