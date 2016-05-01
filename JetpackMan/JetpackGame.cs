using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.Shapes;

namespace JetpackMan
{
    public class JetpackGame : Game
    {
        static int WindowWidth = 1280;
        static int WindowHeight = 800;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch uiSpriteBatch;

        Player player;
        TiledMap map;
        Camera2D camera;
        RectangleF cameraTarget;
        
        ProgressBar jetpackFuelBar;

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
            player = new Player(new Vector2(0, 500));

            jetpackFuelBar = new ProgressBar(new RectangleF(WindowWidth - 100, 20, 80, 12), Color.DarkOliveGreen, Color.Gold, ProgressFillDirection.LeftToRight);

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game to load all content.
        /// </summary>
        protected override void LoadContent()
        {
            camera = new Camera2D(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uiSpriteBatch = new SpriteBatch(GraphicsDevice);

            map = Content.Load<TiledMap>("Tilesets\\testmap");
            player.texture = Content.Load<Texture2D>("Graphics\\player");

            player.position.X = 256;
            player.position.Y = map.HeightInPixels;

            camera.ZoomIn(2f);
            cameraTarget = new RectangleF(player.position.X - (cameraTarget.Width / 2),
                                          player.position.Y - (cameraTarget.Height / 2),
                                          0.1f * WindowWidth, 0.1f * WindowHeight); // TODO: figure out why it's 0.1f
            camera.LookAt(cameraTarget.Center);
        }

        /// <summary>
        /// UnloadContent will be called once per game to unload all game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        protected void UpdateCamera(Viewport viewport)
        {
            if (player.BoundingRect.Left < cameraTarget.Left)
            {
                cameraTarget.X = player.BoundingRect.Left;
            }

            if (player.BoundingRect.Right > cameraTarget.Right)
            {
                cameraTarget.X += player.BoundingRect.Right - cameraTarget.Right;
            }

            if (player.BoundingRect.Bottom > cameraTarget.Bottom)
            {
                cameraTarget.Y += player.BoundingRect.Bottom - cameraTarget.Bottom;
            }

            if (player.BoundingRect.Top < cameraTarget.Top)
            {
                cameraTarget.Y = player.BoundingRect.Top;
            }
            
            camera.LookAt(cameraTarget.Center);
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

            player.Update(map);
            jetpackFuelBar.progress = (float) player.JetpackFuelCtr / Player.MaxJetpackFuelFrames;

            UpdateCamera(graphics.GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
                map.Draw(spriteBatch);
                player.Draw(spriteBatch);
            spriteBatch.End();

            uiSpriteBatch.Begin();
                jetpackFuelBar.Draw(uiSpriteBatch);
            uiSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
