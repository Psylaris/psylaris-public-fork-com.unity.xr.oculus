﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.XR.Oculus;

namespace Unity.XR.Oculus.Editor
{
    [CustomEditor(typeof(OculusSettings))]
    public class OculusSettingsEditor : UnityEditor.Editor
    {
        private const string kSharedDepthBuffer = "SharedDepthBuffer";
        private const string kDashSupport = "DashSupport";
        private const string kStereoRenderingModeDesktop = "m_StereoRenderingModeDesktop";
        private const string kStereoRenderingModeAndroid = "m_StereoRenderingModeAndroid";
        private const string kLowOverheadMode = "LowOverheadMode";
        private const string kOptimizeBufferDiscards = "OptimizeBufferDiscards";
        private const string kPhaseSync = "PhaseSync";

        static GUIContent s_SharedDepthBufferLabel = EditorGUIUtility.TrTextContent("Shared Depth Buffer");
        static GUIContent s_DashSupportLabel = EditorGUIUtility.TrTextContent("Dash Support");
        static GUIContent s_StereoRenderingModeLabel = EditorGUIUtility.TrTextContent("Stereo Rendering Mode");
        static GUIContent s_LowOverheadModeLabel = EditorGUIUtility.TrTextContent("Low Overhead Mode (GLES)");
        static GUIContent s_OptimizeBufferDiscardsLabel = EditorGUIUtility.TrTextContent("Optimize Buffer Discards (Vulkan)");
        static GUIContent s_PhaseSyncLabel = EditorGUIUtility.TrTextContent("Phase Sync");

        private SerializedProperty m_SharedDepthBuffer;
        private SerializedProperty m_DashSupport;
        private SerializedProperty m_StereoRenderingModeDesktop;
        private SerializedProperty m_StereoRenderingModeAndroid;
        private SerializedProperty m_LowOverheadMode;
        private SerializedProperty m_OptimizeBufferDiscards;
        private SerializedProperty m_PhaseSync;

        public override void OnInspectorGUI()
        {
            if (serializedObject == null || serializedObject.targetObject == null)
                return;

            if (m_SharedDepthBuffer == null) m_SharedDepthBuffer = serializedObject.FindProperty(kSharedDepthBuffer);
            if (m_DashSupport == null) m_DashSupport = serializedObject.FindProperty(kDashSupport);
            if (m_StereoRenderingModeDesktop == null) m_StereoRenderingModeDesktop = serializedObject.FindProperty(kStereoRenderingModeDesktop);
            if (m_StereoRenderingModeAndroid == null) m_StereoRenderingModeAndroid = serializedObject.FindProperty(kStereoRenderingModeAndroid);
            if (m_LowOverheadMode == null) m_LowOverheadMode = serializedObject.FindProperty(kLowOverheadMode);
            if (m_OptimizeBufferDiscards == null) m_OptimizeBufferDiscards = serializedObject.FindProperty(kOptimizeBufferDiscards);
            if (m_PhaseSync == null) m_PhaseSync = serializedObject.FindProperty(kPhaseSync);

            serializedObject.Update();

            EditorGUIUtility.labelWidth = 220.0f;

            BuildTargetGroup selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.HelpBox("Oculus settings cannnot be changed when the editor is in play mode.", MessageType.Info);
                EditorGUILayout.Space();
            }
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            if (selectedBuildTargetGroup == BuildTargetGroup.Standalone)
            {
                EditorGUILayout.PropertyField(m_StereoRenderingModeDesktop, s_StereoRenderingModeLabel);
                EditorGUILayout.PropertyField(m_SharedDepthBuffer, s_SharedDepthBufferLabel);
                EditorGUILayout.PropertyField(m_DashSupport, s_DashSupportLabel);
            }
            else if(selectedBuildTargetGroup == BuildTargetGroup.Android)
            {
                EditorGUILayout.PropertyField(m_StereoRenderingModeAndroid, s_StereoRenderingModeLabel);
                EditorGUILayout.PropertyField(m_LowOverheadMode, s_LowOverheadModeLabel);
                EditorGUILayout.PropertyField(m_OptimizeBufferDiscards, s_OptimizeBufferDiscardsLabel);
                EditorGUILayout.PropertyField(m_PhaseSync, s_PhaseSyncLabel);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndBuildTargetSelectionGrouping();

            serializedObject.ApplyModifiedProperties();

            EditorGUIUtility.labelWidth = 0.0f;
        }
    }
}
