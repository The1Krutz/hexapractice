// SPDX-FileCopyrightText: 2023 The1Krutz <the1krutz@gmail.com>
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monohexa;

public class Game1 : Game {
  private readonly int _screenWidth = 2560;
  private readonly int _screenHeight = 1440;

  private readonly Vector2 _spriteSize = new(70, 70);
  private readonly Vector2 _spriteCenterOffset = new(-35, -35);

  private readonly GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;
  private Texture2D _dirtHexTexture;

  private readonly Layout _layout;
  private readonly HashSet<Hex> _hexes = new();

  public Game1() {
    _graphics = new GraphicsDeviceManager(this) {
      PreferredBackBufferWidth = _screenWidth,
      PreferredBackBufferHeight = _screenHeight
    };

    Content.RootDirectory = "Content";
    IsMouseVisible = true;

    _layout = new(
      Layout.Pointy,
      _spriteSize,
      new Vector2(_screenWidth / 2, _screenHeight / 2) + _spriteCenterOffset);
  }

  protected override void Initialize() {
    // TODO: Add your initialization logic here

    /**
     * Note to self here:
     * Instead of worrying about the weird corners or trying to naturally understand the coordinates
     * so you can make a square-ish grid of hexes, just create a big grid using the doubled
     * coordinates, convert it to axial once at the beginning and never go back to doubled. That way
     * your brain can comprehend the square-ish map, and the coordinates are built properly for the
     * hex map
     */
    // use a loop to generate all the hexes, and add them to _hexes
    const int N = 10;

    for (int q = -N; q <= N; q++) {
      for (int r = -N; r <= N; r++) {
        for (int s = -N; s <= N; s++) {
          if (q + r + s == 0) {
            _hexes.Add(new Hex(q, r, s));
          }
        }
      }
    }

    base.Initialize();
  }

  protected override void LoadContent() {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    _dirtHexTexture = Content.Load<Texture2D>("Tiles/Terrain/dirt_01");
  }

  protected override void Update(GameTime gameTime) {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
      Exit();
    }

    // TODO: Add your update logic here

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _spriteBatch.Begin();

    foreach (Hex hex in _hexes) {
      Vector2 pixel = _layout.HexToPixel(hex);
      _spriteBatch.Draw(_dirtHexTexture, pixel, Color.White);
    }

    _spriteBatch.End();

    base.Draw(gameTime);
  }
}
