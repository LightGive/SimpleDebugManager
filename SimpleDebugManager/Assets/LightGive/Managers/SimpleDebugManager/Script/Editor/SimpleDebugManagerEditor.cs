using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleDebugManager))]
public class SimpleDebugManagerEditor : Editor
{
	private SerializedProperty m_isShowProp;
	private SerializedProperty m_debugTimeScaleProp;
	private SerializedProperty m_targetFrameRateProp;
	private SerializedProperty m_warningFrameRateProp;
	private SerializedProperty m_cautionFrameRateProp;
	private SerializedProperty m_normalFrameRateColorProp;
	private SerializedProperty m_warningFrameRateColorProp;
	private SerializedProperty m_cautionFrameRateColorProp;
	private SerializedProperty m_isDebugTimeScaleProp;

	private void OnEnable()
	{
		m_isShowProp = serializedObject.FindProperty("m_isShow");
		m_debugTimeScaleProp = serializedObject.FindProperty("m_debugTimeScale");
		m_targetFrameRateProp = serializedObject.FindProperty("m_targetFrameRate");
		m_warningFrameRateProp = serializedObject.FindProperty("m_warningFrameRate");
		m_cautionFrameRateProp = serializedObject.FindProperty("m_cautionFrameRate");
		m_normalFrameRateColorProp = serializedObject.FindProperty("m_normalFrameRateColor");
		m_warningFrameRateColorProp = serializedObject.FindProperty("m_warningFrameRateColor");
		m_cautionFrameRateColorProp = serializedObject.FindProperty("m_cautionFrameRateColor");
		m_isDebugTimeScaleProp = serializedObject.FindProperty("m_isDebugTimeScale");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("FPS Text Setting", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField(m_isShowProp);
		EditorGUILayout.PropertyField(m_targetFrameRateProp);
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(m_normalFrameRateColorProp);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(m_warningFrameRateColorProp);
		EditorGUILayout.PropertyField(m_warningFrameRateProp, new GUIContent(""), GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(m_cautionFrameRateColorProp);
		EditorGUILayout.PropertyField(m_cautionFrameRateProp, new GUIContent(""), GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Time Setting", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(m_debugTimeScaleProp);



		m_isDebugTimeScaleProp.boolValue = GUILayout.Toggle(m_isDebugTimeScaleProp.boolValue, m_isDebugTimeScaleProp.boolValue ? "on" : "off", "button");



		serializedObject.ApplyModifiedProperties();
	}
}
