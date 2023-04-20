// SPDX-FileCopyrightText: 2023 The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

namespace Monohexa.Test;

public class SampleTests {
  [Fact]
  public void Test1() {
    Assert.Equal(2, Add(1, 1));
  }

  [Theory]
  [InlineData(3)]
  [InlineData(5)]
  [InlineData(7)]
  public void Test2(int x) {
    Assert.True(IsOdd(x));
  }

  public static int Add(int x, int y) {
    return x + y;
  }

  public static bool IsOdd(int x) {
    return x % 2 == 1;
  }
}
