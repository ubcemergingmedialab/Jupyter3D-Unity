using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using System.IO;

namespace Aristo {

static class AndroidBuildCheck {
  private const string android32Lib = "Assets/Aristo/Plugins/Android/libs/armeabi-v7a/libaristo_interface.so";
  private const string android64Lib = "Assets/Aristo/Plugins/Android/libs/arm64-v8a/libaristo_interface.so";
  private const string waveVRLib = "Assets/Aristo/Plugins/Android/libs/armeabi-v7a/libaristo_interface_wavevr.so";
  // need to use different filename for 32/64 backup, or 2018 series will compain
  private const string android32Backup = android32Lib + ".backup32";
  private const string android64Backup = android64Lib + ".backup64";
  private const string waveVRBackup = waveVRLib + ".backup";

  private class CustomPreprocessor : IPreprocessBuild {
    public int callbackOrder {
      get {
        return 0;
      }
    }

    public void OnPreprocessBuild(BuildTarget target, string path) {
      if (target != BuildTarget.Android)
        return;
      if (AndroidPlatformCheck.BuildAristoWithWaveVR) {
        Debug.Log("Build with WaveVR version of Hand Tracking SDK");
        AssetDatabase.MoveAsset(android32Lib, android32Backup);
        AssetDatabase.MoveAsset(android64Lib, android64Backup);
        AssetDatabase.MoveAsset(waveVRLib, android32Lib);
      } else {
        Debug.Log("Build with Android version of Hand Tracking SDK");
        AssetDatabase.MoveAsset(waveVRLib, waveVRBackup);
      }
    }
  }

  private class CustomPostprocessor : IPostprocessBuild {
    public int callbackOrder {
      get {
        return 0;
      }
    }

    public void OnPostprocessBuild(BuildTarget target, string path) {
      if (target != BuildTarget.Android)
        return;
      if (File.Exists(waveVRBackup))
        AssetDatabase.MoveAsset(waveVRBackup, waveVRLib);
      else if (File.Exists(android32Backup)) {
        AssetDatabase.MoveAsset(android32Lib, waveVRLib);
        AssetDatabase.MoveAsset(android32Backup, android32Lib);
        AssetDatabase.MoveAsset(android64Backup, android64Lib);
      }
    }
  }
}

}
