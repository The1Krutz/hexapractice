// SPDX-FileCopyrightText: 2023 The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

// I've modified this from the original, but credit where it's due:
// Generated code -- CC0 -- No Rights Reserved -- http://www.redblobgames.com/grids/hexagons/

using Microsoft.Xna.Framework;

namespace Monohexa.Test;

public class HexTests {
  [Fact]
  public void TestHexConstructorFailsWhenGivenInvalidCoordinates() {
    Assert.Throws<ArgumentException>(() => new Hex(-1, -1, -1));
    Assert.Throws<ArgumentException>(() => new Hex(0, 0, -1));
  }

  [Fact]
  public void TestHexEqualityTrueWhenEqual() {
    Assert.Equal(new Hex(), new Hex(0, 0, 0));
    Assert.Equal(new Hex(1, -1, 0), new Hex(1, -1, 0));

    Assert.True(new Hex(0, 0) == new Hex(0, 0, 0));
    Assert.True(new Hex(1, -1, 0) == new Hex(1, -1, 0));
  }

  [Fact]
  public void TestHexEqualityFalseWhenNotEqual() {
    Assert.NotEqual(new Hex(0, 0, 0), new Hex(1, 0, -1));
    Assert.NotEqual(new Hex(1, -1, 0), new Hex(1, 0, -1));

    Assert.True(new Hex(0, 0, 0) != new Hex(1, 0, -1));
    Assert.True(new Hex(1, -1, 0) != new Hex(1, 0, -1));
  }

  [Fact]
  public void TestHexAddition() {
    Assert.Equal(new Hex(4, -10, 6), new Hex(1, -3, 2) + (new Hex(3, -7, 4)));
  }

  [Fact]
  public void TestHexSubtraction() {
    Assert.Equal(new Hex(-2, 4, -2), new Hex(1, -3, 2) - (new Hex(3, -7, 4)));
  }

  [Fact]
  public void TestHexScalarMultiplication() {
    Assert.Equal(new Hex(6, -14, 8), new Hex(3, -7, 4) * 2);
  }

  [Fact]
  public void TestHexScalarDivision() {
    Assert.Equal(new Hex(3, -7, 4), new Hex(6, -14, 8) / 2);
  }

  [Fact]
  public void TestHexRotateLeft() {
    Assert.Equal(new Hex(-2, -1, 3), new Hex(1, -3, 2).RotateLeft);
  }

  [Fact]
  public void TestHexRotateRight() {
    Assert.Equal(new Hex(3, -2, -1), new Hex(1, -3, 2).RotateRight);
  }

  [Fact]
  public void TestHexDirection() {
    Assert.Equal(new Hex(0, -1, 1), Hex.Direction(2));
  }

  [Fact]
  public void TestHexNeighbor() {
    Assert.Equal(new Hex(1, -3, 2), new Hex(1, -2, 1).Neighbor(2));
  }

  [Fact]
  public void TestHexDiagonalNeighbor() {
    Assert.Equal(new Hex(-1, -1, 2), new Hex(1, -2, 1).DiagonalNeighbor(3));
  }

  [Fact]
  public void TestHexLength() {
    Assert.Equal(7, new Hex(3, -7, 4).Length);
  }

  [Fact]
  public void TestHexDistance() {
    Assert.Equal(7, new Hex(3, -7, 4).Distance(new Hex(0, 0, 0)));
  }
}

public class FractionalHexTests {
  [Fact]
  public void TestHexRound() {
    FractionalHex a = new();
    FractionalHex b = new(1.0f, -1.0f);
    FractionalHex c = new(0.0f, -1.0f, 1.0f);

    Assert.True(new Hex(5, -10, 5) == new FractionalHex(0.0f, 0.0f, 0.0f).HexLerp(new FractionalHex(10.0f, -20.0f, 10.0f), 0.5f).HexRound());
    Assert.True(a.HexRound() == a.HexLerp(b, 0.499f).HexRound());
    Assert.True(b.HexRound() == a.HexLerp(b, 0.501f).HexRound());
    Assert.True(a.HexRound() == new FractionalHex((a.Q * 0.4f) + (b.Q * 0.3f) + (c.Q * 0.3f), (a.R * 0.4f) + (b.R * 0.3f) + (c.R * 0.3f), (a.S * 0.4f) + (b.S * 0.3f) + (c.S * 0.3f)).HexRound());
    Assert.True(c.HexRound() == new FractionalHex((a.Q * 0.3f) + (b.Q * 0.3f) + (c.Q * 0.4f), (a.R * 0.3f) + (b.R * 0.3f) + (c.R * 0.4f), (a.S * 0.3f) + (b.S * 0.3f) + (c.S * 0.4f)).HexRound());
  }

