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

            jetpackFuelBar = new ProgressBar(new Rectangle(WindowWidth - 100, 20, 80, 12), Color.DarkOliveGreen, Color.Gold, ProgressBarFillDirection.LeftToRight);

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

            player = new Player(new Vector2(256, map.HeightInPixels), Content.Load<Texture2D>("Graphics\\player"), Content.Load<Texture2D>("Graphics\\bullet"));
            EntityManager.AddEntity(player);

            camera.ZoomIn(2f);
            cameraTarget = new RectangleF(player.position.X - (cameraTarget.Width / 2),
                                          player.position.Y - (cameraTarget.Height / 2),
                                          0.1f * WindowWidth, 0.1f * WindowHeight); // TODO: figure out why it's 0.1f
            camera.LookAt(cameraTarget.Center);
        }

        /// <summary>
        /// UnloadContent will be called once per game to unload all game-specific content.
        /// </summary>
        protected override void UnloadContent() { }

        void UpdateEntities()
        {
            EntityManager.Update(map);
        }

        void UpdateCamera()
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

        void UpdateUI()
        {
            jetpackFuelBar.progress = ((float)player.jetpackFuelCtr) / Player.MaxJetpackFuelFrames;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateEntities();

            UpdateUI();
            UpdateCamera();

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
                EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            uiSpriteBatch.Begin();
                jetpackFuelBar.Draw(uiSpriteBatch);
            uiSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
