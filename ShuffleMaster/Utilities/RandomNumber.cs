using System;
using System.Security.Cryptography;

namespace ShuffleMaster.Utilities;

  public static class RandomNumber
  {
    // Thanks to: https://scottlilly.com/create-better-random-numbers-in-c/
    private static readonly RandomNumberGenerator Rand = RandomNumberGenerator.Create();

    /// <summary>
    /// Returns a "better" random number within the specified range.
    /// </summary>
    /// <param name="minValue">The the inclusive lower bound of the random number to be generated.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
    /// <returns>A random integer that is greater than or equal to the minValue, and less than the maxValue.</returns>
    public static int Next(int minValue, int maxValue)
    {
      var randomNumber = new byte[1];
      Rand.GetBytes(randomNumber);

      var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

      // Ensure the multiplier will always be between 0.0 and *just* under 1.0.
      var multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

      // Add 1 to the range to allow for the rounding done in Math.Floor
      var range = maxValue - minValue + 1;

      var randomValueInRange = Math.Floor(multiplier * range);

      return (int)(minValue + randomValueInRange);
    }

    /// <summary>
    /// Returns a "better" random number less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
    /// <returns></returns>
    public static int Next(int maxValue)
    {
      return Next(0, maxValue - 1);
    }

    /// <summary>
    /// Use when you want a "true" random double -- unseeded.
    /// </summary>
    /// <returns>Value between 0.0 and 1.0, inclusive.</returns>
    public static double NextDouble()
    {
      const double minValue = 0.0;
      const double maxValue = 1.0;
      var randomNumber = new byte[1];
      Rand.GetBytes(randomNumber);

      var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

      // Ensure the multiplier will always be between 0.0 and *just* under 1.0.
      var multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

      // Add 1 to the range to allow for the rounding done in Math.Floor
      var range = maxValue - minValue + 1;

      var randomValueInRange = Math.Floor(multiplier * range);

      return (minValue + randomValueInRange);
    }

    public static bool FlipCoin()
    {
      return NextDouble() > 0.49;
    }
  }