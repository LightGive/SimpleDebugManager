using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDebugManager : LightGive.SingletonMonoBehaviour<SimpleDebugManager>
{
	private const float WidthPercent = 0.11f;
	private const float WidthToHeightPercent = 0.46f;
	private const float BlackAlpha = 0.5f;

	[SerializeField]
	private ShowDebugPosition m_debugPosition;
	[SerializeField]
	private bool m_isShow;
	[SerializeField]
	private int m_targetFrameRate;
	[SerializeField]
	private Color normalFrameRateColor = Color.white;
	[SerializeField]
	private Color warningFrameRateColor = Color.yellow;
	[SerializeField]
	private Color cautionFrameRateColor = Color.red;

	private int m_frameCount;
	private float m_prevTime;
	private Canvas m_debugCanvas;
	private Image m_imageBackgroundFPS;
	private Text m_textFPS;

	public enum ShowDebugPosition
	{
		TopRight,
		TopLeft,
		TopCenter,
		BottomRight,
		BottomLeft,
		BottomCenter,
	}

	protected override void Awake()
	{
		Init();
		base.Awake();
		CreateCanvas();
	}

	private void Update()
	{
		CalcFramePerSecound();
	}

	void Init()
	{
		isDontDestroy = true;
		Application.targetFrameRate = m_targetFrameRate;
		m_frameCount = 0;
		m_prevTime = 0.0f;
	}

	void CreateCanvas()
	{
		var canvasObject = new GameObject("DebugCanvas");
		canvasObject.transform.SetParent(this.transform);
		m_debugCanvas = canvasObject.AddComponent<Canvas>();
		m_debugCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		m_debugCanvas.sortingOrder = 9999;

		var backgroundFpsObject = new GameObject("BackgroundFPS");
		backgroundFpsObject.transform.SetParent(canvasObject.transform);
		m_imageBackgroundFPS = backgroundFpsObject.AddComponent<Image>();


		var wid = Screen.width * WidthPercent;
		var hei = wid * WidthToHeightPercent;
		m_imageBackgroundFPS.rectTransform.sizeDelta = new Vector2(wid, hei);
		m_imageBackgroundFPS.color = new Color(0.0f, 0.0f, 0.0f, BlackAlpha);

		SetPosition();

		var textObject = new GameObject("TextFPS");
		textObject.transform.SetParent(backgroundFpsObject.transform);
		m_textFPS = textObject.AddComponent<Text>();
		m_textFPS.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		m_textFPS.alignment = TextAnchor.MiddleCenter;
		m_textFPS.fontStyle = FontStyle.Bold;
		m_textFPS.resizeTextForBestFit = true;
		m_textFPS.resizeTextMaxSize = 300;
		m_textFPS.resizeTextMinSize = 5;
		m_textFPS.rectTransform.sizeDelta = m_imageBackgroundFPS.rectTransform.sizeDelta;
		m_textFPS.rectTransform.anchoredPosition = Vector2.zero;
		m_textFPS.text = "120";
	}

	void SetPosition()
	{
		var left = -(Screen.width / 2.0f) + (m_imageBackgroundFPS.rectTransform.sizeDelta.x / 2.0f);
		var right = (Screen.width / 2.0f) - (m_imageBackgroundFPS.rectTransform.sizeDelta.x / 2.0f);
		var bottom = -(Screen.height / 2.0f) + (m_imageBackgroundFPS.rectTransform.sizeDelta.y / 2.0f);
		var top = (Screen.height / 2.0f) - (m_imageBackgroundFPS.rectTransform.sizeDelta.y / 2.0f);

		switch (m_debugPosition)
		{
			case ShowDebugPosition.TopLeft: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(left, top); break;
			case ShowDebugPosition.TopRight: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(right, top); break;
			case ShowDebugPosition.TopCenter: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(0.0f, top); break;
			case ShowDebugPosition.BottomLeft: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(left, bottom); break;
			case ShowDebugPosition.BottomRight: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(right, bottom); break;
			case ShowDebugPosition.BottomCenter: m_imageBackgroundFPS.rectTransform.anchoredPosition = new Vector2(0.0f, bottom); break;
		}
	}


	/// <summary>
	/// FPSを計算する
	/// </summary>
	void CalcFramePerSecound()
	{
		++m_frameCount;
		float t = Time.realtimeSinceStartup - m_prevTime;

		if (t >= 0.5f)
		{
			if (m_frameCount < 20)
			{
				m_textFPS.color = cautionFrameRateColor;
			}
			else if (m_frameCount < 40f)
			{
				m_textFPS.color = warningFrameRateColor;
			}
			else
			{
				m_textFPS.color = normalFrameRateColor;
			}

			m_textFPS.text = m_frameCount.ToString("F1");
			m_prevTime = Time.realtimeSinceStartup;
			m_frameCount = 0;
		}
	}
}