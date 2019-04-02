using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aristo;

class Draw : MonoBehaviour {
  public Color NormalColor = Color.green;
  public Color HighlightColor = Color.red;
  public ObjectFactory Factory = null;
  public GameObject PointPrefab = null;

  private Transform Camera = null;
  private LineRenderer Line = null;
  private List<Collider> Points;
  private int CurrentIndex = 0;

  void Awake() {
    Points = new List<Collider>();
    for (int i = 0; i < 4; i++)
      AddPoint(i);
    Line = GetComponent<LineRenderer>();
  }

  void Start() {
    Camera = GestureProvider.Current.transform;
    gameObject.SetActive(false);
    Line.enabled = false;
  }

  void Update() {
    if (CurrentIndex == 0)
      return;
    Vector3 indexTip = GestureProvider.RightHand.points[GestureProvider.Mode == GestureMode.Skeleton ? 8 : 0];
    if (indexTip.IsValidGesturePoint()) {
      Line.positionCount = CurrentIndex + 1;
      Line.SetPosition(CurrentIndex, indexTip);
    } else
      Line.positionCount = CurrentIndex;
  }

  void OnEnable() {
    transform.position = Camera.position;
    transform.rotation = Quaternion.Euler(0, Camera.rotation.eulerAngles.y, 0);
    SetIndex(0);
  }

  void OnDisable() {
    Line.enabled = false;
  }

  void OnTrigger(int index) {
    if (index != CurrentIndex % 4)
      return;
    if (CurrentIndex == 4) {
      Factory.AddObject();
      gameObject.SetActive(false);
      return;
    }
    SetIndex(CurrentIndex + 1);
  }

  public void OnStateChanged(int state) {
    gameObject.SetActive(state == 1);
  }

  void AddPoint(int index) {
    const float size = 0.075f;
    float x = index < 2 ? -size : size;
    float y = index % 3 == 0 ? size : -size;

    var go = GameObject.Instantiate(PointPrefab, transform, false);
    go.name = "Point " + index;
    go.transform.localPosition = new Vector3(x, y, 0.35f);
    go.transform.localRotation = Quaternion.identity;
    go.layer = gameObject.layer;

    var light = go.GetComponent<Light>();
    light.color = NormalColor;
    light.range = 0.03f;
    var collider = go.GetComponent<SphereCollider>();
    collider.radius = light.range;

    var trigger = go.GetComponent<TriggerCallback>();
    trigger.Value = index;
    trigger.Callback = OnTrigger;

    Points.Add(collider);
  }

  void SetIndex(int index) {
    CurrentIndex = index;
    Line.enabled = index > 0;
    Points[CurrentIndex % 4].GetComponent<Light>().color = HighlightColor;
    if (index > 0) {
      Line.positionCount = index;
      Line.SetPosition(index - 1, Points[index - 1].transform.position);
      Points[CurrentIndex - 1].GetComponent<Light>().color = NormalColor;
    }
  }
}
