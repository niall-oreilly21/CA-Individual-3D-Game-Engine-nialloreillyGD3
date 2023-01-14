#region Pre-compiler directives

//#define DEMO
#define HI_RES

#endregion

using GD.Engine;
using GD.Engine.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GD.App
{
#if DEMO

    public enum CameraIDType : sbyte
    {
        First,
        Third,
        Security
    }

#endif

    public class AppData
    {
        #region Graphics

#if HI_RES
        public static readonly Vector2 APP_RESOLUTION = Resolutions.SixteenNine.HD;
#else
        public static readonly Vector2 APP_RESOLUTION = Resolutions.FourThree.VGA;
#endif

        #endregion Graphics

        #region World Scale

        public static readonly float SKYBOX_WORLD_SCALE = 2000;

        #endregion World Scale

        #region Camera - General

        public static readonly float CAMERA_FOV_INCREMENT_LOW = 1;
        public static readonly float CAMERA_FOV_INCREMENT_MEDIUM = 2;
        public static readonly float CAMERA_FOV_INCREMENT_HIGH = 4;

        #endregion

        #region Camera - First Person

        public static readonly string FIRST_PERSON_CAMERA_NAME = "fpc 1";
        public static readonly float FIRST_PERSON_MOVE_SPEED = 0.036f;
        public static readonly float FIRST_PERSON_STRAFE_SPEED = 0.6f * FIRST_PERSON_MOVE_SPEED;
        public static readonly Vector3 FIRST_PERSON_DEFAULT_CAMERA_POSITION = new Vector3(0, 15, 5);

        public static readonly float FIRST_PERSON_CAMERA_FCP = 3000;
        public static readonly float FIRST_PERSON_CAMERA_NCP = 0.1f;

        public static readonly float FIRST_PERSON_HALF_FOV = MathHelper.PiOver2 / 2.0f;

        public static readonly float FIRST_PERSON_CAMERA_SMOOTH_FACTOR = 0.1f;

        public static readonly float PLAYER_COLLIDABLE_JUMP_HEIGHT = 5;

        #endregion Camera - First Person

        #region Camera - Third Person

        public static readonly string THIRD_PERSON_CAMERA_NAME = "third person camera";

        #endregion

        #region Camera - Security Camera

        public static readonly float SECURITY_CAMERA_MAX_ANGLE = 180;
        public static readonly float SECURITY_CAMERA_ANGULAR_SPEED_MUL = 300;
        public static readonly Vector3 SECURITY_CAMERA_ROTATION_AXIS = new Vector3(0, 1, 0);
        public static readonly string SECURITY_CAMERA_NAME = "security camera 1";

        #endregion Camera - Security Camera

        #region Camera - Curve

        public static readonly string CURVE_CAMERA_NAME = "curve camera 1";

        #endregion

        #region Input Key Mappings

        public static readonly Keys[] KEYS_ONE = { Keys.W, Keys.S, Keys.A, Keys.D };
        public static readonly Keys[] KEYS_TWO = { Keys.U, Keys.J, Keys.H, Keys.K };

        #endregion Input Key Mappings

        #region Movement Constants

        public static readonly float PLAYER_MOVE_SPEED = 0.1f;
        private static readonly float PLAYER_STRAFE_SPEED_MULTIPLIER = 0.75f;
        public static readonly float PLAYER_STRAFE_SPEED = PLAYER_STRAFE_SPEED_MULTIPLIER * PLAYER_MOVE_SPEED;

        //can use either same X-Y rotation for camera controller or different
        public static readonly float PLAYER_ROTATE_SPEED_SINGLE = 0.001f;

        //why bother? can you tilt your head at the same speed as you rotate it?
        public static readonly Vector2 PLAYER_ROTATE_SPEED_VECTOR2 = new Vector2(0.004f, 0.003f);

        #endregion Movement Constants

        #region Picking

        public static readonly float PICKING_MIN_PICK_DISTANCE = 2;
        public static readonly float PICKING_MAX_PICK_DISTANCE = 100;

        #endregion

        #region Core

        public static readonly double MAX_GAME_TIME_IN_MSECS = 2500; //180000
        public static readonly Vector3 GRAVITY = new Vector3(0, -9.81f, 0);

        #endregion

        #region Snake

        #region Snake Attributes
        public static readonly Vector3 SNAKE_START_POSITION = new Vector3(0, 0, 0);
        public static readonly float SNAKE_DEFAULT_MOVE_SPEED = 600f;
        private static readonly float SNAKE_MULTIPLIER_SPEED = 0.04f;
        public static readonly float SNAKE_MULTIPLIER = SNAKE_DEFAULT_MOVE_SPEED * SNAKE_MULTIPLIER_SPEED;
        public static readonly float SNAKE_HEAD_TRANSLATE_AMOUNT = 0.7f;
        public static readonly string SNAKE_HEAD_NAME = "Snake head";
        #endregion Snake Attributes

        #region Snake World

        #region Cube Dimensions
        private static readonly int SNAKE_GAME_SIZE = 21;

        public static readonly float SNAKE_GAME_MIN_BOUNDARY = -22.5f;
        public static readonly float SNAKE_GAME_MAX_BOUNDARY = 22.5f;
        #endregion Cube Dimensions

        #region GameObjects Scale
        public static readonly float SCALE_AMOUNT = 3f;
        public static readonly float COLLIDER_SCALE_AMOUNT = 2.5f;
        public static readonly Vector3 SNAKE_GAMEOBJECTS_SCALE =  new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, SCALE_AMOUNT);
        public static readonly Vector3 SNAKE_GAMEOBJECTS_COLLIDER_SCALE = new Vector3(COLLIDER_SCALE_AMOUNT, COLLIDER_SCALE_AMOUNT, COLLIDER_SCALE_AMOUNT);
        #endregion GameObjects Scale

        #endregion Snake World

        #region Cameras
        public static readonly float CAMERA_POSITION = 77.5f;
        public static readonly float CAMERA_ROTATION = 90f;
        public static readonly float CAMERA_HEIGHT = 0f;

        #region Front Camera
        public static readonly string FRONT_CAMERA_NAME = "front camera";
        public static readonly Vector3 DEFAULT_FRONT_CAMERA_TRANSLATION = new Vector3(CAMERA_HEIGHT, CAMERA_HEIGHT, CAMERA_POSITION);
        public static readonly Vector3 DEFAULT_FRONT_CAMERA_ROTATION = Vector3.Zero;
        #endregion Front Camera

        #region Back Camera
        public static readonly string BACK_CAMERA_NAME = "back camera";
        public static readonly Vector3 DEFAULT_BACK_CAMERA_TRANSLATION = new Vector3(CAMERA_HEIGHT, CAMERA_HEIGHT, -CAMERA_POSITION);
        public static readonly Vector3 DEFAULT_BACK_CAMERA_ROTATION = new Vector3(0, CAMERA_ROTATION * 2, 0);
        #endregion Back Camera

        #region Top Camera
        public static readonly string TOP_CAMERA_NAME = "top camera";
        public static readonly Vector3 DEFAULT_TOP_CAMERA_TRANSLATION = new Vector3(CAMERA_HEIGHT, CAMERA_POSITION, CAMERA_HEIGHT);
        public static readonly Vector3 DEFAULT_TOP_CAMERA_ROTATION = new Vector3(-CAMERA_ROTATION, 0, 0);
        #endregion Top Camera

        #region Bottom Camera
        public static readonly string BOTTOM_CAMERA_NAME = "bottom camera";
        public static readonly Vector3 DEFAULT_BOTTOM_CAMERA_TRANSLATION = new Vector3(CAMERA_HEIGHT, -CAMERA_POSITION, CAMERA_HEIGHT);
        public static readonly Vector3 DEFAULT_BOTTOM_CAMERA_ROTATION = new Vector3(CAMERA_ROTATION, 0, 0);
        #endregion Bottom Camera

        #region Right Camera
        public static readonly string RIGHT_CAMERA_NAME = "right camera";
        public static readonly Vector3 DEFAULT_RIGHT_CAMERA_TRANSLATION = new Vector3(CAMERA_POSITION, CAMERA_HEIGHT, CAMERA_HEIGHT);
        public static readonly Vector3 DEFAULT_RIGHT_CAMERA_ROTATION = new Vector3(0, CAMERA_ROTATION, 0);
        #endregion Right Camera

        #region Left Camera
        public static readonly string LEFT_CAMERA_NAME = "left camera";
        public static readonly Vector3 DEFAULT_LEFT_CAMERA_TRANSLATION = new Vector3(-CAMERA_POSITION, CAMERA_HEIGHT, CAMERA_HEIGHT);
        public static readonly Vector3 DEFAULT_LEFT_CAMERA_ROTATION = new Vector3(0, -CAMERA_ROTATION, 0);
        #endregion Left Camera


        #region Intro Camera

        #region Curve Translations
        public static readonly Vector3[] CURVE_TRANSLATIONS = {DEFAULT_FRONT_CAMERA_TRANSLATION, DEFAULT_RIGHT_CAMERA_TRANSLATION, DEFAULT_BACK_CAMERA_TRANSLATION, DEFAULT_LEFT_CAMERA_TRANSLATION,DEFAULT_FRONT_CAMERA_TRANSLATION };
        #endregion Curve Translations

        #region Curve Rotations
        //Last two rotation can't be a minus as Curve Behaviour will track to the wrong location
        public static readonly Vector3[] CURVE_ROTATIONS = { DEFAULT_FRONT_CAMERA_ROTATION, DEFAULT_RIGHT_CAMERA_ROTATION, DEFAULT_BACK_CAMERA_ROTATION, new Vector3(0, 270, 0), new Vector3(0, 360, 0) };
        #endregion Curve Rotations

        #region Curve Rotations
        public static readonly int CURVE_TIME_SPAN = 4000;
        #endregion Curve Rotations

        #endregion Intro Camera
        #endregion Cameras

        #region Consumables

        #region Food
        public static readonly string FOOD_BASE_NAME = "food";
        public static readonly float FOOD_ROTATE_SPEED = 0.05f;
        public static readonly int DEFAULT_INITIAL_FOOD = 5;
        #endregion Food

        #region Bomb
        public static readonly string BOMB_BASE_NAME = "bomb";
        public static readonly float BOMB_ROTATE_SPEED = 0.03f;
        public static readonly int DEFAULT_INITIAL_BOMBS = 2;
        #endregion Bomb

        #endregion Consumables

        #region Game State Manager Data
        public static readonly float MAX_SNAKE_LEVEL_TIME = 70f;
        #endregion Game State Manager Data

        #region Menus

        #region Button Attributes
        public static readonly float BUTTON_HOVER_SCALE_BY = 0.1f;
        public static readonly float BUTTON_HOVER_OFFSET = 12f;
        public static readonly Vector2 BUTTON_SCALE = new Vector2(1f, 1f);
        public static readonly Vector2 BUTTON_TEXT_TRANSLATION = new Vector2(70f, 5f);
        #endregion button Attributes

        #region Menu Background
        public static readonly string SNAKE_MENU_BACKGROUND_TEXTURE_PATH = "Assets/Textures/Menu/Backgrounds/snake_background";
        public static readonly string BACKGROUND_NAME = "background";
        #endregion Menu Background

        #region Controls
        public static readonly string SNAKE_MENU_BUTTON_TEXTURE_PATH = "Assets/Textures/Menu/Controls/snake_button_texture";
        #endregion Controls

        #region Main Menu

        #region Start Game Button
        public static readonly string START_GAME_BUTTON_NAME = "Start Game Button";
        public static readonly Vector2 START_GAME_BUTTON_TRANSLATION = new Vector2(0f, 150f);
        public static readonly Vector2 START_GAME_BUTTON_TEXT_OFFSET = new Vector2(65f, 5f);
        public static readonly Color START_GAME_BUTTON_COLOR = Color.MediumPurple;
        public static readonly string START_GAME_BUTTON_TEXT = "START";
        #endregion Start Game Button

        #region Audio Game Button
        public static readonly string AUDIO_GAME_BUTTON_NAME = "Audio Game Button";
        public static readonly Vector2 AUDIO_GAME_BUTTON_TRANSLATION = new Vector2(0f, 50f);
        public static readonly Vector2 AUDIO_GAME_BUTTON_TEXT_OFFSET = new Vector2(70f, 5f);
        public static readonly Color AUDIO_GAME_BUTTON_COLOR = Color.Gold;
        public static readonly string AUDIO_GAME_BUTTON_TEXT = "AUDIO";
        #endregion Audio Game Button

        #region Controls Game Button
        public static readonly string CONTROLS_GAME_BUTTON_NAME = "Controls Game Button";
        public static readonly Vector2 CONTROLS_GAME_BUTTON_TRANSLATION = new Vector2(0f, -50f);
        public static readonly Vector2 CONTROLS_GAME_BUTTON_TEXT_OFFSET = new Vector2(5f, 5f);
        public static readonly Color CONTROLS_GAME_BUTTON_COLOR = Color.Green;
        public static readonly string CONTROLS_GAME_BUTTON_TEXT = "CONTROLS";
        #endregion Controls Game Button

        #region Exit Game Button
        public static readonly string EXIT_GAME_BUTTON_NAME = "Exit Game Button";
        public static readonly Vector2 EXIT_GAME_BUTTON_TRANSLATION = new Vector2(0f, -150f);
        public static readonly Vector2 EXIT_GAME_BUTTON_TEXT_OFFSET = new Vector2(90f, 5f);
        public static readonly Color EXIT_GAME_BUTTON_COLOR = Color.Red;
        public static readonly string EXIT_GAME_BUTTON_TEXT = "EXIT";
        #endregion Controls Game Button

        #endregion Main Menu


        #region End Menu
        public static readonly string END_MENU_NAME = "end menu";
        #endregion End Menu

        #endregion Menus

        #region Snake State Manager
        public static readonly int TOTAL_LEVELS = 3;

        public static readonly int LEVEL_ONE = 1;
        public static readonly int LEVEL_TWO = 2;
        public static readonly int LEVEL_THREE = 3;

        #region Food Level Default Figures
        private static readonly int DEFAULT_FOOD_LEVEL_ONE = 20;
        private static readonly int DEFAULT_FOOD_LEVEL_TWO = 15;
        private static readonly int DEFAULT_FOOD_LEVEL_THREE = 5;

        public static readonly int[] DEFAULT_FOOD_EACH_LEVEL = { DEFAULT_FOOD_LEVEL_ONE, DEFAULT_FOOD_LEVEL_TWO, DEFAULT_FOOD_LEVEL_THREE };
        #endregion Food Level Default Figures

        #region Bomb Level Default Figures
        private static readonly int DEFAULT_BOMB_LEVEL_ONE = 0;
        private static readonly int DEFAULT_BOMB_LEVEL_TWO = 15;
        private static readonly int DEFAULT_BOMB_LEVEL_THREE = 5;

        public static readonly int[] DEFAULT_BOMB_EACH_LEVEL = { DEFAULT_BOMB_LEVEL_ONE, DEFAULT_BOMB_LEVEL_TWO, DEFAULT_BOMB_LEVEL_THREE };
        #endregion Bomb Level Default Figures

        #endregion Snake State Manager

        #region UI
        public static readonly string LEVEL_UI_NAME = "Level text ui";
        public static readonly string SCORE_UI_NAME = "Score text ui";
        public static readonly string CAMERA_UI_TEXT = "Camera UI";
        public static readonly string DEFAULT_LEVEL_TEXT = "Level ";
        public static readonly string DEFAULT_SCORE_TEXT = "Score ";
        public static readonly string FRONT_CAMERA_UI_TEXT = "FRONT";
        public static readonly string BACK_CAMERA_UI_TEXT = "BACK";
        public static readonly string TOP_CAMERA_UI_TEXT = "TOP";
        public static readonly string BOTTOM_CAMERA_UI_TEXT = "BOTTOM";
        public static readonly string RIGHT_CAMERA_UI_TEXT = "RIGHT";
        public static readonly string LEFT_CAMERA_UI_TEXT = "LEFT";
        public static readonly string LEVEL_START_TIME_UI_NAME = "Level start timer ui";
        public static readonly float LEVEL_START_TIME_UI = 3000f;
        public static readonly string TIMER_UI_NAME = "Timer ui";
        #endregion UI

        #region Textures
        private static readonly string TEXTURES_BASE_PATH = "Assets/Textures/Snake Game/";

        #region Consumables Textures
        private static readonly string CONSUMABLES_BASE_TEXTURE_PATH = TEXTURES_BASE_PATH + "Consumables/";
        public static readonly string FOOD_TEXTURE_PATH = CONSUMABLES_BASE_TEXTURE_PATH + "apple_texture";
        #endregion Consumables Textures

        #region Snake Textures
        private static readonly string SNAKE_BODY_BASE_TEXTURE_PATH = TEXTURES_BASE_PATH + "Snake Body/";
        public static readonly string SNAKE_TONGUE_TEXTURE_PATH = SNAKE_BODY_BASE_TEXTURE_PATH + "snake_tongue";
        public static readonly string SNAKE_SKIN_TEXTURE_PATH = SNAKE_BODY_BASE_TEXTURE_PATH + "snake_skin";
        public static readonly string SNAKE_HEAD_TEXTURE_PATH = SNAKE_BODY_BASE_TEXTURE_PATH + "snake_head";
        #endregion Snake Textures

        #region Background Texture
        public static readonly string BACKGROUND_TEXTURE_PATH = "Assets/black";
        #endregion Background Texture

        #endregion Textures

        #region Scene Manager
        public static readonly string GAME_SCENE_NAME = "snake 3D game";
        #endregion Scene Manager

        #endregion Snake
    }
}