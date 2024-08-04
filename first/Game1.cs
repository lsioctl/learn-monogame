using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace first;

public class Game1 : Game
{
    Texture2D ballTexture;
    Vector2 ballPosition;
    float ballSpeed;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    int deadZone;

    private Vector2 ToBoundedPosition() =>
        ballPosition switch
        {
            { X: var x } when x > (_graphics.PreferredBackBufferWidth - ballTexture.Width / 2) =>
                new Vector2(_graphics.PreferredBackBufferWidth - ballTexture.Width / 2, ballPosition.Y),
            { X: var x } when x < ballTexture.Width / 2 =>
                new Vector2(ballTexture.Width / 2, ballPosition.Y),
            { Y: var y } when y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2 =>
            new Vector2(ballPosition.X, _graphics.PreferredBackBufferHeight - ballTexture.Height / 2),
            { Y: var y } when y < ballTexture.Height / 2 =>
                new Vector2(ballPosition.X, ballTexture.Height / 2),
            _ => ballPosition
        };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // center of the screen
        ballPosition = new Vector2(
            _graphics.PreferredBackBufferWidth / 2,
            _graphics.PreferredBackBufferHeight / 2
        );
        ballSpeed = 300f;
        deadZone = 4096;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var kstate = Keyboard.GetState();

        // using delta time
        float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // jeyboard input
        // TODO: function
        if (kstate.IsKeyDown(Keys.Up))
        {
            ballPosition.Y -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Down))
        {
            ballPosition.Y += updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Left))
        {
            ballPosition.X -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.Right))
        {
            ballPosition.X += updatedBallSpeed;
        }

        // joystick input
        // TODO: function
        if (Joystick.LastConnectedIndex == 0)
        {
            // TODO: PlayerIndex.One is a enum, not an int
            // JoystickState jstate = Joystick.GetState(PlayerIndex.One);
            JoystickState jstate = Joystick.GetState((int)PlayerIndex.One);

            if (jstate.Axes[1] < -deadZone)
            {
                ballPosition.Y -= updatedBallSpeed;
            }
            else if (jstate.Axes[1] > deadZone)
            {
                ballPosition.Y += updatedBallSpeed;
            }

            if (jstate.Axes[0] < -deadZone)
            {
                ballPosition.X -= updatedBallSpeed;
            }
            else if (jstate.Axes[0] > deadZone)
            {
                ballPosition.X += updatedBallSpeed;
            }
        }

        // Boundary check
        ballPosition = ToBoundedPosition();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(
            ballTexture,
            ballPosition,
            null,
            Color.White,
            0f,
            // ensure the ball is really centered and not the top left of the image
            new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            01f
        );
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
