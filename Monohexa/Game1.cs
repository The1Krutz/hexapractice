using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monohexa;

public class Game1 : Game {
  private GraphicsDeviceManager _graphics;
  private SpriteBatch _spriteBatch;

  Texture2D testHexTexture;

  public Game1() {
    _graphics = new GraphicsDeviceManager(this)
    {
      PreferredBackBufferWidth = 1280,
      PreferredBackBufferHeight = 720
    };

    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  protected override void Initialize() {
    // TODO: Add your initialization logic here

    base.Initialize();
  }

  protected override void LoadContent() {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    testHexTexture = Content.Load<Texture2D>("Tiles/Terrain/dirt_01");
  }

  protected override void Update(GameTime gameTime) {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    // TODO: Add your update logic here

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime) {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _spriteBatch.Begin();
    _spriteBatch.Draw(testHexTexture, new Vector2(0, 0), Color.White);
    _spriteBatch.End();

    base.Draw(gameTime);
  }
}
