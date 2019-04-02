//credit: https://github.com/Clodo76/vr-desktop-mirror

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

public class Desktop : MonoBehaviour
{
    [HideInInspector]
    public int Screen = 0;
    [HideInInspector]
    public int ScreenIndex = 0;

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy,
                      int dwData, int dwExtraInfo);

    [Flags]
    public enum MouseEventFlags
    {
        LEFTDOWN = 0x00000002,
        LEFTUP = 0x00000004,
        MIDDLEDOWN = 0x00000020,
        MIDDLEUP = 0x00000040,
        MOVE = 0x00000001,
        ABSOLUTE = 0x00008000,
        RIGHTDOWN = 0x00000008,
        RIGHTUP = 0x00000010
    }

    private DesktopManager m_manager;
    private LineRenderer m_line;
    private Renderer m_renderer;
    private MeshCollider m_collider;

    private bool m_zoom = false;
    private bool m_zoomWithFollowCursor = false;

    private Vector3 m_positionNormal;
    private Quaternion m_rotationNormal;
    private Vector3 m_positionZoomed;
    private Quaternion m_rotationZoomed;

    private float m_positionAnimationStart = 0;

    // Keyboard and Mouse
    private float m_lastShowClickStart = 0;

    void Start()
    {
        m_line = GetComponent<LineRenderer>();
        m_renderer = GetComponent<MeshRenderer>();
        m_collider = GetComponent<MeshCollider>();
    }

    public void SetManager(DesktopManager m)
    {
        m_manager = m;
        m_manager.Connect(this);
    }

    public void Update()
    {
        bool skip = false;
        if (Visible() == false)
            skip = true;
        if ((m_zoom == false))
            skip = true;
        if (skip == false)
        {
            float step = 0;
            if (Time.time - m_positionAnimationStart > 1)
                step = 1;
            else
                step = Time.time - m_positionAnimationStart;

        }

    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        m_manager.Disconnect(this);
    }

    public void HideLine()
    {
        m_line.enabled = false;
    }

    public void Hide()
    {
        m_renderer.enabled = false;
        m_collider.enabled = false;
    }

    public void Show()
    {
        Debug.Log(m_renderer.name);
        m_renderer.enabled = true;
        m_collider.enabled = true;

    }

    public bool Visible()
    {
        return (m_renderer.enabled);
    }

    public void CheckKeyboardAndMouse()
    {

        if (m_manager.KeyboardZoom != KeyCode.None)
        {
            if (Input.GetKeyDown(m_manager.KeyboardZoom))
            {
                if (m_zoom == false)
                {
                    m_zoomWithFollowCursor = true;
                    ZoomIn();
                }
                else
                    ZoomOut();
            }

            if ((m_zoom) && (m_zoomWithFollowCursor))
            {
                DesktopManager.ActionInThisFrame = true;

                m_manager.KeyboardZoomDistance += Input.GetAxisRaw("Mouse ScrollWheel");
                m_manager.KeyboardZoomDistance = Mathf.Clamp(m_manager.KeyboardZoomDistance, 0.2f, 100);

                // Cursor position in world space
                Vector3 cursorPos = m_manager.GetCursorPos();
                cursorPos.x = cursorPos.x / m_manager.GetScreenWidth(Screen);
                cursorPos.y = cursorPos.y / m_manager.GetScreenHeight(Screen);
                cursorPos.y = 1 - cursorPos.y;
                cursorPos.x = cursorPos.x - 0.5f;
                cursorPos.y = cursorPos.y - 0.5f;
                cursorPos = transform.TransformPoint(cursorPos);

                Vector3 deltaCursor = transform.position - cursorPos;

                m_positionZoomed = Camera.main.transform.position + Camera.main.transform.rotation * new Vector3(0, 0, m_manager.KeyboardZoomDistance);
                m_rotationZoomed = Camera.main.transform.rotation;

                m_positionZoomed += deltaCursor;
            }

        }
    }

    public void ZoomIn()
    {
        m_positionAnimationStart = Time.time;
        m_zoom = true;
    }

    public void ZoomOut()
    {
        m_positionAnimationStart = Time.time;

        m_zoom = false;
    }

    public void ReInit(Texture2D tex, int width, int height)
    {
        GetComponent<Renderer>().material.mainTexture = tex;
        GetComponent<Renderer>().material.mainTexture.filterMode = m_manager.TextureFilterMode;
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, -1));

        float sx = width;
        float sy = height;
        sx = sx * m_manager.ScreenScaleFactor;
        sy = sy * m_manager.ScreenScaleFactor;
        transform.localScale = new Vector3(sx, sy, 1);

    }

}
