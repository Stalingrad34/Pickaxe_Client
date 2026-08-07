#if YandexGamesPlatform_yg
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuild
    {
        public static void CustomLoadScreen()
        {
            string css = FileTextCopy("CustomLoadScreen.css");
            styleFile += $"\n\n\n{css}";
        }
    }
}
#endif
