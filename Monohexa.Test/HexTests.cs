// SPDX-FileCopyrightText: 2023 The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

namespace Monohexa.Test;

public class HexTests {
  [Theory]
  [InlineData(3)]
  [InlineData(5)]
  [InlineData(7)]
  public void Testa2(int x) {
    Assert.True(IsaOdd(x));
  }

  public static bool IsaOdd(int x) {
    return x % 2 == 1;
  }

  [Fact]
  public void TestHexEquality_TrueWhenEqual() {
    Assert.True(new Hex(0, 0, 0).Equals(new Hex(0, 0, 0)));
    Assert.True(new Hex(1, -1, 0).Equals(new Hex(1, -1, 0)));
  }

  [Fact]
  public void TestHexEquality_FalseWhenNotEqual() {
    Assert.False(new Hex(0, 0, 0).Equals(new Hex(1, 0, -1)));
    Assert.False(new Hex(1, -1, 0).Equals(new Hex(1, 0, -1)));
  }

  [Fact]
  public void TestHexArithmetic() {
    Assert.True(new Hex(4, -10, 6).Equals(new Hex(1, -3, 2).Add(new Hex(3, -7, 4))));
    Assert.True(new Hex(-2, 4, -2).Equals(new Hex(1, -3, 2).Subtract(new Hex(3, -7, 4))));
  }

  [Fact]
  public void TestHexDirection() {
    Assert.True(Hex.Direction(2).Equals(new Hex(0, -1, 1)));
  }

  [Fact]
  public void TestHexNeighbor() {
    HexTestHelpers.EqualHex("hex_neighbor", new Hex(1, -3, 2), new Hex(1, -2, 1).Neighbor(2));
  }

  [Fact]
  public void TestHexDiagonal() {
    HexTestHelpers.EqualHex("hex_diagonal", new Hex(-1, -1, 2), new Hex(1, -2, 1).DiagonalNeighbor(3));
  }

  [Fact]
  public void TestHexDistance() {
    HexTestHelpers.EqualInt("hex_distance", 7, new Hex(3, -7, 4).Distance(new Hex(0, 0, 0)));
  }

  [Fact]
  public void TestHexRotateRight() {
    HexTestHelpers.EqualHex("hex_rotate_right", new Hex(1, -3, 2).RotateRight(), new Hex(3, -2, -1));
  }

  [Fact]
  public void TestHexRotateLeft() {
    HexTestHelpers.EqualHex("hex_rotate_left", new Hex(1, -3, 2).RotateLeft(), new Hex(-2, -1, 3));
  }

  [Fact]
  public void TestHexRound() {
    FractionalHex a = new FractionalHex(0.0, 0.0, 0.0);
    FractionalHex b = new FractionalHex(1.0, -1.0, 0.0);
    FractionalHex c = new FractionalHex(0.0, -1.0, 1.0);
    HexTestHelpers.EqualHex("hex_round 1", new Hex(5, -10, 5), new FractionalHex(0.0, 0.0, 0.0).HexLerp(new FractionalHex(10.0, -20.0, 10.0), 0.5).HexRound());
    HexTestHelpers.EqualHex("hex_round 2", a.HexRound(), a.HexLerp(b, 0.499).HexRound());
    HexTestHelpers.EqualHex("hex_round 3", b.HexRound(), a.HexLerp(b, 0.501).HexRound());
    HexTestHelpers.EqualHex("hex_round 4", a.HexRound(), new FractionalHex((a.q * 0.4) + (b.q * 0.3) + (c.q * 0.3), (a.r * 0.4) + (b.r * 0.3) + (c.r * 0.3), (a.s * 0.4) + (b.s * 0.3) + (c.s * 0.3)).HexRound());
    HexTestHelpers.EqualHex("hex_round 5", c.HexRound(), new FractionalHex((a.q * 0.3) + (b.q * 0.3) + (c.q * 0.4), (a.r * 0.3) + (b.r * 0.3) + (c.r * 0.4), (a.s * 0.3) + (b.s * 0.3) + (c.s * 0.4)).HexRound());
  }

  [Fact]
  public void TestHexLerp() {
    HexTestHelpers.EqualHex("hex_lerp", new Hex(5, -10, 5), new FractionalHex(0.0, 0.0, 0.0).HexLerp(new FractionalHex(10.0, -20.0, 10.0), 0.5).HexRound());
  }

