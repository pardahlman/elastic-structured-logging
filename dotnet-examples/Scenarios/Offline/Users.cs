namespace Scenarios.Offline
{
  using System;
  using System.Collections.Generic;

  public class Users
  {
    public static readonly User ObiWan = new User
    {
      FirstName = "Obi-Wan",
      LastName = "Kenobi",
      Email = "obi-one@example.com",
      UserId = Guid.NewGuid(),
      Age = 86
    };

    public static readonly User Luke = new User
    {
      FirstName = "Luke",
      LastName = "Skywalker",
      Email = "luke@example.com",
      UserId = Guid.NewGuid(),
      Age = 66
    };

    public static readonly User Leia = new User
    {
      FirstName = "Leia",
      LastName = "Organa",
      Email = "luke@example.com",
      UserId = Guid.NewGuid(),
      Age = 66
    };

    public static readonly User Palpatine = new User
    {
      FirstName = "Emperor",
      LastName = "Palpatine",
      Email = "emporer@darkside.com",
      UserId = Guid.NewGuid(),
      Age = 101
    };

    public static readonly User JarJar = new User
    {
      FirstName = "Jar Jar",
      LastName = "Binks",
      Email = "the.jar@naaboo.com",
      UserId = Guid.NewGuid(),
      Age = 35
    };

    public static readonly User HanSolo = new User
    {
      FirstName = "Han",
      LastName = "Solo",
      Email = "han@so.lo",
      UserId = Guid.NewGuid(),
      Age = 40
    };

    public static readonly User KyloRen = new User
    {
      FirstName = "Kylo",
      LastName = "Ren",
      Email = "kylo@example.com",
      UserId = Guid.NewGuid(),
      Age = 34
    };

    public static List<User> All => new List<User> {HanSolo, JarJar, KyloRen, Leia, Luke, ObiWan, Palpatine};
  }
}
