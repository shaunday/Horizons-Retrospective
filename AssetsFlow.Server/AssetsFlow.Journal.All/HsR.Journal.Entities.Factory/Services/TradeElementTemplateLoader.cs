using HsR.Journal.Entities.Factory.Models;
using System.Text.Json;

namespace HsR.Journal.Entities.Factory.Services
{
    public static class TradeElementTemplateLoader
    {
        private static readonly Dictionary<string, TradeElementTemplate> _templates = new();
        private static readonly string _templatePath = Path.Combine(AppContext.BaseDirectory, "trade-elements-templates");

        // CA1869: Cache and reuse JsonSerializerOptions instance
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        static TradeElementTemplateLoader() => LoadTemplates();

        private static void LoadTemplates()
        {
            foreach (var actionType in Enum.GetNames<TradeActionType>())
            {
                var filePath = Path.Combine(_templatePath, $"{actionType.ToLower()}-template.json");
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Template file not found for {actionType}: {filePath}", filePath);

                var template = JsonSerializer.Deserialize<TradeElementTemplate>(File.ReadAllText(filePath), _jsonOptions);
                if (template != null)
                    _templates[actionType] = template;
            }
        }

        // Only returns the template, no element creation
        public static TradeElementTemplate GetTemplate(TradeActionType type)
        {
            if (!_templates.TryGetValue(type.ToString(), out var template))
                throw new ArgumentException($"Template not found for element type: {type}");

            return template;
        }
    }
}
