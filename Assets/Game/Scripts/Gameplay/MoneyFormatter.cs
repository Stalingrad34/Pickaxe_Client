using UnityEngine;

namespace Game.Scripts.Gameplay
{
  public static class MoneyFormatter
  {
    private static readonly string[] Suffixes =
    {
      "",   // 1
      "K",  // thousand
      "M",  // million
      "B",  // billion
      "T",  // trillion
      "Q",  // quadrillion
      "S",  // sextillion / твой игровой следующий ранг
      "O",
      "N",
      "D"
    };

    public static string Format(long value)
    {
      if (value < 1000)
        return Mathf.FloorToInt(value).ToString();

      int suffixIndex = 0;

      while (value >= 1000 && suffixIndex < Suffixes.Length - 1)
      {
        value /= 1000;
        suffixIndex++;
      }

      if (value >= 100)
        return $"{value:0}{Suffixes[suffixIndex]}";

      if (value >= 10)
        return $"{value:0.#}{Suffixes[suffixIndex]}";

      return $"{value:0.##}{Suffixes[suffixIndex]}";
    }
  }
}