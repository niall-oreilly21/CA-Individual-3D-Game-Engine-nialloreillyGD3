#region Pre-compiler directives

#define DEMO
//#define SHOW_DEBUG_INFO

#endregion

using App.Managers;
using GD.App;
using GD.Core;
using GD.Engine;
using GD.Engine.Collections;
using GD.Engine.Events;
using GD.Engine.Globals;
using GD.Engine.Inputs;
using GD.Engine.Managers;
using GD.Engine.Parameters;
using GD.Engine.Utilities;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static GD.Engine.Camera;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using Application = GD.Engine.Globals.Application;
using Box = JigLibX.Geometry.Box;
using Cue = GD.Engine.Managers.Cue;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Material = GD.Engine.Material;

namespace GD.App
{
    public class Main : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect unlitEffect;
        private BasicEffect litEffect;

        private CameraManager cameraManager;
        private SceneManager<Scene> sceneManager;
        private SoundManager soundManager;
        private PhysicsManager physicsManager;
        private RenderManager renderManager;
        private EventDispatcher eventDispatcher;
        private PickingManager pickingManager;
        private MyStateManager stateManager;
        private SceneManager<Scene2D> uiManager;
        private SceneManager<Scene2D> menuManager;
        private ConsumableManager foodManager;
        private ConsumableManager bombManager;
        private GameObject foodGameObject;
        private GameObject bombGameObject;
        private GameObject menuGameObjectTitle;
        private GameObject snakeCameraGameObject;
        private BasicEffect exitSignEffect;
        SpriteFont spriteFontMenu;
        SpriteFont spriteFontUI;
        private Dictionary<string, MenuButton> menuButtonDictionary;
        private ContentDictionary<Texture2D> textureDictionary;
        private ContentDictionary<SpriteFont> fontDictionary;
        private Dictionary<string, Curve3D> curveDictionary;
        private SnakeManager snakeManager;

#if DEMO

        private event EventHandler OnChanged;

#endif

        #endregion Fields

        #region Constructors

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #endregion Constructors

        #region Actions - Initialize

#if DEMO

        private void DemoCode()
        {
            //shows how we can create an event, register for it, and raise it in Main::Update() on Keys.E press
            DemoEvent();

            //shows us how to listen to a specific event
            DemoStateManagerEvent();

            Demo3DSoundTree();
        }

        private void Demo3DSoundTree()
        {
            //var camera = Application.CameraManager.ActiveCamera.AudioListener;
            //var audioEmitter = //get tree, get emitterbehaviour, get audio emitter

            //object[] parameters = {"sound name", audioListener, audioEmitter};

            //EventDispatcher.Raise(new EventData(EventCategoryType.Sound,
            //    EventActionType.OnPlay3D, parameters));
            //throw new NotImplementedException();
        }

        private void DemoStateManagerEvent()
        {
            EventDispatcher.Subscribe(EventCategoryType.Game, HandleEvent);
        }

        private void HandleEvent(EventData eventData)
        {
            switch (eventData.EventActionType)
            {
                case EventActionType.OnWin:
                    System.Diagnostics.Debug.WriteLine(eventData.Parameters[0] as string);
                    break;

                case EventActionType.OnLose:
                    menuManager.SetActiveScene(AppData.END_MENU_NAME);
                    break;

                case EventActionType.InitializeLevelUITimerStart:
                    InitializeLevelUITimerStart();
                    break;

                case EventActionType.ResetIntroCamera:
                    ResetIntroCameraCurve();
                    break;

            }
        }

   
        private void DemoEvent()
        {
            OnChanged += HandleOnChanged;
        }

        private void HandleOnChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{e} was sent by {sender}");
        }

