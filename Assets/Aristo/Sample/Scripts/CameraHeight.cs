using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aristo;

class CameraHeight : MonoBehaviour {
  void Awake() {
#if UNITY_ANDROID
    // increase camera height by 1.5m on android, since they assume camera height starts at 0m
    transform.position = new Vector3(0, 1.5f, 0);
#endif
    GameObject.Destroy(this);
  }
}
