namespace Admin.Desktop.Resources.Langs
{
    public partial class LangProviderTemplate
    {
        public string LangResourceKey { get; set; }

        public IList<string> PropertyNames { get; set; } = new List<string>();

        public IReadOnlyDictionary<string, string> LangDicts { get; set; } = new Dictionary<string, string>();

        public LangProviderTemplate(string resourceKey, Dictionary<string, string> langDicts)
        {
            LangResourceKey = resourceKey;
            PropertyNames = langDicts.OrderBy(x => x.Key).Select(x => x.Key).ToList();
            LangDicts = langDicts;
        }
    }
}
