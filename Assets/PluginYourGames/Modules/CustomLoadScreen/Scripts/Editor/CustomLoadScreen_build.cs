#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void CustomLoadScreen()
        {
            string css = FileTextCopy("CustomLoadScreen.css");
            styleFile += $"\n\n\n{css}";

            indexFile = indexFile.Replace(
                "spinner.style.display = \"none\";\n                    progressBarEmpty.style.display = \"\";",
                "spinner.style.display = \"\";\n                    progressBarEmpty.style.display = \"none\";");
            indexFile = indexFile.Replace("spinner.style.display = \"none\";", "spinner.style.display = \"\";");
            indexFile = indexFile.Replace("progressBarEmpty.style.display = \"\";", "progressBarEmpty.style.display = \"none\";");

            indexFile = indexFile.Replace(
                "if (spinner.style.display !== \"none\")",
                "if (loadingCover.style.display !== \"none\")");
        }
    }
}
#endif
