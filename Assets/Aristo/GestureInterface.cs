using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Aristo {

// Enum for selecting computation backend.
public enum GestureBackend {
  Auto = 0,  // default backend, use GPU on PC and CPU on Android, Recommended
  CPU = 1,   // use CPU, not supported on PC
  GPU = 2,   // use GPU, supported on PC/Android
}

// Enum for detection mode. Larger mode return more info, but runs more slowly. If a mode is not
// supported on a device, will fallback to previous supported mode.
public enum GestureMode {
  Point2D = 0,   // Fastest mode, return one 2d point for hand
  Point3D = 1,   // Return one 3d point for hand, supported on Vive Pro and Focus
  Skeleton = 2,  // Return skeleton (21 points) for hand, supported on Vive and Vive Pro
}

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public class GestureOption {
  public GestureBackend backend = GestureBackend.Auto;
  public GestureMode mode = GestureMode.Skeleton;
}

// Enum for predefined gesture classification
public enum GestureType {
  Unknown = 0,  // All other gestures not in predefined set
  Point = 1,
  Fist = 2,
  OK = 3,
  Like = 4,
  Five = 5,
}

// Class containing detection result for one hand
[StructLayout(LayoutKind.Sequential)]
public class GestureResult {
  // Returns if this hand is left/right
  public bool isLeft {
    get;
    private set;
  }

  // Returns position of the hand. This field is guaranteed to be not null. The meaning of this
  // field is different based on actual GestureMode.
  // Point2D & Point3D: Only first point is used as the the position of hand.
  // Skeleton: The points is a 21-sized array with all the keypoints of the hand.
  public Vector3[] points {
    get;
    private set;
  }

  // Returns pre-defined gesture type.
  public GestureType gesture {
    get;
    private set;
  }

  // Returns position of the hand. The meaning of this field is different based on actual GestureMode.
  // Point2D & Point3D: It's value is same as points[0].
  // Skeleton: This is calculated position of palm center.
  public Vector3 position {
    get;
    private set;
  }

  // Returns rotation of the hand. The meaning of this field is different based on actual GestureMode.
  // Point2D & Point3D: This is direction from camera position to points[0], up is always +y.
  // Skeleton: This is calculated rotation of palm.
  public Quaternion rotation {
    get;
    private set;
  }

  internal GestureResult(GestureResultRaw raw) {
    isLeft = raw.isLeft;
    gesture = raw.gesture;
    points = raw.points;

    if (GestureProvider.HaveSkeleton) {
      Vector3 wrest = points[0];
      // finger roots
      Vector3 index = points[5], middle = points[9], ring = points[13], pinky = points[17];

      Vector3 vec1 = (index + middle) / 2 - wrest;
      Vector3 vec2 = (ring + pinky) / 2 - wrest;
      position = (vec1 + vec2) / 3 + wrest;

      Vector3 forward = isLeft ? Vector3.Cross(vec1, vec2) : Vector3.Cross(vec2, vec1);
      if (forward.sqrMagnitude < 1e-6)
        forward = Vector3.forward;
      Vector3 up = middle - wrest;
      if (up.sqrMagnitude < 1e-6)
        up = Vector3.up;
      rotation = Quaternion.LookRotation(forward, up);
    } else {
      position = points[0];
      var cameraTransform = GestureProvider.Current.transform;
      rotation = Quaternion.LookRotation(position - cameraTransform.position);
    }
  }
}

[StructLayout(LayoutKind.Sequential)]
internal class GestureResultRaw {
  [MarshalAs (UnmanagedType.I1)]
  internal bool isLeft;

  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
  internal Vector3[] points;

  internal GestureType gesture;
}

// Enum for possible errors in gesture detection
public enum GestureFailure {
  None = 0,        // No error occurs
  OpenCL = -1,     // (Only on Windows) OpenCL is not supported on the machine
  Camera = -2,     // Start camera failed
  Internal = -10,  // Internal errors
  CPUOnPC = -11,   // CPU backend is not supported on Windows
};

// Enum for possible status in gesture detection
public enum GestureStatus {
  NotStarted = 0, // Detection is not started or stopped
  Starting = 1,   // Detection is started, but first result is not returned yet
  Running = 2,    // Detection is running and updates result regularly
  Error = 3,      // Detection failed to start, or error occured during detection
}

static class GestureInterface {
  [DllImport("aristo_interface")]
  internal static extern GestureFailure StartGestureDetection([In, Out] GestureOption option);

  [DllImport("aristo_interface")]
  internal static extern void StopGestureDetection();

  [DllImport("aristo_interface")]
  internal static extern int GetGestureResult(out IntPtr points, out int frameIndex);

  [DllImport("aristo_interface")]
  internal static extern void SetCameraTransform(Vector3 position, Quaternion rotation);

  [DllImport("aristo_interface")]
  internal static extern void UseExternalTransform([MarshalAs (UnmanagedType.I1)] bool value);
}

// Helper class for extension methods
public static class GestureHelper {
  // Test if a point in skeleton is valid or not. Can be used as point.IsValidGesturePoint().
  public static bool IsValidGesturePoint(this Vector3 point) {
    return point.x != 0 || point.y != 0 || point.z != 0;
  }
}

}
