// SPDX-FileCopyrightText: 2023 The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

// I've modified this from the original, but credit where it's due:
// Generated code -- CC0 -- No Rights Reserved -- http://www.redblobgames.com/grids/hexagons/

namespace Monohexa;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public readonly struct Hex {
  public readonly int Q;
  public readonly int R;
  public readonly int S;

  public Hex() : this(0, 0, 0) { }

  public Hex(int q, int r) : this(q, r, -q - r) { }

  public Hex(int q, int r, int s) {
    if (q + r + s != 0) {
      throw new ArgumentException("q + r + s must be 0");
    }

    Q = q;
    R = r;
    S = s;
  }

  public Hex RotateLeft => new(-S, -Q, -R);

  public Hex RotateRight => new(-R, -S, -Q);

  public static readonly List<Hex> Directions = new() {
    new Hex(1, 0, -1),
    new Hex(1, -1, 0),
    new Hex(0, -1, 1),
    new Hex(-1, 0, 1),
    new Hex(-1, 1, 0),
    new Hex(0, 1, -1)
  };

  public static Hex Direction(int direction) {
    return Directions[direction];
  }

  public Hex Neighbor(int direction) {
    return this + Direction(direction);
  }

  public static readonly List<Hex> Diagonals = new() {
    new Hex(2, -1, -1),
    new Hex(1, -2, 1),
    new Hex(-1, -1, 2),
    new Hex(-2, 1, 1),
    new Hex(-1, 2, -1),
    new Hex(1, 1, -2)
  };

  public Hex DiagonalNeighbor(int direction) {
    return this + Diagonals[direction];
  }

  public int Length => (int)((MathF.Abs(Q) + MathF.Abs(R) + MathF.Abs(S)) / 2);

  public int Distance(Hex b) => (this - b).Length;

  // operators

  public static Hex operator +(Hex a, Hex b) => new(a.Q + b.Q, a.R + b.R, a.S + b.S);

  public static Hex operator -(Hex a, Hex b) => new(a.Q - b.Q, a.R - b.R, a.S - b.S);

  public static Hex operator *(Hex a, int k) => new(a.Q * k, a.R * k, a.S * k);

  /// <summary>
  /// Divides by k. Be careful, this is integer division. Rounding is likely
  /// </summary>
  public static Hex operator /(Hex a, int k) => new(a.Q / k, a.R / k, a.S / k);

  public static bool operator ==(Hex a, Hex b) => a.Equals(b);

  public static bool operator !=(Hex a, Hex b) => !(a == b);

  public override bool Equals(object obj) {
    if (obj == null || GetType() != obj.GetType()) {
      return false;
    }

    Hex other = (Hex)obj;

    return Q == other.Q && R == other.R && S == other.S;
  }

  public override int GetHashCode() => Q.GetHashCode() ^ R.GetHashCode() ^ S.GetHashCode();
}

