using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aristo;

class Laser : MonoBehaviour {
  private static Color color = new Color(0.3f, 0, 0, 1);
  private const float angularVelocity = 50.0f;

  public GameObject laser = null;
  public GameObject light = null;

  private bool visible = false;
  private Renderer hit = null;

  void Start() {
    light.SetActive(false);
#if UNITY_ANDROID
    laser.transform.localRotation = Quaternion.identity;
#endif
  }

  void Update() {
    var hand = GestureProvider.RightHand;
    if (hand == null) {
      laser.SetActive(false);
      return;
    }

    transform.position = hand.position;

    // smooth rotation for skeleton mode
    if (laser.activeSelf && GestureProvider.HaveSkeleton)
      transform.rotation = Quaternion.RotateTowards(transform.rotation, hand.rotation, angularVelocity * Time.deltaTime);
    else
      transform.rotation = hand.rotation;

    laser.SetActive(visible);
  }

  public void OnStateChanged(int state) {
    light.SetActive(state == 1);
    if (state == 2)
      visible = true;
    else {
      visible = false;
      if (hit != null)
        StopHit();
    }
  }

  void OnTriggerEnter(Collider other) {
    if (!other.gameObject.name.StartsWith("Cube"))
      return;
    if (hit != null)
      StopHit();
    hit = other.GetComponent<Renderer>();
    if (hit != null) {
      hit.material.EnableKeyword("_EMISSION");
      hit.material.SetColor ("_EmissionColor", color);
    }
  }

  void OnTriggerExit(Collider other) {
    if (hit != null && hit == other.GetComponent<Renderer>())
      StopHit();
  }

  void StopHit() {
    hit.material.DisableKeyword("_EMISSION");
    hit = null;
  }
}
