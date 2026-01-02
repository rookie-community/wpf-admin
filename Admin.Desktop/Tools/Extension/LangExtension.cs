using Admin.Desktop.Resources.Langs;

namespace Admin.Desktop.Tools.Extension;

public class LangExtension : HandyControl.Tools.Extension.LangExtension
{
    public LangExtension()
    {
        Source = LangProvider.Instance;
    }
}
