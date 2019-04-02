using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace Aristo {

[InitializeOnLoad]
class AndroidPlatformCheck : EditorWindow {
  private const string GoogleVRDefine = "ARISTO_WITH_GOOGLEVR";
  private const string WaveVRDefine = "ARISTO_WITH_WAVEVR";
  private static string IgnoreFilePath;

  static AndroidPlatformCheck() {
    EditorApplication.update += Check;
  }

  static void Check() {
    IgnoreFilePath = Application.dataPath + "/../AristoSkipPlatformCheck.txt";
    EditorApplication.update -= Check;
    if (File.Exists(IgnoreFilePath))
      return;

    // check if GoogleVR and WaveVR plugin exist
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var types = assemblies.SelectMany(a => a.GetTypes()).ToList();
    bool hasGooglevrPlugin = types.Any(t => t.FullName == "GvrSettings");
    bool hasWavevrPlugin = types.Any(t => t.FullName == "WaveVR_Render");
    if (hasGooglevrPlugin && hasWavevrPlugin) {
      bool showDialog = EditorPrefs.GetBool("Aristo.AndroidPlatformCheck.ShowDialog", false);
      EditorPrefs.SetBool("Aristo.AndroidPlatformCheck.ShowDialog", true);
      if (showDialog) {
        bool result = EditorUtility.DisplayDialog(
                        "Your Project continas both GoogleVR and WaveVR plugin",
                        "Both plugins cannot work together and Aristo gesture plugin cannot determine which API to use," +
                        "Please add " + GoogleVRDefine + " or " + WaveVRDefine + " to android scripting define symbols manually.",
                        "Got it", "Skip Checks"
                      );
        if (!result)
          File.WriteAllText(IgnoreFilePath, "");
      } else
        Debug.LogWarningFormat("Aristo detected both GoogleVR and WaveVR plugin, please add {0} or {1} to android scripting define symbols manually.",
                               GoogleVRDefine, WaveVRDefine);
      return;
    }

    // update symbols
    string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
    string newSymbols = "";
    foreach (var define in symbols.Split(';')) {
      if (define == GoogleVRDefine) {
        if (!hasGooglevrPlugin)
          continue;
        hasGooglevrPlugin = false;
      } else if (define == WaveVRDefine) {
        if (!hasWavevrPlugin)
          continue;
        hasWavevrPlugin = false;
      }
      AppendDefine(ref newSymbols, define);
    }
    if (hasGooglevrPlugin) {
      AppendDefine(ref newSymbols, GoogleVRDefine);
      Debug.LogFormat("Add scripting define symbol {0} for Android platform", GoogleVRDefine);
    }
    if (hasWavevrPlugin) {
      AppendDefine(ref newSymbols, WaveVRDefine);
      Debug.LogFormat("Add scripting define symbol {0} for Android platform", WaveVRDefine);
    }
    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, newSymbols);
  }

  static void AppendDefine(ref string defines, string element) {
    if (defines != "")
      defines += ";";
    defines += element;
  }

  internal static bool BuildAristoWithWaveVR {
    get {
      return PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android)
             .Split(';').Any( d => d == WaveVRDefine);
    }
  }
}

}
