namespace Game.Scripts.Infrastructure.Custom
{
    public static class CustomButtonSettings
    {
        public enum Style
        {
            Blue,
            Grey,
            Green,
            Orange
        }

        public static CustomButtonStyle GetButtonStyle(Style style)
        {
            return AssetProvider.GetCustomButtonStyle(style);
        }
    }
}