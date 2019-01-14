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
	private Text m_textFramePerSecound;
	[SerializeField]
	private bool m_isShow = true;
	[SerializeField]
	private int m_targetFrameRate = 60;
	[SerializeField]
	private int m_warningFrameRate = 40;
	[SerializeField]
	private int m_cautionFrameRate = 20;
	[SerializeField]
	private Color m_normalFrameRateColor = Color.white;
	[SerializeField]
	private Color m_warningFrameRateColor = Color.yellow;
	[SerializeField]
	private Color m_cautionFrameRateColor = Color.red;

	[SerializeField]
	private float m_debugTimeScale = 1.0f;
	[SerializeField]
	private bool m_isDebugTimeScale = false;

	private int m_frameCount;
	private float m_prevTime;
	private Image m_imageBackgroundFPS;

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
		isDontDestroy = true;
		base.Awake();
		Init();
	}

	private void Update()
	{
		CalcFramePerSecound();
	}

	void Init()
	{
		Application.targetFrameRate = m_targetFrameRate;
		m_frameCount = 0;
		m_prevTime = 0.0f;
	}

	//private void OnLogMessage(string i_logText, string i_stackTrace, LogType i_type)
	//{
	//	if (string.IsNullOrEmpty(i_logText))
	//	{
	//		return;
	//	}

	//	m_textUI.text += i_logText;
	//}



	/// <summary>
	/// FPSを計算する
	/// </summary>
	void CalcFramePerSecound()
	{
		++m_frameCount;
		float t = Time.realtimeSinceStartup - m_prevTime;

		if (t >= 0.5f)
		{
			if (m_frameCount < m_cautionFrameRate)
			{
				m_textFramePerSecound.color = m_cautionFrameRateColor;
			}
			else if (m_frameCount < m_warningFrameRate)
			{
				m_textFramePerSecound.color = m_warningFrameRateColor;
			}
			else
			{
				m_textFramePerSecound.color = m_normalFrameRateColor;
			}

			m_textFramePerSecound.text = "FPS " + m_frameCount.ToString("00");
			m_prevTime = Time.realtimeSinceStartup;
			m_frameCount = 0;
		}
	}
}