  [Fact]
  public void TestHexLerp() {
    Assert.True(new Hex(5, -10, 5) == new FractionalHex(0.0f, 0.0f, 0.0f).HexLerp(new FractionalHex(10.0f, -20.0f, 10.0f), 0.5f).HexRound());
  }

  [Fact]
  public void TestHexLineDraw() {
    List<Hex> drawline = FractionalHex.HexLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4));
    List<Hex> expected = new() {
      new Hex(0, 0, 0),
      new Hex(0, -1, 1),
      new Hex(0, -2, 2),
      new Hex(1, -3, 2),
      new Hex(1, -4, 3),
      new Hex(1, -5, 4)
    };

    Assert.Equal(expected, drawline);
  }
}

public class OffsetCoordTests {
  [Fact]
  public void TestOffsetRoundTrip() {
    Hex a = new(3, 4, -7);
    OffsetCoord b = new(1, -3);
    Assert.Equal(a, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, a)));
    Assert.Equal(b, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, b)));
    Assert.Equal(a, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, a)));
    Assert.Equal(b, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, b)));
    Assert.Equal(a, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, a)));
    Assert.Equal(b, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, b)));
    Assert.Equal(a, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, a)));
    Assert.Equal(b, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, b)));
  }

  [Fact]
  public void TestOffsetFromCube() {
    Assert.Equal(new OffsetCoord(1, 3), OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, new Hex(1, 2, -3)));
    Assert.Equal(new OffsetCoord(1, 2), OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, new Hex(1, 2, -3)));
  }

  [Fact]
  public void TestOffsetToCube() {
    Assert.Equal(new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, new OffsetCoord(1, 3)));
    Assert.Equal(new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.ODD, new OffsetCoord(1, 2)));
  }
}

public class DoubledCoordTests {
  [Fact]
  public void TestDoubledRoundtrip() {
    Hex a = new(3, 4, -7);
    DoubledCoord b = new(1, -3);
    Assert.Equal(a, DoubledCoord.QdoubledFromCube(a).QdoubledToCube());
    Assert.Equal(b, DoubledCoord.QdoubledFromCube(b.QdoubledToCube()));
    Assert.Equal(a, DoubledCoord.RdoubledFromCube(a).RdoubledToCube());
    Assert.Equal(b, DoubledCoord.RdoubledFromCube(b.RdoubledToCube()));
  }

  [Fact]
  public void TestDoubledFromCube() {
    Assert.Equal(new DoubledCoord(1, 5), DoubledCoord.QdoubledFromCube(new Hex(1, 2, -3)));
    Assert.Equal(new DoubledCoord(4, 2), DoubledCoord.RdoubledFromCube(new Hex(1, 2, -3)));
  }

  [Fact]
  public void TestDoubledToCube() {
    Assert.Equal(new Hex(1, 2, -3), new DoubledCoord(1, 5).QdoubledToCube());
    Assert.Equal(new Hex(1, 2, -3), new DoubledCoord(4, 2).RdoubledToCube());
  }
}

public class LayoutTests {
  [Fact]
  public void TestLayout() {
    Hex h = new(3, 4, -7);
    Layout flat = new(Layout.Flat, new Vector2(10.0f, 15.0f), new Vector2(35.0f, 71.0f));
    Assert.Equal(h, flat.PixelToHex(flat.HexToPixel(h)).HexRound());
    Layout pointy = new(Layout.Pointy, new Vector2(10.0f, 15.0f), new Vector2(35.0f, 71.0f));
    Assert.Equal(h, pointy.PixelToHex(pointy.HexToPixel(h)).HexRound());
  }
}