#endif

        protected override void Initialize()
        {
            //moved spritebatch initialization here because we need it in InitializeDebug() below
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //core engine - common across any game
            InitializeEngine(AppData.APP_RESOLUTION, true, true);

            //game specific content
            InitializeLevel("Snake 3D", AppData.SKYBOX_WORLD_SCALE);

#if SHOW_DEBUG_INFO
            InitializeDebug();
#endif

#if DEMO
            DemoCode();
#endif

            base.Initialize();
        }

        #endregion Actions - Initialize

        #region Actions - Level Specific

        protected override void LoadContent()
        {
            //moved spritebatch initialization to Main::Initialize() because we need it in InitializeDebug()
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void InitializeLevel(string title, float worldScale)
        {
            //set game title
            SetTitle(title);

            //load sounds, textures, models etc
            LoadMediaAssets();

            //initialize curves used by cameras
            InitializeCurves();

            //initialize rails used by cameras
            InitializeRails();

            //add scene manager and starting scenes
            InitializeScenes();

            //add collidable drawn stuff
            InitializeCollidableContent(worldScale);

            //add non-collidable drawn stuff
            InitializeNonCollidableContent(worldScale);

            //add the player
            InitializeSnakeHead();
            InitilizeConsumableManagers();

            //add menus
            InitializeMenus();

            //set UI scene
            InitilizeUIScene();
            InitializeUI();

            //send all initial events

            #region Start Events - Menu etc

            //start the game paused
            EventDispatcher.Raise(new EventData(EventCategoryType.Menu, EventActionType.OnPause));

            #endregion
        }

        private void InitilizeConsumableManagers()
        {
            foodManager = new FoodManager(this, foodGameObject);
            bombManager = new BombManager(this, bombGameObject);
        }

        private void InitilizeUIScene()
        {
            var mainHUD = new Scene2D("game HUD");

            #region Add Scene to Manager and Set Active

            //add scene2D to manager
            uiManager.Add(mainHUD.ID, mainHUD);

            //what ui do i see first?
            uiManager.SetActiveScene(mainHUD.ID);

            #endregion
        }


        private void InitializeMenus()
        {
            InitializeMenuTitle();
            InitializeMainMenu();
            InitializeLevelsMenu();
            InitializeControlsMenu();
            InitializeAudioMenu();
            InitializePauseMenu();
            InitializeEndGameMenu();

            menuManager.SetActiveScene(AppData.LEVELS_SCENE_NAME);
        }

        private void InitializeMenuTitle()
        {
            #region Main Menu Title
            menuGameObjectTitle = InitializeUIText(AppData.MENU_TITLE_UI_TEXT_NAME, AppData.MENU_TITLE_UI_TEXT_SCALE, AppData.MENU_TITLE_TEXT_OFFSET, AppData.MENU_TITLE_UI_TEXT, AppData.MENU_TITLE_UI_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Menu_Text, AppData.MENU_TITLE_TRANSLATION);
            #endregion Main Menu Title
        }

        private GameObject CloneMenusBackgroundTexture(string newName, string textureName)
        {
            #region Menus Background Texture
            Texture2D cloneMenuBackgroundTexture = textureDictionary[textureName];

            GameObject menuBackgroundTextureCloneGameObject = new GameObject(newName);
            var scaleToWindow = _graphics.GetScaleFactorForResolution(cloneMenuBackgroundTexture, Vector2.Zero);

            menuBackgroundTextureCloneGameObject.Transform =
                new Transform
                (
                    new Vector3(scaleToWindow, 1),
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0)
                );

            var material = new TextureMaterial2D(cloneMenuBackgroundTexture, Color.White, 1);
            menuBackgroundTextureCloneGameObject.AddComponent(new Renderer2D(material));

            return menuBackgroundTextureCloneGameObject;
            #endregion Menus Background Texture
        }

        public GameObject CloneModelGameObjectButton(string newName)
        {
            GameObject gameObjectClone = new GameObject(newName);
            Renderer2D cloneRenderer2D = null;

            #region Transform
            gameObjectClone.Transform = new Transform(
                new Vector3(AppData.BUTTON_SCALE, 1),
                Vector3.Zero,
                new Vector3(Application.Screen.ScreenCentre - AppData.BUTTON_SCALE * textureDictionary[AppData.SNAKE_MENU_BUTTON_TEXTURE_NAME].GetCenter() - menuButtonDictionary[newName].ButtonTranslation, 0)
                );
            #endregion Transform


            #region Texture
            Material2D cloneTextureMaterial2D = new TextureMaterial2D(textureDictionary[AppData.SNAKE_MENU_BUTTON_TEXTURE_NAME], menuButtonDictionary[newName].ButtonColor, 0.9f);
            cloneRenderer2D = new Renderer2D(cloneTextureMaterial2D);
            gameObjectClone.AddComponent(cloneRenderer2D);
            #endregion Texture

            #region Collider
            ButtonCollider2D cloneSnakeButtonCollider2D = new SnakeButtonCollider2D(gameObjectClone, cloneRenderer2D, AppData.BUTTON_HOVER_SCALE_BY, AppData.BUTTON_HOVER_OFFSET);
            cloneSnakeButtonCollider2D.AddEvent(MouseButton.Left, menuButtonDictionary[newName].ButtonEventData);
            gameObjectClone.AddComponent(cloneSnakeButtonCollider2D);
            #endregion Collider

            #region Text
            Material2D cloneTextMaterial2D = new TextMaterial2D(fontDictionary[AppData.MENU_FONT_NAME], new StringBuilder(menuButtonDictionary[newName].ButtonText), menuButtonDictionary[newName].ButtonTextOffSet, Color.White, 0.8f);
            cloneRenderer2D = new Renderer2D(cloneTextMaterial2D);
            gameObjectClone.AddComponent(cloneRenderer2D);
            #endregion Text

            return gameObjectClone;
        }

        private void InitializeMainMenu()
        {
            #region Create Main Menu Scene

            GameObject menuGameObject = null;

            var mainMenuScene = new Scene2D(AppData.MAIN_MENU_SCENE_NAME);
            menuGameObject = CloneMenusBackgroundTexture(AppData.MENU_BACKGROUND_NAME + AppData.MAIN_MENU_SCENE_NAME, AppData.SNAKE_MENU_BACKGROUND_TEXTURE_NAME);
            mainMenuScene.Add(menuGameObject);
            mainMenuScene.Add(menuGameObjectTitle);

            menuGameObject = CloneModelGameObjectButton(AppData.START_BUTTON_NAME);
            mainMenuScene.Add(menuGameObject);
            
            menuGameObject = CloneModelGameObjectButton(AppData.AUDIO_BUTTON_NAME);
            mainMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton(AppData.CONTROLS_BUTTON_NAME);
            mainMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton( AppData.EXIT_BUTTON_NAME);
            mainMenuScene.Add(menuGameObject);


            #region Add Scene to Manager
            menuManager.Add(mainMenuScene.ID, mainMenuScene);
            #endregion Add Scene to Manager

            #endregion Create Main Menu Scene
        }

        private void InitializeLevelsMenu()
        {
            #region Create Levels Menu Scene


            GameObject menuGameObject = null;

            var levelsMenuScene = new Scene2D(AppData.LEVELS_SCENE_NAME);
            menuGameObject = CloneMenusBackgroundTexture(AppData.MENU_BACKGROUND_NAME + AppData.LEVELS_SCENE_NAME, AppData.MENU_BACKGROUND_TEXTURE_NAME);
            levelsMenuScene.Add(menuGameObject);
            levelsMenuScene.Add(menuGameObjectTitle);

            menuGameObject = CloneModelGameObjectButton(AppData.LEVEL_ONE_BUTTON_NAME);
            levelsMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton(AppData.LEVEL_TWO_BUTTON_NAME);
            levelsMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton(AppData.LEVEL_THREE_BUTTON_NAME);
            levelsMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton(AppData.BACK_BUTTON_NAME);
            levelsMenuScene.Add(menuGameObject);

            #region Add Scene to Manager
            menuManager.Add(levelsMenuScene.ID, levelsMenuScene);
            #endregion Add Scene to Manager

            #endregion Create Levels Menu Scene
        }

        private void InitializePauseMenu()
        {
            #region Create Pause Menu Scene
            GameObject menuGameObject = null;

            var pauseMenuScene = new Scene2D(AppData.PAUSE_SCENE_NAME);
            menuGameObject = CloneMenusBackgroundTexture(AppData.MENU_BACKGROUND_NAME + AppData.PAUSE_SCENE_NAME, AppData.SNAKE_MENU_BACKGROUND_TEXTURE_NAME);
            pauseMenuScene.Add(menuGameObject);
            pauseMenuScene.Add(menuGameObjectTitle);
            

            menuGameObject = CloneModelGameObjectButton(AppData.RESUME_BUTTON_NAME);
            pauseMenuScene.Add(menuGameObject);

            menuGameObject = CloneModelGameObjectButton(AppData.MAIN_MENU_BUTTON_NAME);
            pauseMenuScene.Add(menuGameObject);

            #region Add Scene to Manager
            menuManager.Add(pauseMenuScene.ID, pauseMenuScene);

            #endregion Add Scene to Manager

            #endregion Pause Menu Scene
        }

        private GameObject CloneControlsTexture(string newName, Vector3 newScale, Vector3 newTranslation)
        {
            GameObject cloneGameObject = new GameObject(newName);

            cloneGameObject.GameObjectType = GameObjectType.UI_Texture;

            cloneGameObject.Transform = new Transform
                (
                newScale,
                new Vector3(0, 0, 0),
                newTranslation
                );

            TextureMaterial2D cloneMaterial = new TextureMaterial2D(textureDictionary[newName], Color.White, 0.9f);

            Renderer2D cloneRenderer2D = new Renderer2D(cloneMaterial);
            cloneGameObject.AddComponent(cloneRenderer2D);

            return cloneGameObject;
        }
        private void InitializeControlsMenu()
        {
            #region Create Controls Menu Scene
            GameObject menuGameObject = null;
            var controlsMenuScene = new Scene2D(AppData.CONTROLS_SCENE_NAME);
            controlsMenuScene.Add(menuGameObjectTitle);

            #region Controls Menu Textures
            menuGameObject = CloneModelGameObjectButton(AppData.BACK_BUTTON_NAME);
            controlsMenuScene.Add(menuGameObject);

            menuGameObject = CloneMenusBackgroundTexture(AppData.MENU_BACKGROUND_NAME + AppData.CONTROLS_SCENE_NAME, AppData.MENU_BACKGROUND_TEXTURE_NAME);
            controlsMenuScene.Add(menuGameObject);

            menuGameObject = CloneControlsTexture(AppData.CAMERA_CONTROLS_BACKGROUND_NAME, AppData.CAMERA_CONTROLS_BACKGROUND_SCALE, AppData.CAMERA_CONTROLS_BACKGROUND_TRANSLATION);
            controlsMenuScene.Add(menuGameObject);

            menuGameObject = CloneControlsTexture(AppData.SNAKE_CONTROLS_BACKGROUND_NAME, AppData.SNAKE_CONTROLS_BACKGROUND_SCALE, AppData.SNAKE_CONTROLS_BACKGROUND_TRANSLATION);
            controlsMenuScene.Add(menuGameObject);

            menuGameObject = CloneControlsTexture(AppData.XYZ_CONTROLS_BACKGROUND_NAME, AppData.XYZ_CONTROLS_BACKGROUND_SCALE, AppData.XYZ_CONTROLS_BACKGROUND_TRANSLATION);
            controlsMenuScene.Add(menuGameObject);
            #endregion Controls Menu Textures

            #region Controls Menu Text
            menuGameObject = InitializeUIText(AppData.SNAKE_CONTROLS_UI_TEXT_NAME, AppData.CONTROLS_UI_TEXT_SCALE, AppData.SNAKE_CONTROLS_TEXT_OFFSET, AppData.SNAKE_CONTROLS_UI_TEXT, AppData.CONTROLS_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Menu_Text);
            controlsMenuScene.Add(menuGameObject);

            menuGameObject = InitializeUIText(AppData.CAMERA_CONTROLS_UI_TEXT_NAME, AppData.CONTROLS_UI_TEXT_SCALE, AppData.CAMERA_CONTROLS_TEXT_OFFSET, AppData.CAMERA_CONTROLS_UI_TEXT, AppData.CONTROLS_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Menu_Text);
            controlsMenuScene.Add(menuGameObject);
            #endregion Controls Menu Text

            #region Add Scene to Manager
            menuManager.Add(controlsMenuScene.ID, controlsMenuScene);

            #endregion Add Scene to Manager

            #endregion Create Controls Menu Scene
        }

        private void InitializeAudioMenu()
        {
            #region Create Audio Menu Scene
            GameObject menuGameObject = null;
            var audioMenuScene = new Scene2D(AppData.AUDIO_SCENE_NAME);
            audioMenuScene.Add(menuGameObjectTitle);

            menuGameObject = CloneModelGameObjectButton(AppData.BACK_BUTTON_NAME);
            audioMenuScene.Add(menuGameObject);

            #region Add Scene to Manager
            menuManager.Add(audioMenuScene.ID, audioMenuScene);

            #endregion Add Scene to Manager

            #endregion Create Audio Menu Scene
        }

        private void InitializeEndGameMenu()
        {
            
        }

        private GameObject InitializeUIText(string newName, Vector2 newScale, Vector2 textOffSet, string text, Color textColor, string uiFontName, GameObjectType gameObjectType)
        {
            GameObject uiTextGameObjectClone = new GameObject(newName);
            uiTextGameObjectClone.GameObjectType = gameObjectType;

            uiTextGameObjectClone.Transform = new Transform(
                new Vector3(newScale, 1),
                Vector3.Zero,
                Vector3.Zero
                );

            TextMaterial2D material = new TextMaterial2D(fontDictionary[uiFontName], text, textOffSet, textColor, 0.8f);

            Renderer2D renderer2D = new Renderer2D(material);

            uiTextGameObjectClone.AddComponent(renderer2D);

            return uiTextGameObjectClone;

        }

        private GameObject InitializeUIText(string newName, Vector2 newScale, Vector2 textOffSet, string text, Color textColor, string uiFontName, GameObjectType gameObjectType, Vector2 translation)
        {
            GameObject uiTextGameObjectClone = new GameObject(newName);
            uiTextGameObjectClone.GameObjectType = gameObjectType;

            uiTextGameObjectClone.Transform = new Transform(
                new Vector3(newScale, 1),
                Vector3.Zero,
                new Vector3(Application.Screen.ScreenCentre - newScale * textureDictionary[AppData.SNAKE_MENU_BUTTON_TEXTURE_NAME].GetCenter() - translation, 0)
                );

            TextMaterial2D material = new TextMaterial2D(fontDictionary[uiFontName], text, textOffSet, textColor, 0.8f);

            Renderer2D renderer2D = new Renderer2D(material);

            uiTextGameObjectClone.AddComponent(renderer2D);

            return uiTextGameObjectClone;

        }


        private void InitializeLevelUITimerStart()
        {
            #region Level Start Time UI
            GameObject uiGameObject = null;

            uiGameObject = InitializeUIText(AppData.LEVEL_START_TIME_UI_NAME, AppData.LEVEL_START_TIMER_UI_TEXT_SCALE, Application.Screen.ScreenCentre - AppData.LEVEL_START_TIMER_UI_TEXT_OFFSET, AppData.TIMER_UI_TEXT, AppData.DEFAULT_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Timer_Text);
            uiManager.ActiveScene.Add(uiGameObject);

            uiGameObject.AddComponent(new UIStartLevelTimerController(AppData.LEVEL_START_TIMER_UI_SECONDS));

            #endregion Level Start Time UI
        }

        private void InitializeUI()
        {
            GameObject uiGameObject = null;

            #region Current Level
            uiGameObject = InitializeUIText(AppData.LEVEL_UI_TEXT_NAME, AppData.LEVEL_UI_TEXT_SCALE, AppData.LEVEL_UI_TEXT_OFFSET, AppData.DEFAULT_LEVEL_TEXT + stateManager.CurrentScore, AppData.DEFAULT_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Game_Text);
            uiManager.ActiveScene.Add(uiGameObject);
            #endregion Current Level

            #region Current Score
            uiGameObject = InitializeUIText(AppData.SCORE_UI_TEXT_NAME, AppData.SCORE_UI_TEXT_SCALE, AppData.SCORE_UI_TEXT_OFFSET, AppData.DEFAULT_SCORE_TEXT + stateManager.CurrentScore, AppData.DEFAULT_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Game_Text);
            uiManager.ActiveScene.Add(uiGameObject);
            #endregion Current Score

            #region Current Camera
            uiGameObject = InitializeUIText(AppData.CAMERA_UI_TEXT_NAME, AppData.CAMERA_UI_TEXT_SCALE, AppData.CAMERA_UI_TEXT_OFFSET, AppData.FRONT_CAMERA_UI_TEXT, AppData.DEFAULT_UI_TEXT_COLOR, AppData.MENU_FONT_NAME, GameObjectType.UI_Game_Text);
            uiManager.ActiveScene.Add(uiGameObject);
            #endregion Current Camera

            #region Timer
            uiGameObject = InitializeUIText(AppData.TIMER_UI_TEXT_NAME, AppData.TIMER_UI_TEXT_SCALE, AppData.TIMER_UI_TEXT_OFFSET, AppData.TIMER_UI_TEXT, AppData.TIMER_UI_TEXT_COLOR, AppData.UI_FONT_NAME, GameObjectType.UI_Game_Text);
            uiManager.ActiveScene.Add(uiGameObject);
            uiGameObject.AddComponent(new UITimerController());
            uiManager.ActiveScene.Add(uiGameObject);
            #endregion Timer

            #region UI Helper
            uiGameObject = InitializeUIText(AppData.UI_TEXT_HELPER_NAME, AppData.UI_TEXT_HELPER_SCALE, AppData.UI_TEXT_HELPER_TEXT_OFFSET, AppData.UI_TEXT_HELPER_TEXT[0], AppData.TIMER_UI_TEXT_COLOR, AppData.UI_FONT_NAME, GameObjectType.UI_Game_Text);
            uiManager.ActiveScene.Add(uiGameObject);
            uiGameObject.AddComponent(new UIHelperController(AppData.UI_TEXT_HELPER_TEXT));
            uiManager.ActiveScene.Add(uiGameObject);
            #endregion UI Helper

        }

        private void SetTitle(string title)
        {
            Window.Title = title.Trim();
        }

        private void LoadMediaAssets()
        {
            //sounds, models, textures
            LoadSounds();
            LoadTextures();
            LoadModels();
            LoadFonts();
        }

        private void LoadSounds()
        {
            var soundEffect =
                Content.Load<SoundEffect>("Assets/Audio/Diegetic/explode1");

            //add the new sound effect
            soundManager.Add(new Cue(
                "boom1",
                soundEffect,
                SoundCategoryType.Alarm,
                new Vector3(1, 1, 0),
                false));
        }

        private void LoadFonts()
        {
            fontDictionary.Add(AppData.MENU_FONT_NAME, AppData.MENU_FONT_PATH);
            fontDictionary.Add(AppData.UI_FONT_NAME, AppData.UI_FONT_PATH);
        }
        private void LoadTextures()
        {
            #region Game Textures
            textureDictionary.Add(AppData.BACKGROUND_TEXTURE_NAME, AppData.BACKGROUND_TEXTURE_PATH);
            #endregion Game Textures

            #region Consumables Textures
            textureDictionary.Add(AppData.FOOD_TEXTURE_NAME, AppData.FOOD_TEXTURE_PATH);
            #endregion Consumables Textures

            #region Snake Textures
            textureDictionary.Add(AppData.SNAKE_TONGUE_TEXTURE_NAME, AppData.SNAKE_TONGUE_TEXTURE_PATH);
            textureDictionary.Add(AppData.SNAKE_SKIN_TEXTURE_NAME, AppData.SNAKE_SKIN_TEXTURE_PATH);
            textureDictionary.Add(AppData.SNAKE_HEAD_TEXTURE_NAME, AppData.SNAKE_HEAD_TEXTURE_PATH);
            #endregion Snake Textures

            #region Menu Textures
            textureDictionary.Add(AppData.SNAKE_MENU_BUTTON_TEXTURE_NAME, AppData.SNAKE_MENU_BUTTON_TEXTURE_PATH);
            textureDictionary.Add(AppData.MENU_BACKGROUND_TEXTURE_NAME, AppData.MENU_BACKGROUND_TEXTURE_PATH);
            textureDictionary.Add(AppData.SNAKE_MENU_BACKGROUND_TEXTURE_NAME, AppData.SNAKE_MENU_BACKGROUND_TEXTURE_PATH);

            #region Controls Textures
            textureDictionary.Add(AppData.CAMERA_CONTROLS_BACKGROUND_NAME, AppData.CAMERA_CONTROLS_TEXTURE_PATH);
            textureDictionary.Add(AppData.SNAKE_CONTROLS_BACKGROUND_NAME, AppData.SNAKE_CONTROLS_BACKGROUND_TEXTURE_PATH);
            textureDictionary.Add(AppData.XYZ_CONTROLS_BACKGROUND_NAME, AppData.XYZ_CONTROLS_BACKGROUND_TEXTURE_PATH);
            #endregion Controls Textures

            #endregion Menu Textures
    }

        private void LoadModels()
        {
            //load and add to dictionary
        }

        private void InitializeCurves()
        {
            Curve3D curve3DTranslation = new Curve3D(CurveLoopType.Oscillate);

            for (int i = 0; i < AppData.CURVE_TRANSLATIONS.Length; i++)
            {
                curve3DTranslation.Add(AppData.CURVE_TRANSLATIONS[i], AppData.CURVE_TIME_SPAN * i);
            }

            Curve3D curve3DRotation = new Curve3D(CurveLoopType.Oscillate);

            for (int i = 0; i < AppData.CURVE_ROTATIONS.Length; i++)
            {
                curve3DRotation.Add(AppData.CURVE_ROTATIONS[i], AppData.CURVE_TIME_SPAN * i);
            }


        }

        private void InitializeRails()
        {
            //load and add to dictionary
        }

        private void InitializeScenes()
        {
            //initialize a scene
            var scene = new Scene(AppData.GAME_SCENE_NAME);

            //add scene to the scene manager
            sceneManager.Add(scene.ID, scene);

            //don't forget to set active scene
            sceneManager.SetActiveScene(AppData.GAME_SCENE_NAME);
        }

        private void InitializeEffects()
        {

            spriteFontMenu = Content.Load<SpriteFont>("Assets/Fonts/menu_font");
            spriteFontUI = Content.Load<SpriteFont>("Assets/Fonts/ui_font");

            //only for skybox with lighting disabled
            unlitEffect = new BasicEffect(_graphics.GraphicsDevice);
            unlitEffect.TextureEnabled = true;

            //all other drawn objects
            litEffect = new BasicEffect(_graphics.GraphicsDevice);
            litEffect.TextureEnabled = true;
            litEffect.LightingEnabled = true;
            litEffect.EnableDefaultLighting();


            #region Exit Sign Emission Effect

            exitSignEffect = new BasicEffect(_graphics.GraphicsDevice);
            exitSignEffect.TextureEnabled = true;
            exitSignEffect.LightingEnabled = true;

            exitSignEffect.PreferPerPixelLighting = true;

            //exitSignEffect.AmbientLightColor = Color.AntiqueWhite.ToVector3();
            exitSignEffect.EmissiveColor = new Vector3(165 / 255f, 226 / 255f, 255 / 255f);
            exitSignEffect.EnableDefaultLighting();
            exitSignEffect.DirectionalLight0.DiffuseColor = new Vector3(165 / 255f, 226 / 255f, 255 / 255f);
            exitSignEffect.DirectionalLight0.Direction = new Vector3(0, 0, 1);

            //exitSignEffect.AmbientLightColor = new Vector3(232 / 255f, 71 / 255f, 76 / 255f);

            exitSignEffect.AmbientLightColor = new Vector3(165 / 255f, 226 / 255f, 255 / 255f);

            #endregion
        }

        private void ResetIntroCameraCurve()
        {
            //define what action the curve will apply to the target game object
            var curveAction = (Curve3D curve, GameObject target, GameTime gameTime) =>
            {
                target.Transform.SetTranslation(curveDictionary[AppData.INTRO_CURVE_TRANSLATIONS_NAME].Evaluate(gameTime.TotalGameTime.TotalMilliseconds, 1));
                target.Transform.SetRotation(curveDictionary[AppData.INTRO_CURVE_ROTATIONS_NAME].Evaluate(gameTime.TotalGameTime.TotalMilliseconds, 1));
            };

            cameraManager.SetActiveCamera(AppData.CURVE_CAMERA_NAME);

            cameraManager.ActiveCamera.gameObject.RemoveComponent<CurveBehaviour>();

            cameraManager.ActiveCamera.gameObject.AddComponent(new CurveBehaviour(curveDictionary[AppData.INTRO_CURVE_TRANSLATIONS_NAME], curveAction));
        }

        public GameObject CloneModelGameObjectCamera(GameObject gameObject, string newName, Vector3 rotation, Vector3 translation)
        {
            GameObject gameObjectClone = new GameObject(newName);

            gameObjectClone.Transform = new Transform(
                null,
                rotation,
                translation
                );

            Camera camera = gameObject.GetComponent<Camera>();
            Camera cloneCamera = new Camera(camera.FieldOfView, camera.AspectRatio, camera.NearPlaneDistance, camera.FarPlaneDistance, camera.ViewPort);
            gameObjectClone.AddComponent(cloneCamera);

            return gameObjectClone;
        }
        private void InitializeCameras()
        {
            //camera
            GameObject cameraGameObject = null;

            #region Third Person

            cameraGameObject = new GameObject(AppData.THIRD_PERSON_CAMERA_NAME);
            cameraGameObject.Transform = new Transform(null, null, null);
            cameraGameObject.AddComponent(new Camera(
                AppData.FIRST_PERSON_HALF_FOV, //MathHelper.PiOver2 / 2,
                (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
                AppData.FIRST_PERSON_CAMERA_NCP, //0.1f,
                AppData.FIRST_PERSON_CAMERA_FCP,
                new Viewport(0, 0, _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight))); // 3000

            cameraGameObject.AddComponent(new ThirdPersonController());

            cameraManager.Add(cameraGameObject.Name, cameraGameObject);

            #endregion

            #region First Person

            //camera 1
            cameraGameObject = new GameObject(AppData.FIRST_PERSON_CAMERA_NAME);
            cameraGameObject.Transform = new Transform(null, null,
                AppData.FIRST_PERSON_DEFAULT_CAMERA_POSITION);

            #region Camera - View & Projection

            cameraGameObject.AddComponent(
             new Camera(
             AppData.FIRST_PERSON_HALF_FOV, //MathHelper.PiOver2 / 2,
             (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
             AppData.FIRST_PERSON_CAMERA_NCP, //0.1f,
             AppData.FIRST_PERSON_CAMERA_FCP,
             new Viewport(0, 0, _graphics.PreferredBackBufferWidth,
             _graphics.PreferredBackBufferHeight))); // 3000

            #endregion

            #region Collision - Add capsule

            //adding a collidable surface that enables acceleration, jumping
            var characterCollider = new CharacterCollider(cameraGameObject, true);

            cameraGameObject.AddComponent(characterCollider);
            characterCollider.AddPrimitive(new Capsule(
                cameraGameObject.Transform.Translation,
                Matrix.CreateRotationX(MathHelper.PiOver2),
                1, 3.6f),
                new MaterialProperties(0.2f, 0.8f, 0.7f));
            characterCollider.Enable(cameraGameObject, false, 1);

            #endregion

            #region Collision - Add Controller for movement (now with collision)

            cameraGameObject.AddComponent(new CollidableFirstPersonController(cameraGameObject,
                characterCollider,
                AppData.FIRST_PERSON_MOVE_SPEED, AppData.FIRST_PERSON_STRAFE_SPEED,
                AppData.PLAYER_ROTATE_SPEED_VECTOR2, AppData.FIRST_PERSON_CAMERA_SMOOTH_FACTOR, true,
                AppData.PLAYER_COLLIDABLE_JUMP_HEIGHT));

            #endregion

            #region 3D Sound

            //added ability for camera to listen to 3D sounds
            cameraGameObject.AddComponent(new AudioListenerBehaviour());

            #endregion

            cameraManager.Add(cameraGameObject.Name, cameraGameObject);

            #endregion First Person

            #region Security

            //camera 2
            cameraGameObject = new GameObject(AppData.SECURITY_CAMERA_NAME);

            cameraGameObject.Transform
                = new Transform(null,
                null,
                new Vector3(0, 10, 50));

            //add camera (view, projection)
            cameraGameObject.AddComponent(new Camera(
                MathHelper.PiOver2 / 2,
                (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
                0.1f, 3500,
                new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)));

            //add rotation
            cameraGameObject.AddComponent(new CycledRotationBehaviour(
                AppData.SECURITY_CAMERA_ROTATION_AXIS,
                AppData.SECURITY_CAMERA_MAX_ANGLE,
                AppData.SECURITY_CAMERA_ANGULAR_SPEED_MUL,
                TurnDirectionType.Right));

            //adds FOV change on mouse scroll
            cameraGameObject.AddComponent(new CameraFOVController(AppData.CAMERA_FOV_INCREMENT_LOW));

            cameraManager.Add(cameraGameObject.Name, cameraGameObject);

            #endregion Security

            #region Snake Cameras

            //Front Camera
            snakeCameraGameObject = new GameObject(AppData.FRONT_CAMERA_NAME);

            snakeCameraGameObject.Transform
                = new Transform(null,
                AppData.DEFAULT_FRONT_CAMERA_ROTATION,
                AppData.DEFAULT_FRONT_CAMERA_TRANSLATION);

            //add camera (view, projection)
           var camera = new Camera(
                MathHelper.PiOver4,
                (float)_graphics.PreferredBackBufferWidth/ _graphics.PreferredBackBufferHeight,
                0.1f, 3500,
                new Viewport(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            cameraManager.Add(snakeCameraGameObject.Name, snakeCameraGameObject);

            snakeCameraGameObject.AddComponent(camera);

            //Back Camera
            cameraManager.Add(AppData.BACK_CAMERA_NAME, CloneModelGameObjectCamera(cameraGameObject, AppData.BACK_CAMERA_NAME, AppData.DEFAULT_BACK_CAMERA_ROTATION, AppData.DEFAULT_BACK_CAMERA_TRANSLATION));

            //Top Camera
            cameraManager.Add(AppData.TOP_CAMERA_NAME, CloneModelGameObjectCamera(cameraGameObject, AppData.TOP_CAMERA_NAME, AppData.DEFAULT_TOP_CAMERA_ROTATION, AppData.DEFAULT_TOP_CAMERA_TRANSLATION));

            //Bottom Camera
            cameraManager.Add(AppData.BOTTOM_CAMERA_NAME,CloneModelGameObjectCamera(cameraGameObject, AppData.BOTTOM_CAMERA_NAME, AppData.DEFAULT_BOTTOM_CAMERA_ROTATION, AppData.DEFAULT_BOTTOM_CAMERA_TRANSLATION));

            //Right Camera
            cameraManager.Add(AppData.RIGHT_CAMERA_NAME, CloneModelGameObjectCamera(cameraGameObject, AppData.RIGHT_CAMERA_NAME, AppData.DEFAULT_RIGHT_CAMERA_ROTATION, AppData.DEFAULT_RIGHT_CAMERA_TRANSLATION));

            //Left Camera
            cameraManager.Add(AppData.LEFT_CAMERA_NAME, CloneModelGameObjectCamera(cameraGameObject, AppData.LEFT_CAMERA_NAME, AppData.DEFAULT_LEFT_CAMERA_ROTATION, AppData.DEFAULT_LEFT_CAMERA_TRANSLATION));

            //Curve Camera
            cameraManager.Add(AppData.CURVE_CAMERA_NAME, CloneModelGameObjectCamera(cameraGameObject, AppData.CURVE_CAMERA_NAME, new Vector3(0,0,0), new Vector3(0, 0, 0)));

            cameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
            #endregion Snake Cameras
        }

        private void InitializeCollidableContent(float worldScale)
        {;
            InitializeBaseModel();
            InitilizeFood();
            InitilizeBomb();
        }

       
        public GameObject CloneModelGameObject(GameObject gameObject, string newName, Vector3 translation)
        {
            GameObject gameObjectClone = new GameObject(newName, gameObject.ObjectType, gameObject.RenderType);
            gameObjectClone.GameObjectType = gameObject.GameObjectType;

            gameObjectClone.Transform = new Transform(
                gameObject.Transform.Scale,
                gameObject.Transform.Rotation,
                translation
                );

            Renderer renderer = gameObject.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);
            gameObjectClone.AddComponent(cloneRenderer);


            return gameObjectClone;
        }

        private void InitializeNonCollidableContent(float worldScale)
        {
            //create sky
            InitializeSkyBox(worldScale);

            InitializeBaseModel();
        }

        private void InitializeBaseModel()
        {

            var gameObject = new GameObject("base ", ObjectType.Static, RenderType.Transparent);
            gameObject.GameObjectType = GameObjectType.Collectible;

            gameObject.Transform = new Transform(
                 new Vector3(45, 45, 45),
                Vector3.Zero,
                new Vector3(0, 0, 0)
                );
            var texture = Content.Load<Texture2D>("Assets/niall");
            var meshBase2 = new CubeMesh(_graphics.GraphicsDevice);

            gameObject.AddComponent(new Renderer(
                new GDBasicEffect(exitSignEffect),
                new Material(texture, 1f),
                meshBase2));

            sceneManager.ActiveScene.Add(gameObject);
        }

        public GameObject CloneModelGameObjectBase(GameObject gameObject, string newName, Vector3 translation, int opacity)
        {
            GameObject gameObjectClone = new GameObject(newName, gameObject.ObjectType, gameObject.RenderType);
            gameObjectClone.GameObjectType = gameObject.GameObjectType;

            gameObjectClone.Transform = new Transform(
                gameObject.Transform.Scale,
                gameObject.Transform.Rotation,
                translation
                );

            Renderer renderer = gameObject.GetComponent<Renderer>();
            Renderer cloneRenderer = new Renderer(renderer.Effect, renderer.Material, renderer.Mesh);

            cloneRenderer.Material.DiffuseColor = Color.MintCream;
            cloneRenderer.Material.Alpha = 1f / opacity;
            gameObjectClone.AddComponent(cloneRenderer);



            return gameObjectClone;
        }

        private void InitializeSnakeHead()
        {
            //game object
            var snakeGameObject = new GameObject(AppData.SNAKE_HEAD_NAME, ObjectType.Dynamic, RenderType.Opaque);
            snakeGameObject.GameObjectType = GameObjectType.Player;

            snakeGameObject.Transform = new Transform(
                AppData.SNAKE_GAMEOBJECTS_SCALE,
                Vector3.Zero,
                AppData.SNAKE_START_POSITION);
            var texture = Content.Load<Texture2D>(AppData.SNAKE_HEAD_TEXTURE_PATH);
            var meshBase = new SphereMesh(_graphics.GraphicsDevice);

            snakeGameObject.AddComponent(new Renderer(
                new GDBasicEffect(unlitEffect),
                new Material(texture, 1),
                meshBase));

            Application.Player = snakeGameObject;

            var collider = new CharacterCollider(snakeGameObject, true);

            snakeGameObject.AddComponent(collider);
            collider.AddPrimitive(
                new Box(
                    snakeGameObject.Transform.Translation,
                    snakeGameObject.Transform.Rotation,
                    AppData.SNAKE_GAMEOBJECTS_COLLIDER_SCALE
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            collider.Enable(snakeGameObject, false, 1);

            var snakeGameObjectTongue = new GameObject("snake tongue", ObjectType.Dynamic, RenderType.Opaque);

            snakeGameObjectTongue.Transform = new Transform(
                new Vector3(2.5f,0.6f,0.6f),
                Vector3.Zero,
                null);
            texture = Content.Load<Texture2D>(AppData.SNAKE_TONGUE_TEXTURE_PATH);
            var meshBase2 = new OctahedronMesh(_graphics.GraphicsDevice);

            snakeGameObjectTongue.AddComponent(new Renderer(
                new GDBasicEffect(litEffect),
                new Material(texture, 1),
                meshBase2));

            snakeGameObjectTongue.AddComponent(new SnakeTongueController(AppData.SNAKE_HEAD_TRANSLATE_AMOUNT));
            sceneManager.ActiveScene.Add(snakeGameObjectTongue);


            texture = Content.Load<Texture2D>(AppData.SNAKE_SKIN_TEXTURE_PATH);
            var snakeSkin = new Material(texture, 1);
            CubeMesh snakeBodyMesh = new CubeMesh(_graphics.GraphicsDevice);
            OctahedronMesh snakeTailMesh = new OctahedronMesh(_graphics.GraphicsDevice);
            snakeManager = new SnakeManager(this, snakeGameObject, snakeBodyMesh, snakeTailMesh, snakeSkin, AppData.SNAKE_DEFAULT_MOVE_SPEED, AppData.SNAKE_MULTIPLIER);
            Application.SnakeManager = snakeManager;

            snakeGameObject.Transform.SetRotation(0, 90, 0);
            snakeGameObject.AddComponent(new CollidableSnakeController());

        }

        private void InitilizeFood()
        {
    
            //game object
            foodGameObject = new GameObject("food 1", ObjectType.Static, RenderType.Opaque);
            foodGameObject.GameObjectType = GameObjectType.Consumable;

            foodGameObject.Transform = new Transform(
                AppData.SNAKE_GAMEOBJECTS_SCALE,
                Vector3.Zero,
                Vector3.Zero);
            var texture = textureDictionary[AppData.FOOD_TEXTURE_NAME];
            var meshBase = new SphereMesh(_graphics.GraphicsDevice);

            foodGameObject.AddComponent(new Renderer(
                new GDBasicEffect(unlitEffect),
                new Material(texture, 1),
                meshBase));

            Collider collider = new FoodCollider(foodGameObject, true,true);
            collider.AddPrimitive(
                new Sphere(
                    foodGameObject.Transform.Translation,
                    AppData.SCALE_AMOUNT / 2f
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            collider.Enable(foodGameObject, true, 1);
            foodGameObject.AddComponent(collider);

            foodGameObject.AddComponent(new FoodController(AppData.FOOD_ROTATE_SPEED));
        }

        private void InitilizeBomb()
        {

            //game object
            bombGameObject = new GameObject("bomb 1", ObjectType.Static, RenderType.Opaque);
            bombGameObject.GameObjectType = GameObjectType.Consumable;

            bombGameObject.Transform = new Transform
                (
                AppData.SNAKE_GAMEOBJECTS_SCALE,
                Vector3.Zero,
                Vector3.Zero);
            var texture = Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate2");
            var meshBase = new TetrahedronMesh(_graphics.GraphicsDevice);

            bombGameObject.AddComponent(new Renderer(
                new GDBasicEffect(unlitEffect),
                new Material(texture, 1, Color.Green),
                meshBase));

            Collider collider = new FoodCollider(bombGameObject, true, true);
            collider.AddPrimitive(
                new Sphere(
                    bombGameObject.Transform.Translation,
                    AppData.SCALE_AMOUNT / 2f
                    ),
                new MaterialProperties(0.8f, 0.8f, 0.7f)
                );

            collider.Enable(bombGameObject, true, 1);
            bombGameObject.AddComponent(collider);

            bombGameObject.AddComponent(new BombController(AppData.BOMB_ROTATE_SPEED));
        }

        private void InitializeSkyBox(float worldScale)
        {
            float halfWorldScale = worldScale / 2.0f;

            GameObject quad = null;
            var gdBasicEffect = new GDBasicEffect(unlitEffect);
            var quadMesh = new QuadMesh(_graphics.GraphicsDevice);
            var texture = Content.Load<Texture2D>(AppData.BACKGROUND_TEXTURE_PATH);

            //skybox - back face
            quad = new GameObject("skybox back face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1), null, new Vector3(0, 0, -halfWorldScale));
            
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1,Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);

            //skybox - left face
            quad = new GameObject("skybox left face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1),
                new Vector3(0, 90, 0), new Vector3(-halfWorldScale, 0, 0));
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1, Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);

            //skybox - right face
            quad = new GameObject("skybox right face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1),
                new Vector3(0, -90, 0), new Vector3(halfWorldScale, 0, 0));
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1, Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);

            //skybox - top face
            quad = new GameObject("skybox top face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1),
                new Vector3(90, -90, 0), new Vector3(0, halfWorldScale, 0));
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1, Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);


            //skybox - bottom face
            quad = new GameObject("skybox bottom face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1),
                new Vector3(-90, 90, 0), new Vector3(0, -halfWorldScale, 0));
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1, Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);

            //skybox - front face
            quad = new GameObject("skybox front face");
            quad.Transform = new Transform(new Vector3(worldScale, worldScale, 1),
                new Vector3(0, -180, 0), new Vector3(0, 0, halfWorldScale));
            quad.AddComponent(new Renderer(gdBasicEffect, new Material(texture, 1, Color.Blue), quadMesh));
            sceneManager.ActiveScene.Add(quad);
        }

        #endregion Actions - Level Specific

        #region Actions - Engine Specific

        private void InitializeEngine(Vector2 resolution, bool isMouseVisible, bool isCursorLocked)
        {
            //add support for mouse etc
            InitializeInput();

            //add game effects
            InitializeEffects();

            //add dictionaries to store and access content
            InitializeDictionaries();

            //add camera, scene manager
            InitializeManagers();

            //share some core references
            InitializeGlobals();

            //set screen properties (incl mouse)
            InitializeScreen(resolution, isMouseVisible, isCursorLocked);

            //add game cameras
            InitializeCameras();
        }

        private void InitializeGlobals()
        {
            //Globally shared commonly accessed variables
            Application.Main = this;
            Application.GraphicsDeviceManager = _graphics;
            Application.GraphicsDevice = _graphics.GraphicsDevice;
            Application.Content = Content;

            //Add access to managers from anywhere in the code
            Application.CameraManager = cameraManager;
            Application.SceneManager = sceneManager;
            Application.SoundManager = soundManager;
            Application.PhysicsManager = physicsManager;

            Application.UISceneManager = uiManager;
            Application.MenuSceneManager = menuManager;
            Application.StateManager = stateManager;
        }

        private void InitializeInput()
        {
            //Globally accessible inputs
            Input.Keys = new KeyboardComponent(this);
            Components.Add(Input.Keys);
            Input.Mouse = new MouseComponent(this);
            Components.Add(Input.Mouse);
            Input.Gamepad = new GamepadComponent(this);
            Components.Add(Input.Gamepad);
        }

        /// <summary>
        /// Sets game window dimensions and shows/hides the mouse
        /// </summary>
        /// <param name="resolution"></param>
        /// <param name="isMouseVisible"></param>
        /// <param name="isCursorLocked"></param>
        private void InitializeScreen(Vector2 resolution, bool isMouseVisible, bool isCursorLocked)
        {
            Screen screen = new Screen();

            //set resolution
            screen.Set(resolution, isMouseVisible, isCursorLocked);

            //set global for re-use by other entities
            Application.Screen = screen;

            //set starting mouse position i.e. set mouse in centre at startup
            Input.Mouse.Position = screen.ScreenCentre;

            ////calling set property
            //_graphics.PreferredBackBufferWidth = (int)resolution.X;
            //_graphics.PreferredBackBufferHeight = (int)resolution.Y;
            //IsMouseVisible = isMouseVisible;
            //_graphics.ApplyChanges();
        }

        private void InitializeManagers()
        {
            //add event dispatcher for system events - the most important element!!!!!!
            eventDispatcher = new EventDispatcher(this);
            //add to Components otherwise no Update() called
            Components.Add(eventDispatcher);

            //add support for multiple cameras and camera switching
            cameraManager = new CameraManager(this);
            //add to Components otherwise no Update() called
            Components.Add(cameraManager);

            //big kahuna nr 1! this adds support to store, switch and Update() scene contents
            sceneManager = new SceneManager<Scene>(this);
            //add to Components otherwise no Update()
            Components.Add(sceneManager);

            //big kahuna nr 2! this renders the ActiveScene from the ActiveCamera perspective
            renderManager = new RenderManager(this, new ForwardSceneRenderer(_graphics.GraphicsDevice));
            renderManager.DrawOrder = 1;
            Components.Add(renderManager);

            //add support for playing sounds
            soundManager = new SoundManager();
            //why don't we add SoundManager to Components? Because it has no Update()
            //wait...SoundManager has no update? Yes, playing sounds is handled by an internal MonoGame thread - so we're off the hook!

            //add the physics manager update thread
            physicsManager = new PhysicsManager(this, AppData.GRAVITY);
            Components.Add(physicsManager);

            #region Collision - Picking

            //picking support using physics engine
            //this predicate lets us say ignore all the other collidable objects except interactables and consumables
            Predicate<GameObject> collisionPredicate =
                (collidableObject) =>
                {
                    if (collidableObject != null)
                        return collidableObject.GameObjectType
                        == GameObjectType.Interactable
                        || collidableObject.GameObjectType == GameObjectType.Consumable
                        || collidableObject.GameObjectType == GameObjectType.Collectible;
                    return false;
                };

            pickingManager = new PickingManager(this,
                AppData.PICKING_MIN_PICK_DISTANCE,
                AppData.PICKING_MAX_PICK_DISTANCE,
                collisionPredicate);
            Components.Add(pickingManager);

            #endregion

            #region Game State

            //add state manager for inventory and countdown
            stateManager = new MyStateManager(this, new SnakeLevelsData(AppData.DEFAULT_FOOD_EACH_LEVEL, AppData.DEFAULT_BOMB_EACH_LEVEL, AppData.START_TIMES_EACH_LEVEL));
            Components.Add(stateManager);

            #endregion

            #region UI

            uiManager = new SceneManager<Scene2D>(this);
            uiManager.StatusType = StatusType.Off;
            uiManager.IsPausedOnPlay = false;
            Components.Add(uiManager);

            var uiRenderManager = new Render2DManager(this, _spriteBatch, uiManager);
            uiRenderManager.StatusType = StatusType.Off;
            uiRenderManager.DrawOrder = 2;
            uiRenderManager.IsPausedOnPlay = false;
            Components.Add(uiRenderManager);

            #endregion

            #region Menu

            menuManager = new SceneManager<Scene2D>(this);
            menuManager.StatusType = StatusType.Updated;
            menuManager.IsPausedOnPlay = true;
            Components.Add(menuManager);

            var menuRenderManager = new Render2DManager(this, _spriteBatch, menuManager);
            menuRenderManager.StatusType = StatusType.Drawn;
            menuRenderManager.DrawOrder = 3;
            menuRenderManager.IsPausedOnPlay = true;
            Components.Add(menuRenderManager);

            #endregion
        }

        private void InitializeDictionaries()
        {
            //TODO - add texture dictionary, soundeffect dictionary, model dictionary
            InitilizeButtonTextTranslationsDictionary();
            InitilizeCurveDictionary();

            textureDictionary = new ContentDictionary<Texture2D>();
            fontDictionary = new ContentDictionary<SpriteFont>();


        }

        private void InitilizeCurveDictionary()
        {
            curveDictionary = new Dictionary<string, Curve3D>();

            Curve3D curve3DTranslation = new Curve3D(CurveLoopType.Oscillate);

            for (int i = 0; i < AppData.CURVE_TRANSLATIONS.Length; i++)
            {
                curve3DTranslation.Add(AppData.CURVE_TRANSLATIONS[i], AppData.CURVE_TIME_SPAN * i);
            }

            Curve3D curve3DRotation = new Curve3D(CurveLoopType.Oscillate);

            for (int i = 0; i < AppData.CURVE_ROTATIONS.Length; i++)
            {
                curve3DRotation.Add(AppData.CURVE_ROTATIONS[i], AppData.CURVE_TIME_SPAN * i);
            }

            curveDictionary.Add(AppData.INTRO_CURVE_TRANSLATIONS_NAME, curve3DTranslation);
            curveDictionary.Add(AppData.INTRO_CURVE_ROTATIONS_NAME, curve3DRotation);
        }

        private void InitilizeButtonTextTranslationsDictionary()
        {

            #region Main Menu Button
            menuButtonDictionary = new Dictionary<string, MenuButton>();
            menuButtonDictionary.Add(AppData.START_BUTTON_NAME, new MenuButton(AppData.START_BUTTON_TRANSLATION, AppData.START_BUTTON_TEXT_OFFSET, AppData.START_BUTTON_COLOR, AppData.START_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnLevelsScene, new object[] {AppData.LEVELS_SCENE_NAME})));
            menuButtonDictionary.Add(AppData.AUDIO_BUTTON_NAME, new MenuButton(AppData.AUDIO_BUTTON_TRANSLATION, AppData.AUDIO_BUTTON_TEXT_OFFSET, AppData.AUDIO_BUTTON_COLOR, AppData.AUDIO_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnAudioScene, new object[] { AppData.AUDIO_SCENE_NAME})));
            menuButtonDictionary.Add(AppData.CONTROLS_BUTTON_NAME, new MenuButton(AppData.CONTROLS_BUTTON_TRANSLATION, AppData.CONTROLS_BUTTON_TEXT_OFFSET, AppData.CONTROLS_BUTTON_COLOR, AppData.CONTROLS_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnControlsScene, new object[] { AppData.CONTROLS_SCENE_NAME })));
            menuButtonDictionary.Add(AppData.EXIT_BUTTON_NAME, new MenuButton(AppData.EXIT_BUTTON_TRANSLATION, AppData.EXIT_BUTTON_TEXT_OFFSET, AppData.EXIT_BUTTON_COLOR, AppData.EXIT_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnGameExit)));
            #endregion Main Menu Button

            #region Levels Menu Button
            menuButtonDictionary.Add(AppData.LEVEL_ONE_BUTTON_NAME, new MenuButton(AppData.LEVEL_ONE_BUTTON_TRANSLATION, AppData.LEVEL_GAME_BUTTON_TEXT_OFFSET, AppData.START_BUTTON_COLOR, AppData.LEVEL_ONE_BUTTON_TEXT, new EventData(EventCategoryType.StateManager, EventActionType.StartOfLevel, new object[] { AppData.LEVEL_ONE })));
            menuButtonDictionary.Add(AppData.LEVEL_TWO_BUTTON_NAME, new MenuButton(AppData.LEVEL_TWO_BUTTON_TRANSLATION, AppData.LEVEL_GAME_BUTTON_TEXT_OFFSET, AppData.START_BUTTON_COLOR, AppData.LEVEL_TWO_BUTTON_TEXT, new EventData(EventCategoryType.StateManager, EventActionType.StartOfLevel, new object[] { AppData.LEVEL_TWO })));
            menuButtonDictionary.Add(AppData.LEVEL_THREE_BUTTON_NAME, new MenuButton(AppData.LEVEL_THREE_BUTTON_TRANSLATION, AppData.LEVEL_GAME_BUTTON_TEXT_OFFSET, AppData.START_BUTTON_COLOR, AppData.LEVEL_THREE_BUTTON_TEXT, new EventData(EventCategoryType.StateManager, EventActionType.StartOfLevel, new object[] { AppData.LEVEL_THREE })));
            #endregion Levels Menu Button

            #region Pause Menu Button
            menuButtonDictionary.Add(AppData.RESUME_BUTTON_NAME, new MenuButton(AppData.RESUME_BUTTON_TRANSLATION, AppData.RESUME_BUTTON_TEXT_OFFSET, AppData.START_BUTTON_COLOR, AppData.RESUME_BUTTON_TEXT, new EventData(EventCategoryType.Menu, EventActionType.OnPlay)));
            menuButtonDictionary.Add(AppData.MAIN_MENU_BUTTON_NAME, new MenuButton(AppData.MAIN_MENU_BUTTON_TRANSLATION, AppData.MAIN_MENU_BUTTON_TEXT_OFFSET, AppData.EXIT_BUTTON_COLOR, AppData.MAIN_MENU_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnMainMenuScene, new object[] { AppData.MAIN_MENU_SCENE_NAME })));
            #endregion Pause Menu Button

            #region Back Button
            menuButtonDictionary.Add(AppData.BACK_BUTTON_NAME, new MenuButton(AppData.BACK_BUTTON_TRANSLATION, AppData.BACK_BUTTON_TEXT_OFFSET, AppData.BACK_BUTTON_COLOR, AppData.BACK_BUTTON_TEXT, new EventData(EventCategoryType.SceneManager, EventActionType.OnMainMenuScene, new object[] { AppData.MAIN_MENU_SCENE_NAME })));
            #endregion Back Button
        }

        private void InitializeDebug(bool showCollisionSkins = true)
        {
            //intialize the utility component
            var perfUtility = new PerfUtility(this, _spriteBatch,
                new Vector2(10, 10),
                new Vector2(0, 22));

            //set the font to be used
            var spriteFont = Content.Load<SpriteFont>("Assets/Fonts/Perf");

            //add components to the info list to add UI information
            float headingScale = 1f;
            float contentScale = 0.9f;
            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Performance ------------------------------", Color.Yellow, headingScale * Vector2.One));
            perfUtility.infoList.Add(new FPSInfo(_spriteBatch, spriteFont, "FPS:", Color.White, contentScale * Vector2.One));
            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Camera -----------------------------------", Color.Yellow, headingScale * Vector2.One));
            perfUtility.infoList.Add(new CameraNameInfo(_spriteBatch, spriteFont, "Name:", Color.White, contentScale * Vector2.One));

            var infoFunction = (Transform transform) =>
            {
                return transform.Translation.GetNewRounded(1).ToString();
            };

            perfUtility.infoList.Add(new TransformInfo(_spriteBatch, spriteFont, "Pos:", Color.White, contentScale * Vector2.One,
                ref Application.CameraManager.ActiveCamera.transform, infoFunction));

            infoFunction = (Transform transform) =>
            {
                return transform.Rotation.GetNewRounded(1).ToString();
            };

            perfUtility.infoList.Add(new TransformInfo(_spriteBatch, spriteFont, "Rot:", Color.White, contentScale * Vector2.One,
                ref Application.CameraManager.ActiveCamera.transform, infoFunction));

            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Object -----------------------------------", Color.Yellow, headingScale * Vector2.One));
            perfUtility.infoList.Add(new ObjectInfo(_spriteBatch, spriteFont, "Objects:", Color.White, contentScale * Vector2.One));
            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Hints -----------------------------------", Color.Yellow, headingScale * Vector2.One));
            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Use mouse scroll wheel to change security camera FOV, F1-F4 for camera switch", Color.White, contentScale * Vector2.One));
            perfUtility.infoList.Add(new TextInfo(_spriteBatch, spriteFont, "Use Up and Down arrow to see progress bar change", Color.White, contentScale * Vector2.One));

            //add to the component list otherwise it wont have its Update or Draw called!
            // perfUtility.StatusType = StatusType.Drawn | StatusType.Updated;
            perfUtility.DrawOrder = 3;
            Components.Add(perfUtility);

            if (showCollisionSkins)
            {
                var physicsDebugDrawer = new PhysicsDebugDrawer(this);
                physicsDebugDrawer.DrawOrder = 4;
                Components.Add(physicsDebugDrawer);
            }
        }

        #endregion Actions - Engine Specific

        #region Actions - Update, Draw

        protected override void Update(GameTime gameTime)
        {
            #region Menu On/Off with U/P

            if (Input.Keys.WasJustPressed(Keys.P))
            {
                menuManager.SetActiveScene(AppData.PAUSE_SCENE_NAME);
                EventDispatcher.Raise(new EventData(EventCategoryType.Menu,
                    EventActionType.OnPause));
            }

            #endregion

#if DEMO

            #region Demo - UI - progress bar

            if (Input.Keys.WasJustPressed(Keys.Up))
            {
                object[] parameters = { "progress bar - health - 1", 1 };
                EventDispatcher.Raise(new EventData(EventCategoryType.UI, EventActionType.OnHealthDelta, parameters));
            }
            else if (Input.Keys.WasJustPressed(Keys.Down))
            {
                object[] parameters = { "progress bar - health - 1", -1 };
                EventDispatcher.Raise(new EventData(EventCategoryType.UI, EventActionType.OnHealthDelta, parameters));
            }

            #endregion

            #region Demo - sound

            if (Input.Keys.WasJustPressed(Keys.B))
            {
                object[] parameters = { "boom1" };
                EventDispatcher.Raise(
                    new EventData(EventCategoryType.Game,
                    EventActionType.OnWin,
                    parameters));

                //    Application.SoundManager.Play2D("boom1");
            }

            #endregion

            #region Camera switching
            if (Input.Keys.WasJustPressed(Keys.Up))
            {
                if (
                    cameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.RIGHT_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.LEFT_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME
                    )
                {
                    cameraManager.SetActiveCamera(AppData.TOP_CAMERA_NAME);
                    UpdateCameraUI(AppData.TOP_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
                    UpdateCameraUI(AppData.BACK_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
                    UpdateCameraUI(AppData.FRONT_CAMERA_UI_TEXT);
                }
            }
            else if (Input.Keys.WasJustPressed(Keys.Right))
            {
                if (
                    cameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME
                    )
                {
                    cameraManager.SetActiveCamera(AppData.RIGHT_CAMERA_NAME);
                    UpdateCameraUI(AppData.RIGHT_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.RIGHT_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
                    UpdateCameraUI(AppData.BACK_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.LEFT_CAMERA_NAME);
                    UpdateCameraUI(AppData.LEFT_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.LEFT_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
                    UpdateCameraUI(AppData.FRONT_CAMERA_UI_TEXT);
                }

            }
            else if (Input.Keys.WasJustPressed(Keys.Left))
            {
                if (
                    cameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME ||
                    cameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME
                   )
                {
                    cameraManager.SetActiveCamera(AppData.LEFT_CAMERA_NAME);
                    UpdateCameraUI(AppData.LEFT_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.LEFT_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
                    UpdateCameraUI(AppData.BACK_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.RIGHT_CAMERA_NAME);
                    UpdateCameraUI(AppData.RIGHT_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.RIGHT_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
                    UpdateCameraUI(AppData.FRONT_CAMERA_UI_TEXT);
                }
               

            }
            else if (Input.Keys.WasJustPressed(Keys.Down))
            {
                if (
                        cameraManager.ActiveCameraName == AppData.FRONT_CAMERA_NAME ||
                        cameraManager.ActiveCameraName == AppData.RIGHT_CAMERA_NAME ||
                        cameraManager.ActiveCameraName == AppData.LEFT_CAMERA_NAME ||
                        cameraManager.ActiveCameraName == AppData.BACK_CAMERA_NAME
                   )
                {
                    cameraManager.SetActiveCamera(AppData.BOTTOM_CAMERA_NAME);
                    UpdateCameraUI(AppData.BOTTOM_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.TOP_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.FRONT_CAMERA_NAME);
                    UpdateCameraUI(AppData.FRONT_CAMERA_UI_TEXT);
                }
                else if (cameraManager.ActiveCameraName == AppData.BOTTOM_CAMERA_NAME)
                {
                    cameraManager.SetActiveCamera(AppData.BACK_CAMERA_NAME);
                    UpdateCameraUI(AppData.BACK_CAMERA_UI_TEXT);
                }
            }

            #endregion Camera switching

            #region Demo - Gamepad

            var thumbsL = Input.Gamepad.ThumbSticks(false);
            //   System.Diagnostics.Debug.WriteLine(thumbsL);

            var thumbsR = Input.Gamepad.ThumbSticks(false);
            //     System.Diagnostics.Debug.WriteLine(thumbsR);

            //    System.Diagnostics.Debug.WriteLine($"A: {Input.Gamepad.IsPressed(Buttons.A)}");

            #endregion Demo - Gamepad

            #region Demo - Raising events using GDEvent

            if (Input.Keys.WasJustPressed(Keys.E))
                OnChanged.Invoke(this, null); //passing null for EventArgs but we'll make our own class MyEventArgs::EventArgs later

            #endregion

#endif
            //fixed a bug with components not getting Update called
            base.Update(gameTime);
        }

        private void UpdateCameraUI(string uiText)
        {
            GameObject cameraUIGameObject = Application.UISceneManager.ActiveScene.Find((uiElement) => uiElement.Name == AppData.CAMERA_UI_TEXT_NAME);

            var material2D = (TextMaterial2D)cameraUIGameObject.GetComponent<Renderer2D>().Material;
            material2D.StringBuilder.Clear();
            material2D.StringBuilder.Append(uiText);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        #endregion Actions - Update, Draw
    }
}