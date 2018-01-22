using System;

namespace Scenarios.Extensions
{
  public static class EnumUtil
  {
    private static readonly Random Random = new Random();

    public static TEnum OneAtRandom<TEnum>() where TEnum : struct, IConvertible
    {
      if (!typeof(TEnum).IsEnum)
      {
        throw new ArgumentException("T must be an enumerated type");
      }
      var available = Enum.GetValues(typeof(TEnum));
      var availableCount = available.Length;
      var picked = Random.Next(availableCount - 1);
      return (TEnum)available.GetValue(picked);
    }
  }
}
