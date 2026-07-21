#if !NO_UNITY
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YG;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace Game.Scripts.Infrastructure
{
  public struct PlatformStoreID
  {
    public int Code;

    public static implicit operator int(PlatformStoreID id)
    {
      return id.Code;
    }

    public static implicit operator PlatformStoreID(int code)
    {
      return new PlatformStoreID { Code = code };
    }
  }

  public struct PlatformInfo
  {
    public RuntimePlatform UnityRuntimePlatform
    {
      get
      {
        if(!_runtimePlatformCached)
        {
          _runtimePlatformCache = Application.platform;
          _runtimePlatformCached = true;
        }

        return _runtimePlatformCache;
      }

#if UNITY_EDITOR || GAME_IS_DEV
      set
      {
        _runtimePlatformCache = value;
        _runtimePlatformCached = true;
      }
#endif
    }

    public PlatformStoreID Store;

    RuntimePlatform _runtimePlatformCache;
    bool _runtimePlatformCached;

    public bool IsMobile()
    {
      return IsiOS() || IsAndroid();
    }

    public bool IsiOS()
    {
      return UnityRuntimePlatform == RuntimePlatform.IPhonePlayer;
    }

    public bool IsAndroid()
    {
      return UnityRuntimePlatform == RuntimePlatform.Android;
    }

    public bool IsWebGL()
    {
      return UnityRuntimePlatform == RuntimePlatform.WebGLPlayer;
    }

    public override string ToString()
    {
      return $"'{UnityRuntimePlatform}' (store id: {Store.Code})";
    }
  }

  public static class Platform
  {
    static PlatformInfo _platform = new PlatformInfo();

    public static string BundleIdentifier = "";
    public static readonly List<string> AndroidUploadKeySignatures = new List<string>();
    static UnityEngine.Object _sGameInstance;

    public static void Init(UnityEngine.Object gameInstance, PlatformStoreID storeFlavor, string bundleID)
    {
      _sGameInstance = gameInstance;
      _platform.Store = storeFlavor;
      if(!string.IsNullOrEmpty(bundleID))
        BundleIdentifier = bundleID;
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#endif
    [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Reset()
    {
      _platform = new PlatformInfo();
      _sGameInstance = null;
    }

    public static bool InGame()
    {
      return !IsEditor() || Application.isPlaying && _sGameInstance != null;
    }

    public static PlatformInfo Get()
    {
      return _platform;
    }

    public static bool IsLikelyRetina()
    {
      const float tabletMinInchSize = 7;
      const float tabletRetinaMinDPI = 260;
      const float phoneRetinaMinDPI = 400;

      if(Screen.dpi == 0)
        return false;

      float inchW = Screen.width / Screen.dpi;
      float inchH = Screen.height / Screen.dpi;

      float retinaDPI =
        (inchW*inchW + inchH*inchH) >= (tabletMinInchSize*tabletMinInchSize) ?
          tabletRetinaMinDPI : phoneRetinaMinDPI;

      return Screen.dpi > retinaDPI;
    }

    public static bool IsIpad()
    {
#if UNITY_IPHONE
      return UnityEngine.iOS.Device.generation.ToString().IndexOf("iPad") >= 0;
#else
      return false;
#endif
    }

    public static bool IsLowRAMDevice()
    {
      return SystemInfo.systemMemorySize <= 3072 || SystemInfo.graphicsMemorySize <= 512;
    }

    public static bool IsAndroid()
    {
      return _platform.IsAndroid();
    }

    public static bool IsiOS()
    {
      return _platform.IsiOS();
    }

    public static bool IsWebGL()
    {
      return _platform.IsWebGL();
    }

    public static bool IsMobileWebGL()
    {
      if (IsWebGL())
        return YG2.envir.isMobile;

      return false;
    }
    
    public static bool IsStandaloneWebGL()
    {
      if (IsWebGL())
        return YG2.envir.isDesktop;

      return false;
    }

    public static bool IsWidescreen()
    {
      float aspect = (float)Screen.width / (float)Screen.height;
      return aspect > 1.5f;
    }

    public struct ResourceData
    {
      public string FilePath;
      public TextAsset Asset;

      public bool IsValid()
      {
        return Asset != null || !string.IsNullOrEmpty(FilePath);
      }

      public byte[] GetBytes()
      {
        if(Asset != null)
          return Asset.bytes;
        else
          return File.ReadAllBytes(FilePath);
      }
    }

    public static ResourceData ReadResourceData(string relPath, bool strict = true)
    {
   //NOTE: alternative for reliable reading of data during fast builds
   #if UNITY_EDITOR
      if(!relPath.EndsWith(".bytes"))
        relPath += ".bytes";
      string filePath = Application.dataPath + "/Resources/" + relPath;
      if(!strict && !File.Exists(filePath))
        return new ResourceData();
      return new ResourceData() {FilePath = filePath};
   #else
      TextAsset asset = Resources.Load(relPath) as TextAsset;
      if(!strict && asset == null)
        return new ResourceData();
      //Error.Verify(asset != null, "No such resource file " + relPath);
      return new ResourceData() {Asset = asset};
   #endif
    }

    public static string GetPersistentPath(string relPath)
    {
      var fullPath = Path.Combine(Application.persistentDataPath, relPath);
      return fullPath;
    }

    public static string GetPersistentPath(string relPath, string subPath)
    {
      var fullPath = Path.Combine(Application.persistentDataPath, relPath, subPath);
      return fullPath;
    }

    public static bool IsBluestacks()
    {
      return SystemInfo.graphicsDeviceName.Equals("Bluestacks");
    }

    public static bool IsEditor()
    {
#if UNITY_EDITOR
      return true;
#else
      return false;
#endif
    }

    public static bool IsIOSSimulator()
    {
#if IOS_BUILD_FOR_SIMULATOR
      return true;
#else
      return false;
#endif
    }

    public static bool IsStandalone()
    {
#if UNITY_STANDALONE
      return true;
#else
      return false;
#endif
    }

    public static int GetAndroidApiLevel()
    {
      return AndroidAPILevel.Get();
    }

    public static bool TasksPermissionsIsEnforced()
    {
      return IsAndroid() && GetAndroidApiLevel() < 21;
    }

    static class AndroidAPILevel
    {
      static int _cached = 0;

      public static int Get()
      {
        return _cached > 0 ? _cached : Retrieve();
      }

      static int Retrieve()
      {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
          _cached = version.GetStatic<int>("SDK_INT");
        }
#endif

        return _cached;
      }
    }

    public static int GetIOSVersion()
    {
#if UNITY_IPHONE
      string strver = UnityEngine.iOS.Device.systemVersion;
      return VersionUtils.GetCode(strver);
#else
      return 0;
#endif
    }

    public static void AndroidMoveAppToBackground()
    {
#if UNITY_ANDROID
      using(AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
      using(AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
      {
        activity.Call<bool>("moveTaskToBack", true);
      }
#endif
    }

    static class AndroidDisplayCutout
    {
      enum State { Unknown, NoCutout, WithCutout }
      static State _state = State.Unknown;
      const int API_LEVEL_ANDROID_PIE = 28;

      static void ResolveState()
      {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(GetAndroidApiLevel() < API_LEVEL_ANDROID_PIE)
          return;

        using(AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using(AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
        using(AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
        using(AndroidJavaObject decor = window.Call<AndroidJavaObject>("getDecorView"))
        using(AndroidJavaObject insets = decor.Call<AndroidJavaObject>("getRootWindowInsets"))
        using(AndroidJavaObject cutout = insets.Call<AndroidJavaObject>("getDisplayCutout"))
        {
          AnalyzeCutout(cutout);
        }
#endif
      }

#if UNITY_ANDROID && !UNITY_EDITOR
      static void AnalyzeCutout(AndroidJavaObject cutout)
      {
        if(cutout == null)
        {
          _state = State.NoCutout;
          return;
        }

        int inset_left   = cutout.Call<int>("getSafeInsetLeft");
        int inset_right  = cutout.Call<int>("getSafeInsetRight");
        int inset_top    = cutout.Call<int>("getSafeInsetTop");
        int inset_bottom = cutout.Call<int>("getSafeInsetBottom");

        if(inset_left != 0 || inset_right != 0 || inset_top != 0 || inset_bottom != 0)
          _state = State.WithCutout;
        else
          _state = State.NoCutout;
      }
#endif

      public static bool Exists()
      {
        if(_state == State.Unknown)
          ResolveState();
        return _state == State.WithCutout;
      }
    }

    public static bool IsAndroidDeviceWithDisplayCutout()
    {
      return AndroidDisplayCutout.Exists();
    }

    public static int GetUTCDelta()
    {
      double totalUtcSec = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
      double totalLocalSec = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
      return Convert.ToInt32(Math.Floor(((totalLocalSec - totalUtcSec) / 3600)) % 24);
    }

#if UNITY_EDITOR
    public static NetworkReachability? SimulatedNetworkReachability;
#endif

    public static NetworkReachability GetInternetReachability()
    {
#if UNITY_EDITOR
      if(SimulatedNetworkReachability.HasValue)
        return SimulatedNetworkReachability.Value;
#endif

      return Application.internetReachability;
    }

    public static bool InternetIsTurnedOff()
    {
      return GetInternetReachability() == NetworkReachability.NotReachable;
    }

    public static bool InternetIsWiFi()
    {
      return GetInternetReachability() == NetworkReachability.ReachableViaLocalAreaNetwork;
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    static extern int extractFileFromXCAssets(string asset_name, string dst_path);

    public static bool IOSExtractXCAssetResource(string asset_name, string dst_path)
    {
      int native_ret_val = extractFileFromXCAssets(asset_name, dst_path);
      return native_ret_val > 0;
    }

    public class IOSReleaseModeDetector
    {
      public const string MODE_DEV        = "Dev";
      public const string MODE_AD_HOC     = "AdHoc";
      public const string MODE_ENTERPRISE = "Enterprise";
      public const string MODE_APPSTORE   = "AppStore";

      [DllImport("__Internal")]
      private static extern string getApplicationReleaseMode();

      static string cached_release_mode;

      public static string GetReleaseMode()
      {
        if(string.IsNullOrEmpty(cached_release_mode))
          cached_release_mode = getApplicationReleaseMode();
        return cached_release_mode;
      }

      public static bool IsAdHoc()
      {
        string mode = GetReleaseMode();
        return mode == MODE_DEV || mode == MODE_AD_HOC;
      }
    }
#endif

    public static string GetStreamingAssetsPath()
    {
#if UNITY_EDITOR
      return GetStreamingAssetsPathForEditor();
#else
      return GetStreamingAssetsPathForBuilds();
#endif
    }

    public static string GetStreamingAssetsPathForEditor()
    {
#if UNITY_EDITOR
      var sep = Path.DirectorySeparatorChar;
      return $"{GameRoot()}build{sep}StreamingAssets";
#else
      //Error.Verify(false, "Only available in Editor!");
      return "";
#endif
    }

    public static string GetStreamingAssetsPathForBuilds()
    {
      return Application.streamingAssetsPath;
    }

#if UNITY_EDITOR
    public static string GameRoot()
    {
      var sep = Path.DirectorySeparatorChar;
      return Path.GetFullPath(Application.dataPath + $"{sep}..{sep}..{sep}");
    }

    public static string AbsPath(string relPath)
    {
      var gameRoot = GameRoot();
      relPath = relPath.Replace("/", Path.DirectorySeparatorChar.ToString());
      return relPath.StartsWith(gameRoot) ? relPath : gameRoot + relPath;
    }
#endif
  }
}
#endif
