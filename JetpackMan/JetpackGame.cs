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
            player = new Player(new Vector2(0, 500));

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
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

            map = Content.Load<TiledMap>("Tilesets\\testmap");
            player.texture = Content.Load<Texture2D>("Graphics\\player");

            camera.ZoomIn(4);
            camera.LookAt(player.BoundingRect.Center);
        }

        /// <summary>
        /// UnloadContent will be called once per game to unload all game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void UpdateCamera(Viewport viewport)
        {

            camera.LookAt(player.BoundingRect.Center);

            /*** Failed attempt at scrolling camera:

            RectangleF camRect = camera.GetBoundingRectangle();
            Vector2 viewportSize = viewport.Bounds.Size.ToVector2();
            RectangleF scrollBounds = new RectangleF(camRect.Center, 0.5f * viewportSize);

            if (player.BoundingRect.Right > scrollBounds.Right)
            {
                System.Console.WriteLine("Intersect Right");
                camera.Move(new Vector2(scrollBounds.Right - player.BoundingRect.Right, 0));
            }
            else if (player.BoundingRect.Left < scrollBounds.Left)
            {
                System.Console.WriteLine("Intersect Left");
                camera.Move(new Vector2(scrollBounds.Left - player.BoundingRect.Left, 0));
            }
            if (player.BoundingRect.Bottom > scrollBounds.Bottom)
            {
                System.Console.WriteLine("Intersect Bottom");
                camera.Move(new Vector2(0, scrollBounds.Bottom - player.BoundingRect.Bottom));
            }
            else if (player.BoundingRect.Top < scrollBounds.Top)
            {
                System.Console.WriteLine("Intersect Top");
                camera.Move(new Vector2(0, scrollBounds.Top - player.BoundingRect.Top));
            }

            ***/
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

            player.Update(graphics.GraphicsDevice.Viewport);

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

            base.Draw(gameTime);
        }
    }
}
