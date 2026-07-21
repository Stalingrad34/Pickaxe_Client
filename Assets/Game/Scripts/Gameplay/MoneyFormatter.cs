namespace Game.Scripts.Gameplay
{
  public static class MoneyFormatter
  {
    private static readonly string[] Suffixes =
    {
      "",
      "K",
      "M",
      "B",
      "T",
      "Q",
      "S",
      "O",
      "N",
      "D"
    };

    public static string Format(long value)
    {
      if (value < 1000)
        return value.ToString();

      int suffixIndex = 0;
      long divider = 1;

      while (value / divider >= 1000 && suffixIndex < Suffixes.Length - 1)
      {
        divider *= 1000;
        suffixIndex++;
      }

      long whole = value / divider;
      long remainder = value % divider;

      long decimalPart = remainder * 10 / divider;

      if (decimalPart == 0)
        return $"{whole}{Suffixes[suffixIndex]}";

      return $"{whole}.{decimalPart}{Suffixes[suffixIndex]}";
    }
  }
}