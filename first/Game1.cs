using System.ComponentModel.DataAnnotations;
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

    private Vector2 ToBoundedPosition()
    {
        float maxRightPosition = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        float maxLeftPosition = ballTexture.Width / 2;
        float maxBottomPosition = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        float maxTopPosition = ballTexture.Height / 2;

        float newX = ballPosition.X switch
        {
            // doesn't seem to works, wants constant
            // > maxRightPosition => maxRightPosition,
            // REMARK: floor function would be better but I want to play with pattern matching :D
            var x when x > maxRightPosition => maxRightPosition,
            var x when x < maxLeftPosition => maxLeftPosition,
            _ => ballPosition.X
        };

        float newY = ballPosition.Y switch
        {
            var y when y > maxBottomPosition => maxBottomPosition,
            var y when y < maxTopPosition => maxTopPosition,
            _ => ballPosition.Y
        };

        return new Vector2(newX, newY);
    }

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
