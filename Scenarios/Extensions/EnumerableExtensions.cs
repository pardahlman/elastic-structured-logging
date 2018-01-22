using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenarios.Extensions
{
  public static class EnumerableExtensions
  {
    private static readonly Random Random = new Random();

    public static T OneAtRandom<T>(this IEnumerable<T> enumerable)
    {
      enumerable = enumerable.ToList();
      var skipCount = Random.Next(enumerable.Count() - 2);
      return enumerable.Skip(skipCount).FirstOrDefault();
    }
  }
}
