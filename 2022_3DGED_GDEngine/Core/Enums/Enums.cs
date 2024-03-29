﻿namespace GD.Engine
{
    public enum GameObjectType : sbyte
    {
        #region 3D

        Interactable,
        Collectible,
        Consumable,
        Prop,
        Player,
        NPC,
        Enemy,
        Architecture,
        Camera,
        Trigger,  //collidable volume with no model
        SnakePart,
        #endregion 3D

        #region 2D

        UI_Menu_Text,
        UI_Texture,
        UI_Button,
        UI_Game_Text,
        UI_Timer_Text

        #endregion 2D
    }

    public enum TurnDirectionType : sbyte
    {
        Left = 1,
        Right = -1
    }

    /// <summary>
    /// Used to define static (lifetime of game) or dynamic (transient) game objects
    /// </summary>
    /// <see cref="GameObject"/>
    public enum ObjectType : sbyte
    {
        Static,
        Dynamic
        //TODO
    }

    /// <summary>
    /// Used to define if gameobject is opaque or transparent. doh!
    /// </summary>
    /// <see cref="GameObject"/>
    public enum RenderType : sbyte
    {
        Opaque = 1,
        Transparent = 0
    }

    /// <summary>
    /// Used by SoundManager to set volume etc on a category of sounds e.g. all explosion sounds
    /// </summary>
    /// <see cref="GD.Engine.Managers.SoundManager"/>
    public enum SoundCategoryType : sbyte
    {
        WinLose,
        Explosion,
        BackgroundMusic,
        UI,
        Alarm,
        Consumable,
        Player
    }

    #region Unused

    /// <summary>
    /// Event categories within the game that a subscriber can subscribe to in the EventDispatcher
    /// </summary>
    public enum EventCategoryType
    {
        /// <summary>
        /// Used when we want to toggle, cycle, change cameras
        /// </summary>
        Camera,

        /// <summary>
        /// Used when something happens to a PC e.g. win,lose,respawn
        /// </summary>
        Game,

        /// <summary>
        /// Used when something happens to a NPC e.g. win,lose,respawn
        /// </summary>
        NonPlayer,

        /// <summary>
        /// Used when player picks up a game object
        /// </summary>
        Pickup,

        /// <summary>
        /// Used when we want to play/pause/mute etc a sound
        /// </summary>
        Sound,

        /// <summary>
        /// Used when player makes a menu choice or to show/hide menu
        /// </summary>
        Menu,

        /// <summary>
        /// Used when we want to modify an on-screen ui element (e.g. UIProgressController with OnHealthDelta
        /// </summary>
        UI,

        /// <summary>
        /// Used to add/remove objects to the scene
        /// </summary>
        GameObject,

        /// <summary>
        /// Used to add/remove objects to the ui
        /// </summary>
        UiObject,

        /// <summary>
        /// Used when a transparent game object becomes opaque and vice verse
        /// </summary>
        Opacity,

        /// <summary>
        /// Used when we pick something with the physics system e.g. a ray pick
        /// </summary>
        Picking,

        Inventory,
        Video,

        //add more here...
        SnakeManager,
        FoodManager,
        StateManager,
        BombManager,
        SceneManager,
        RenderUIGameObjects
    }

    /// <summary>
    /// Event actions that can occur within a category (e.g. EventCategoryType.Sound with EventActionType.OnPlay)
    /// </summary>
    public enum EventActionType
    {
        OnPlay,
        OnPlay2D,
        OnPlay3D,

        OnPause,
        OnResume,
        OnRestart,
        OnExit,
        OnStop,
        OnStopAll,

        OnVolumeDelta,
        OnVolumeSet,
        OnMute,
        OnUnMute,

        OnClick,
        OnHover,

        OnCameraSetActive,
        OnCameraCycle,
        OnCameraUp,

        OnLose,
        OnWin,
        OnPickup,

        OnAddObject,
        OnRemoveObject,
        OnEnableObject,
        OnDisableObject,
        OnSpawnObject,
        OnObjectPicked,
        OnNoObjectPicked,
        OnHealthDelta,
        OnVolumeSetMaster,
        OnVolumeChange,
        OnRemoveInventory,
        OnAddInventory,
        OnMouseClick,

        //add more here...
        OnMove,
        Grow,
        RemoveFood,
        AddFood,
        ResetVelocity,
        UpdateActiveCameraPosition,
        AddBomb,
        RemoveBomb,
        MoveTongue,
        UpdateScore,
        RemoveUILevelStart,
        InitializeLevelUITimerStart,
        StartOfLevel,
        InitilizeFoodStartOfLevel,       
        InitilizeBombsStartOfLevel,
        OnLevelsScene,
        OnMainMenuScene,
        OnAudioScene,
        OnControlsScene,
        OnGameExit,
        ResetSnake,
        UITextIsDrawn,
        UITextIsNotDrawn,
        ResetIntroCamera,
        UpdateEndMenuScreenUIText,
        RemoveSnake,
        ResetCameraUI,
        RestartOfLevel
    }

    /// <summary>
    /// Possible status types for GameObject within the game (e.g. Update | Drawn, Update, Drawn, Off)
    /// </summary>
    public enum StatusType
    {
        Off = 0,            //turn object off
        Drawn = 1,           //e.g. a game or ui object a renderer but no components
        Updated = 2,         //e.g. a camera
        /*
        * Q. Why do we use powers of 2? Will it allow us to do anything different?
        * A. StatusType.Updated | StatusType.Drawn - See ObjectManager::Update() or Draw()
        */
    }

    #endregion Unused
}