public readonly struct FractionalHex {
  public readonly float Q;
  public readonly float R;
  public readonly float S;

  public FractionalHex() : this(0, 0, 0) { }

  public FractionalHex(float q, float r) : this(q, r, -q - r) { }

  public FractionalHex(float q, float r, float s) {
    if (MathF.Round(q + r + s) != 0) {
      throw new ArgumentException("q + r + s must be 0");
    }

    Q = q;
    R = r;
    S = s;
  }

  public Hex HexRound() {
    int qi = (int)MathF.Round(Q);
    int ri = (int)MathF.Round(R);
    int si = (int)MathF.Round(S);

    float dq = MathF.Abs(qi - Q);
    float dr = MathF.Abs(ri - R);
    float ds = MathF.Abs(si - S);

    if (dq > dr && dq > ds) {
      qi = -ri - si;
    } else {
      if (dr > ds) {
        ri = -qi - si;
      } else {
        si = -qi - ri;
      }
    }

    return new Hex(qi, ri, si);
  }

  public FractionalHex HexLerp(FractionalHex b, float t) {
    return new FractionalHex((Q * (1.0f - t)) + (b.Q * t), (R * (1.0f - t)) + (b.R * t), (S * (1.0f - t)) + (b.S * t));
  }

  public static List<Hex> HexLinedraw(Hex a, Hex b) {
    int n = a.Distance(b);
    FractionalHex a_nudge = new(a.Q + 1e-06f, a.R + 1e-06f, a.S - 2e-06f);
    FractionalHex b_nudge = new(b.Q + 1e-06f, b.R + 1e-06f, b.S - 2e-06f);
    List<Hex> results = new();
    float step = 1.0f / MathF.Max(n, 1);

    for (int i = 0; i <= n; i++) {
      results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
    }

    return results;
  }

  public static bool operator ==(FractionalHex a, FractionalHex b) => a.Equals(b);

  public static bool operator !=(FractionalHex a, FractionalHex b) => !(a == b);

  public override bool Equals(object obj) {
    if (obj == null || GetType() != obj.GetType()) {
      return false;
    }

    FractionalHex other = (FractionalHex)obj;

    return Q == other.Q && R == other.R && S == other.S;
  }

  public override int GetHashCode() => Q.GetHashCode() ^ R.GetHashCode() ^ S.GetHashCode();
}

public readonly struct OffsetCoord {
  public readonly int Col;
  public readonly int Row;
  public static readonly int EVEN = 1;
  public static readonly int ODD = -1;

  public OffsetCoord() : this(0, 0) { }

  public OffsetCoord(int col, int row) {
    Col = col;
    Row = row;
  }

  public static OffsetCoord QoffsetFromCube(int offset, Hex h) {
    int col = h.Q;
    int row = h.R + ((h.Q + (offset * (h.Q & 1))) / 2);

    if (offset != EVEN && offset != ODD) {
      throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
    }

    return new OffsetCoord(col, row);
  }

  public static Hex QoffsetToCube(int offset, OffsetCoord h) {
    int q = h.Col;
    int r = h.Row - ((h.Col + (offset * (h.Col & 1))) / 2);
    int s = -q - r;

    if (offset != EVEN && offset != ODD) {
      throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
    }

    return new Hex(q, r, s);
  }

  public static OffsetCoord RoffsetFromCube(int offset, Hex h) {
    int col = h.Q + ((h.R + (offset * (h.R & 1))) / 2);
    int row = h.R;

    if (offset != EVEN && offset != ODD) {
      throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
    }

    return new OffsetCoord(col, row);
  }

  public static Hex RoffsetToCube(int offset, OffsetCoord h) {
    int q = h.Col - ((h.Row + (offset * (h.Row & 1))) / 2);
    int r = h.Row;
    int s = -q - r;

    if (offset != EVEN && offset != ODD) {
      throw new ArgumentException("offset must be EVEN (+1) or ODD (-1)");
    }

    return new Hex(q, r, s);
  }

  public static bool operator ==(OffsetCoord a, OffsetCoord b) => a.Equals(b);

  public static bool operator !=(OffsetCoord a, OffsetCoord b) => !(a == b);

  public override bool Equals(object obj) {
    if (obj == null || GetType() != obj.GetType()) {
      return false;
    }

    OffsetCoord other = (OffsetCoord)obj;

    return Col == other.Col && Row == other.Row;
  }

  public override int GetHashCode() => Col.GetHashCode() ^ Row.GetHashCode();
}

