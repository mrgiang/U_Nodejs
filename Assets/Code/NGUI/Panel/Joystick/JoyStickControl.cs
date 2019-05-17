using Project.Networking;
using UnityEngine;

public class JoyStickControl : Mediator
{
    public static JoyStickControl instance;
    //    private const int DEFAULT_RANGE = 100;
    public Camera UiCamera;
    float actualRadiusBg;
    float actualRadiusStick;

    Rect rectCanTouch;
    float actualRangeStick;
    bool m_isDraging = false;

    public Vector2 direction = Vector2.zero;
    public float strength;
    private int fingerId = -100;

    private bool isFirstLanuch = true;

    private int screenWidth = -1;
    private int screenHeight = -1;
    private Transform stickButton;
    private Transform stickBg;
    private Transform originalPos;

    void OnEnable()
    {
        instance = this;
    }

    void Awake()
    {
        UiCamera = NetworkClient._instance.UICamera;
        Invoke("Init", 0.1f);
        stickButton = transform.Find("joystick_button");
        stickBg = transform.Find("joystick_bg");
        originalPos = transform.Find("joystick_bg_original_pos");
        stickBg.gameObject.SetActive(true);
    }

    void Init()
    {
        if (!(screenWidth != Screen.width || screenHeight != Screen.height || isFirstLanuch))
        {
            return;
        }
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        float aspect = (float)screenHeight / 720;
        BoxCollider box = GetComponent<BoxCollider>();
        actualRangeStick = box.size.x / 2 * aspect;
        rectCanTouch = new Rect(0, 0, box.size.x * aspect, box.size.y * aspect);
        isFirstLanuch = false;
        m_isDraging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDraging)
        {
            Init();

            //            Vector2 orgPos = new Vector2(originalPos.position.x, originalPos.position.y);
            Vector2 touchPositionScreen = GetTouchPosition();
            Ray castPoint = UiCamera.ScreenPointToRay(touchPositionScreen);
            RaycastHit hit;
            Vector2 touchPositionWorld = touchPositionScreen;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                touchPositionWorld = hit.point;
            }

            stickButton.position = touchPositionWorld;

            Vector3 offset = stickButton.localPosition - originalPos.localPosition;
            direction = offset.normalized;
            if (offset.magnitude > actualRangeStick)
            {
                offset = direction * actualRangeStick;
            }
            strength = offset.magnitude;
            stickButton.localPosition = originalPos.localPosition + offset;
            //            DEBUG.Log("stickPosition: " + stickButton.localPosition+"offset: " + offset+ ", strength: "+ strength);
        }

    }

    private void TouchBegin(Vector2 touchPosition)
    {
        m_isDraging = true;
    }

    private Vector2 GetTouchPosition(bool touchBegin = false)
    {
        Vector2 touchPosition = Vector2.zero;
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                {
                    if (touchBegin)
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            Touch touch = Input.GetTouch(i);

                            if (touch.phase == TouchPhase.Began)
                            {
                                Vector2 temp = new Vector2(touch.position.x, touch.position.y);
                                if (rectCanTouch.Contains(temp))
                                {
                                    touchPosition = temp;
                                    fingerId = touch.fingerId;
                                    break;
                                }

                            }
                        }

                    }
                    else
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            Touch touch = Input.GetTouch(i);

                            if (touch.fingerId == fingerId)
                            {
                                Vector2 temp = new Vector2(touch.position.x, touch.position.y);
                                touchPosition = temp;
                                break;
                            }
                        }
                    }

                    break;
                }
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                {
                    touchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    break;
                }
            default:
                break;
        }
        return touchPosition;
    }

    public void Reset()
    {
        m_isDraging = false;
        direction = Vector2.zero;

        fingerId = -100;
        strength = 0;
        stickButton.localPosition = originalPos.localPosition;
    }

    public void InitInstance()
    {
        instance = this;
    }

    void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            Vector2 touchPosition = GetTouchPosition(true);
            TouchBegin(touchPosition);
        }
        else
        {
            Reset();
        }
    }

    public override void onUpdate(string key, object para = null)
    {
        throw new System.NotImplementedException();
    }
}


