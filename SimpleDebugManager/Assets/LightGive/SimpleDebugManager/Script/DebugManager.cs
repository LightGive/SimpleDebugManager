using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using TMPro.EditorUtilities;
#endif

/// <summary>
/// デバッグ関係をまとめたマネージャー
/// </summary>
public class DebugManager : SingletonMonoBehaviour<DebugManager>
{
    [SerializeField]
	private bool isShow = false;

    private int screenLongSide;
    private Rect boxRect;
    private GUIStyle style = new GUIStyle();

    // for fps calculation.
    private int frameCount;
    private float elapsedTime;
    private double frameRate;


	/// <summary>
	/// 初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
		Init();
	}

	void Init()
	{
        UpdateUISize();
    }


	/// <summary>
	/// 更新処理
	/// </summary>
	void Update()
	{
        // FPS calculation
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);

            if (frameRate < 20)
                style.normal.textColor = Color.red;
            else if (frameRate < 40f)
                style.normal.textColor = Color.yellow;
            else
                style.normal.textColor = Color.white;

            frameCount = 0;
            elapsedTime = 0;

            // Update the UI size if the resolution has changed
            if (screenLongSide != Mathf.Max(Screen.width, Screen.height))
            {
                UpdateUISize();
            }
        }
    }

    /// <summary>
    /// Resize the UI according to the screen resolution
    /// </summary>
    private void UpdateUISize()
    {
        screenLongSide = Mathf.Max(Screen.width, Screen.height);
        var rectLongSide = screenLongSide / 8;
        boxRect = new Rect(1, 1, rectLongSide, rectLongSide / 3);
        style.fontSize = (int)(screenLongSide / 36.8);
        //style.normal.textColor = Color.white;
    }

    /// <summary>
    /// Display FPS
    /// </summary>
    private void OnGUI()
    {
        if (!isShow)
            return;

        GUI.Box(boxRect, "");
        GUI.Label(boxRect, "FPS : " + frameRate.ToString("F1") ,style);
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
