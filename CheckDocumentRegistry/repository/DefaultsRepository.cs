using System.Text.Json;
using System.Text.Json.Serialization;

// https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0

namespace CheckDocumentRegistry
{
    internal class DefaultsRepository
    {
        public string doSpreadSheetPath { get; }
        public string uppSpreadSheetPath { get; }
        public string matchedDoPath { get; }
        public string matchedUppPath { get; }
        public string unMatchedDoPath { get; }
        public string unMatchedUppPath { get; }

    }
}