public readonly struct DoubledCoord {
  public readonly int Col;
  public readonly int Row;

  public DoubledCoord() : this(0, 0) { }

  public DoubledCoord(int col, int row) {
    Col = col;
    Row = row;
  }

  public static DoubledCoord QdoubledFromCube(Hex h) {
    int col = h.Q;
    int row = (2 * h.R) + h.Q;

    return new DoubledCoord(col, row);
  }

  public Hex QdoubledToCube() {
    int q = Col;
    int r = (Row - Col) / 2;
    int s = -q - r;

    return new Hex(q, r, s);
  }

  public static DoubledCoord RdoubledFromCube(Hex h) {
    int col = (2 * h.Q) + h.R;
    int row = h.R;

    return new DoubledCoord(col, row);
  }

  public Hex RdoubledToCube() {
    int q = (Col - Row) / 2;
    int r = Row;
    int s = -q - r;

    return new Hex(q, r, s);
  }

  public static bool operator ==(DoubledCoord a, DoubledCoord b) => a.Equals(b);

  public static bool operator !=(DoubledCoord a, DoubledCoord b) => !(a == b);

  public override bool Equals(object obj) {
    if (obj == null || GetType() != obj.GetType()) {
      return false;
    }

    DoubledCoord other = (DoubledCoord)obj;

    return Col == other.Col && Row == other.Row;
  }

  public override int GetHashCode() => Col.GetHashCode() ^ Row.GetHashCode();
}

public readonly struct Orientation {
  public readonly float F0;
  public readonly float F1;
  public readonly float F2;
  public readonly float F3;
  public readonly float B0;
  public readonly float B1;
  public readonly float B2;
  public readonly float B3;
  public readonly float StartAngle;

  public Orientation(float f0,
                     float f1,
                     float f2,
                     float f3,
                     float b0,
                     float b1,
                     float b2,
                     float b3,
                     float startAngle) {
    F0 = f0;
    F1 = f1;
    F2 = f2;
    F3 = f3;
    B0 = b0;
    B1 = b1;
    B2 = b2;
    B3 = b3;
    StartAngle = startAngle;
  }
}

public readonly struct Layout {
  public readonly Orientation Orientation;
  public readonly Vector2 Size;
  public readonly Vector2 Origin;

  public static readonly Orientation Pointy = new(
    MathF.Sqrt(3.0f),
    MathF.Sqrt(3.0f) / 2.0f,
    0.0f,
    3.0f / 2.0f,
    MathF.Sqrt(3.0f) / 3.0f,
    -1.0f / 3.0f,
    0.0f,
    2.0f / 3.0f,
    0.5f);
  public static readonly Orientation Flat = new(
    3.0f / 2.0f,
    0.0f,
    MathF.Sqrt(3.0f) / 2.0f,
    MathF.Sqrt(3.0f),
    2.0f / 3.0f,
    0.0f,
    -1.0f / 3.0f,
    MathF.Sqrt(3.0f) / 3.0f,
    0.0f);

  public Layout(Orientation orientation, Vector2 size, Vector2 origin) {
    Orientation = orientation;
    Size = size;
    Origin = origin;
  }

  public Vector2 HexToPixel(Hex h) {
    Orientation m = Orientation;
    float x = ((m.F0 * h.Q) + (m.F1 * h.R)) * Size.X;
    float y = ((m.F2 * h.Q) + (m.F3 * h.R)) * Size.Y;

    return new Vector2(x + Origin.X, y + Origin.Y);
  }

  public FractionalHex PixelToHex(Vector2 p) {
    Orientation m = Orientation;
    Vector2 pt = new((p.X - Origin.X) / Size.X, (p.Y - Origin.Y) / Size.Y);
    float q = (m.B0 * pt.X) + (m.B1 * pt.Y);
    float r = (m.B2 * pt.X) + (m.B3 * pt.Y);

    return new FractionalHex(q, r, -q - r);
  }

  public Vector2 HexCornerOffset(int corner) {
    Orientation m = Orientation;
    float angle = 2.0f * MathF.PI * (m.StartAngle - corner) / 6.0f;

    return new Vector2(Size.X * MathF.Cos(angle), Size.Y * MathF.Sin(angle));
  }

  public List<Vector2> PolygonCorners(Hex h) {
    List<Vector2> corners = new();
    Vector2 center = HexToPixel(h);
    for (int i = 0; i < 6; i++) {
      Vector2 offset = HexCornerOffset(i);
      corners.Add(new Vector2(center.X + offset.X, center.Y + offset.Y));
    }

    return corners;
  }
}
