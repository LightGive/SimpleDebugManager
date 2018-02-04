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
    private bool slowFrameRateBreak = false;
    [SerializeField]
    private int targetFrameRate = 60;
    [SerializeField]
    private int fontSizeFrameRate = 50;
    [SerializeField]
    private int normalFrameRate = 30;
    [SerializeField]
    private int slowFrameRate = 10;
    [SerializeField]
    private Color normalFrameRateColor = Color.white;
    [SerializeField]
    private Color cautionFrameRateColor = Color.yellow;
    [SerializeField]
    private Color warningFrameRateColor = Color.red;

    [Header("DebugText")]
    [SerializeField]
    private bool isShowBackground = false;
    [SerializeField]
    private int normalFontSize = 50;
    [SerializeField]
    private Color normalTextColor = Color.gray;
    [SerializeField]
    private string[] showTexts;

    [Header("Setting")]
    [SerializeField]
    private KeyCode showDebugKeyCode = KeyCode.D;


    private int frameCount;
    private float elapsedTime;
    private double frameRate;
    private double minFrameRate;
    private double maxFrameRate;
    private GUIStyle fpsGuiStyle = new GUIStyle();
    private GUIStyle normalTextGuiStyle = new GUIStyle();

    private float FrameRateBoxWidth     { get { return fontSizeFrameRate * 2.5f; } }
    private float FrameRateBoxHeight    { get { return fontSizeFrameRate * 1.2f; } }
    private float NormalTextBoxHeight   { get { return normalFontSize * 1.1f;} }


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
        maxFrameRate = 0;
        minFrameRate = targetFrameRate;
        normalTextGuiStyle.alignment = TextAnchor.MiddleLeft;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // FPSを計算する
        CalcFramePerSecound();

        //Debugキーを表示するかをチェックする
        CheckDebugKey();

    }

    /// <summary>
    /// FPSを計算する
    /// </summary>
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
                if (slowFrameRateBreak)
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

            if (minFrameRate > frameRate && Time.realtimeSinceStartup > 2f)
                minFrameRate = frameRate;
            else if (maxFrameRate < frameRate)
                maxFrameRate = frameRate;
                

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
    /// Display
    /// </summary>
    private void OnGUI()
    {
        if (!isShow)
            return;

        //FPS表示
        ShowFramePerSecound();
        ShowNormalText();
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
    /// 通常のデバッグのテキストを表示する
    /// </summary>
    void ShowNormalText()
    {
        var maxLengh = 0;
        for (int i = 0; i < showTexts.Length; i++)
        {
            if (showTexts[i].Length > maxLengh)
            {
                maxLengh = i;
            }
        }

        normalTextGuiStyle.fontSize = normalFontSize;
        normalTextGuiStyle.normal.textColor = normalTextColor;

        var backgroundRect = new Rect(1, 1, GetTextBoxWidth(showTexts[maxLengh]), normalFontSize * showTexts.Length*1.1f);
        if (isShowBackground)
            GUI.Box(backgroundRect, "");

        for (int i = 0; i < showTexts.Length; i++)
        {
            var rect = new Rect(1, NormalTextBoxHeight * i, GetTextBoxWidth(showTexts[i]), normalFontSize);
            GUI.Label(rect, showTexts[i], normalTextGuiStyle);
        }
    }

    /// <summary>
    /// 文字の幅を取得する
    /// </summary>
    /// <returns>The text box width.</returns>
    /// <param name="_text">Text.</param>
    float GetTextBoxWidth(string _text)
    {
        return normalFontSize*_text.Length * 1.0f;
    }

    /// <summary>
    /// デバッグのテキストを追加して表示
    /// </summary>
    /// <param name="_text">Text.</param>
    /// <param name="_line">Line.</param>
    public void SetDebugText(string _text, int _line = 0)
    {
        _line = Mathf.Clamp(_line, 0, showTexts.Length);
        showTexts[_line] = _text;
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
