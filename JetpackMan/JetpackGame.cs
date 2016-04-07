using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;

namespace JetpackMan
{
    public class JetpackGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        TiledMap map;
        Camera2D camera;

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
            player = new Player(new Vector2(100, 500));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game to load all content.
        /// </summary>
        protected override void LoadContent()
        {
            camera = new Camera2D(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map = Content.Load<TiledMap>("Tilesets\\testmap");
            player.texture = Content.Load<Texture2D>("Graphics\\player");
        }

        /// <summary>
        /// UnloadContent will be called once per game to unload all game-specific content.
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

            player.Update();

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

            map.Draw(camera, true);
            player.Draw(spriteBatch, graphics);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