  [Fact]
  public void TestHexLineDraw() {
    HexTestHelpers.EqualHexArray("hex_linedraw", new List<Hex> { new Hex(0, 0, 0), new Hex(0, -1, 1), new Hex(0, -2, 2), new Hex(1, -3, 2), new Hex(1, -4, 3), new Hex(1, -5, 4) }, FractionalHex.HexLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4)));
  }

  [Fact]
  public void TestLayout() {
    Hex h = new Hex(3, 4, -7);
    Layout flat = new Layout(Layout.flat, new Point(10.0, 15.0), new Point(35.0, 71.0));
    HexTestHelpers.EqualHex("layout", h, flat.PixelToHex(flat.HexToPixel(h)).HexRound());
    Layout pointy = new Layout(Layout.pointy, new Point(10.0, 15.0), new Point(35.0, 71.0));
    HexTestHelpers.EqualHex("layout", h, pointy.PixelToHex(pointy.HexToPixel(h)).HexRound());
  }

  [Fact]
  public void TestOffsetRoundTrip() {
    Hex a = new Hex(3, 4, -7);
    OffsetCoord b = new OffsetCoord(1, -3);
    HexTestHelpers.EqualHex("conversion_roundtrip even-q", a, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, a)));
    HexTestHelpers.EqualOffsetcoord("conversion_roundtrip even-q", b, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, b)));
    HexTestHelpers.EqualHex("conversion_roundtrip odd-q", a, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, a)));
    HexTestHelpers.EqualOffsetcoord("conversion_roundtrip odd-q", b, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, b)));
    HexTestHelpers.EqualHex("conversion_roundtrip even-r", a, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, a)));
    HexTestHelpers.EqualOffsetcoord("conversion_roundtrip even-r", b, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, b)));
    HexTestHelpers.EqualHex("conversion_roundtrip odd-r", a, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, a)));
    HexTestHelpers.EqualOffsetcoord("conversion_roundtrip odd-r", b, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, b)));
  }

  [Fact]
  public void TestOffsetFromCube() {
    HexTestHelpers.EqualOffsetcoord("offset_from_cube even-q", new OffsetCoord(1, 3), OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, new Hex(1, 2, -3)));
    HexTestHelpers.EqualOffsetcoord("offset_from_cube odd-q", new OffsetCoord(1, 2), OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, new Hex(1, 2, -3)));
  }

  [Fact]
  public void TestOffsetToCube() {
    HexTestHelpers.EqualHex("offset_to_cube even-", new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, new OffsetCoord(1, 3)));
    HexTestHelpers.EqualHex("offset_to_cube odd-q", new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.ODD, new OffsetCoord(1, 2)));
  }

  [Fact]
  public void TestDoubledRoundtrip() {
    Hex a = new Hex(3, 4, -7);
    DoubledCoord b = new DoubledCoord(1, -3);
    HexTestHelpers.EqualHex("conversion_roundtrip doubled-q", a, DoubledCoord.QdoubledFromCube(a).QdoubledToCube());
    HexTestHelpers.EqualDoubledcoord("conversion_roundtrip doubled-q", b, DoubledCoord.QdoubledFromCube(b.QdoubledToCube()));
    HexTestHelpers.EqualHex("conversion_roundtrip doubled-r", a, DoubledCoord.RdoubledFromCube(a).RdoubledToCube());
    HexTestHelpers.EqualDoubledcoord("conversion_roundtrip doubled-r", b, DoubledCoord.RdoubledFromCube(b.RdoubledToCube()));
  }

  [Fact]
  public void TestDoubledFromCube() {
    HexTestHelpers.EqualDoubledcoord("doubled_from_cube doubled-q", new DoubledCoord(1, 5), DoubledCoord.QdoubledFromCube(new Hex(1, 2, -3)));
    HexTestHelpers.EqualDoubledcoord("doubled_from_cube doubled-r", new DoubledCoord(4, 2), DoubledCoord.RdoubledFromCube(new Hex(1, 2, -3)));
  }

  [Fact]
  public void TestDoubledToCube() {
    HexTestHelpers.EqualHex("doubled_to_cube doubled-q", new Hex(1, 2, -3), new DoubledCoord(1, 5).QdoubledToCube());
    HexTestHelpers.EqualHex("doubled_to_cube doubled-r", new Hex(1, 2, -3), new DoubledCoord(4, 2).RdoubledToCube());
  }
}

internal static class HexTestHelpers {
  public static void EqualHex(string name, Hex a, Hex b) {
    if (!(a.q == b.q && a.s == b.s && a.r == b.r)) {
      Complain(name);
    }
  }

  public static void EqualOffsetcoord(String name, OffsetCoord a, OffsetCoord b) {
    if (!(a.col == b.col && a.row == b.row)) {
      Complain(name);
    }
  }

  public static void EqualDoubledcoord(String name, DoubledCoord a, DoubledCoord b) {
    if (!(a.col == b.col && a.row == b.row)) {
      Complain(name);
    }
  }

  public static void EqualInt(String name, int a, int b) {
    if (a != b) {
      Complain(name);
    }
  }

  public static void EqualHexArray(String name, List<Hex> a, List<Hex> b) {
    EqualInt(name, a.Count, b.Count);
    for (int i = 0; i < a.Count; i++) {
      EqualHex(name, a[i], b[i]);
    }
  }

  public static void Complain(String name) {
    throw new Exception(name);
  }
}
