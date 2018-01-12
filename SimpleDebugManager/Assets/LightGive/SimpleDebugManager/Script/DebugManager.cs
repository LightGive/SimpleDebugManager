using UnityEngine;
using System.Collections.Generic;
using LightGive;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// デバッグ関係をまとめたマネージャー
/// </summary>
public class DebugManager : LightGive.SingletonMonoBehaviour<DebugManager>
{
    public enum ShowDebugPosition
    {
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft,
    }

    [SerializeField]
    private bool isShow = false;

    [Header("FPS")]
    [SerializeField]
    private ShowDebugPosition showDebugPositon;
    [SerializeField]
    private bool isWarningFrameRateBreak = false;
    [SerializeField]
    private int targetFrameRate = 60;
    [SerializeField]
    private int fontSizeFrameRate = 50;
    [SerializeField]
    private int cautionFrameRate = 30;
    [SerializeField]
    private int warningFrameRate = 10;
    [SerializeField]
    private Color normalFrameRateColor = Color.white;
    [SerializeField]
    private Color cautionFrameRateColor = Color.yellow;
    [SerializeField]
    private Color warningFrameRateColor = Color.red;

    [Header("Other")]
    [SerializeField]
    private int maxTextLine = 10;

    [Header("Setting")]
    [SerializeField]
    private KeyCode showDebugKeyCode = KeyCode.D;



    [SerializeField]
    private string[] showTexts;

    private int frameCount;
    private float elapsedTime;
    private double frameRate;

    private GUIStyle fpsGuiStyle = new GUIStyle();
    private GUIStyle normalTextGuiStyle = new GUIStyle();


    private float FrameRateBoxWidth     { get { return fontSizeFrameRate * 2.5f; } }
    private float FrameRateBoxHeight    { get { return fontSizeFrameRate * 1.2f; } }
    private float NormalTextFontSize    { get { return Screen.height / maxTextLine; } }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = targetFrameRate;

        Init();
    }

    void Init()
    {
        fpsGuiStyle.alignment = TextAnchor.MiddleCenter;
        fpsGuiStyle.fontStyle = FontStyle.Bold;
        fpsGuiStyle.normal.textColor = Color.white;
        frameRate = targetFrameRate;


    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // FPS calculation
        CalcFramePerSecound();

        CheckDebugKey();


    }

    void CalcFramePerSecound()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);

            if (frameRate < 20)
            {
                fpsGuiStyle.normal.textColor = Color.red;
                if (isWarningFrameRateBreak)
                    Debug.Break();
            }
            else if (frameRate < 40f)
            {
                fpsGuiStyle.normal.textColor = Color.yellow;
            }
            else
            {
                fpsGuiStyle.normal.textColor = Color.white;
            }
            frameCount = 0;
            elapsedTime = 0;

        }
    }

    /// <summary>
    /// デバッグのキーをチェックする
    /// </summary>
    void CheckDebugKey()
    {
        //表示・非表示を切り替える
        if (Input.GetKeyDown(showDebugKeyCode))
        {
            isShow = !isShow;
        }
    }

    /// <summary>
    /// FPSを表示
    /// </summary>
    void ShowFramePerSecound()
    {
        fpsGuiStyle.fontSize = fontSizeFrameRate;

        Rect debugFpsRect = Rect.zero;
        switch (showDebugPositon)
        {
            case ShowDebugPosition.TopRight:
                debugFpsRect = new Rect(Screen.width - FrameRateBoxWidth, 1, FrameRateBoxWidth, FrameRateBoxHeight);
                break;
            case ShowDebugPosition.TopLeft:
                debugFpsRect = new Rect(1, 1, FrameRateBoxWidth, FrameRateBoxHeight);
                break;
            case ShowDebugPosition.BottomRight:
                debugFpsRect = new Rect(Screen.width - FrameRateBoxWidth, Screen.height - FrameRateBoxHeight, FrameRateBoxWidth, FrameRateBoxHeight);
                break;
            case ShowDebugPosition.BottomLeft:
                debugFpsRect = new Rect(1, Screen.height - FrameRateBoxHeight, FrameRateBoxWidth, FrameRateBoxHeight);
                break;
            default: break;
        }

        GUI.Box(debugFpsRect, "");
        GUI.Label(debugFpsRect, frameRate.ToString("F1"), fpsGuiStyle);
    }

    /// <summary>
    /// Display FPS
    /// </summary>
    private void OnGUI()
    {
        if (!isShow)
            return;

        ShowFramePerSecound();

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DebugManager))]
public class DebugManagerEditor : Editor
{
	private float timeScale = 0.0f;

	public override void OnInspectorGUI()
	{
        base.DrawDefaultInspector();
	}
}
#endif
