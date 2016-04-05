using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JetpackMan
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class JetpackGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        public JetpackGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(new Vector2(100, 500), Content.Load<Texture2D>("Graphics\\player"));

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Keyboard state
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                player.velocity.Y += 0.6f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.position.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player.position.X += 1;
            }
            // Gravity
            player.velocity.Y -= 0.5f;

            System.Console.WriteLine("{0}", player.velocity.Y);

            // Velocity => Position
            player.position += player.velocity;

            // Collision with bottom of screen
            if (player.position.Y < 0)
            {
                player.position.Y = 0;
                player.velocity.Y = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            Vector2 playerPosition = player.position;
            playerPosition.Y = graphics.GraphicsDevice.Viewport.Height - playerPosition.Y - player.texture.Height;
            spriteBatch.Draw(player.texture, playerPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
