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
        public static readonly Vector3 SNAKE_START_POSITION = new Vector3(0, 5, 5);
        public static readonly float SNAKE_DEFAULT_MOVE_SPEED = 600f;
        private static readonly float SNAKE_MULTIPLIER_SPEED = 0.02f;
        public static readonly float SNAKE_MULTIPLIER = SNAKE_DEFAULT_MOVE_SPEED * SNAKE_MULTIPLIER_SPEED;
        #endregion Snake Attributes

        #region Snake World

        #region Cube Dimensions
        private static readonly int SNAKE_GAME_SIZE = 21;

        public static readonly int SNAKE_GAME_MIN_SIZE = -SNAKE_GAME_SIZE;
        public static readonly int SNAKE_GAME_MAX_SIZE = SNAKE_GAME_SIZE;
        #endregion Cube Dimensions

        #region GameObjects Scale
        public static readonly int SCALE_AMOUNT = 3;
        public static readonly float COLLIDER_SCALE_AMOUNT = 2.5f;
        public static readonly Vector3 SNAKE_GAMEOBJECTS_SCALE =  new Vector3(SCALE_AMOUNT, SCALE_AMOUNT, SCALE_AMOUNT);
        public static readonly Vector3 SNAKE_GAMEOBJECTS_COLLIDER_SCALE = new Vector3(COLLIDER_SCALE_AMOUNT, COLLIDER_SCALE_AMOUNT, COLLIDER_SCALE_AMOUNT);
        #endregion GameObjects Scale

        #endregion Snake World

        #region Cameras
        private static readonly int CAMERA_POSITION = 65;
        private static readonly int CAMERA_ROTATION = 90;

        #region Front Camera
        public static readonly string FRONT_CAMERA_NAME = "front camera";
        public static readonly Vector3 DEFAULT_FRONT_CAMERA_TRANSLATION = new Vector3(0, 0, CAMERA_POSITION);
        public static readonly Vector3 DEFAULT_FRONT_CAMERA_ROTATION = Vector3.Zero;
        #endregion Front Camera

        #region Back Camera
        public static readonly string BACK_CAMERA_NAME = "back camera";
        public static readonly Vector3 DEFAULT_BACK_CAMERA_TRANSLATION = new Vector3(0, 0, -CAMERA_POSITION);
        public static readonly Vector3 DEFAULT_BACK_CAMERA_ROTATION = new Vector3(0, CAMERA_ROTATION * 2, 0);
        #endregion Back Camera

        #region Top Camera
        public static readonly string TOP_CAMERA_NAME = "top camera";
        public static readonly Vector3 DEFAULT_TOP_CAMERA_TRANSLATION = new Vector3(0, CAMERA_POSITION, 0);
        public static readonly Vector3 DEFAULT_TOP_CAMERA_ROTATION = new Vector3(-CAMERA_ROTATION, 0, 0);
        #endregion Top Camera

        #region Bottom Camera
        public static readonly string BOTTOM_CAMERA_NAME = "bottom camera";
        public static readonly Vector3 DEFAULT_BOTTOM_CAMERA_TRANSLATION = new Vector3(0, -CAMERA_POSITION, 0);
        public static readonly Vector3 DEFAULT_BOTTOM_CAMERA_ROTATION = new Vector3(CAMERA_ROTATION, 0, 0);
        #endregion Bottom Camera

        #region Right Camera
        public static readonly string RIGHT_CAMERA_NAME = "right camera";
        public static readonly Vector3 DEFAULT_RIGHT_CAMERA_TRANSLATION = new Vector3(CAMERA_POSITION, 0, 0);
        public static readonly Vector3 DEFAULT_RIGHT_CAMERA_ROTATION = new Vector3(0, CAMERA_ROTATION, 0);
        #endregion Right Camera

        #region Left Camera
        public static readonly string LEFT_CAMERA_NAME = "left camera";
        public static readonly Vector3 DEFAULT_LEFT_CAMERA_TRANSLATION = new Vector3(-CAMERA_POSITION, 0, 0);
        public static readonly Vector3 DEFAULT_LEFT_CAMERA_ROTATION = new Vector3(0, -CAMERA_ROTATION, 0);
        #endregion Left Camera

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
        public static readonly float MAX_SNAKE_LEVEL_TIME = 10000f;
        #endregion Game State Manager Data

        #region Menus

        #region Menu Background
        public static readonly string SNAKE_MENU_BACKGROUND_TEXTURE_PATH = "Assets/Textures/Menu/Backgrounds/snake_background";
        public static readonly string BACKGROUND_NAME = "background";
        #endregion Menu Background

        #region Controls
        public static readonly string SNAKE_MENU_BUTTON_TEXTURE_PATH = "Assets/Textures/Menu/Controls/snake_button_texture";
        #endregion Controls

        #region Main Menu

        #region Start Game Button
        public static readonly string START_GAME_BUTTON_NAME = "start game button";
        #endregion Start Game Button

        #endregion Main Menu


        #region End Menu
        public static readonly string END_MENU_NAME = "end menu";
        #endregion End Menu

        #endregion Menus

        #region Snake State Manager
        public static readonly int TOTAL_LEVELS = 2;
        #endregion Snake State Manager

        #endregion Snake
    }
}