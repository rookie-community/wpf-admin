using System.IO;

namespace Admin.Desktop.Resources.Langs
{
    public class LangProviderGenerator
    {
        public static void Generator()
        {
            var resourceType = typeof(Lang);
            //筛选出类型为string的属性名
            var propertyNameList = resourceType.GetProperties()
                 .Where(item => item.PropertyType == typeof(string))
                 .Select(item => item.Name)
                 .ToList();
            var langDicts = new Dictionary<string, string>();
            foreach (var item in propertyNameList)
            {
                // 获取资源字符串
                var resourceValue = Lang.ResourceManager.GetString(item, Lang.Culture);
                // 构建注释
                langDicts[item] = resourceValue ?? string.Empty;
            }

            var temp = new LangProviderTemplate("AdminLangs", langDicts);
            string text = temp.TransformText();
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Resources", "Langs"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(Path.Combine(path, $"{resourceType.Name}Provider.cs"), text);
        }
    }
}
