using UnityEditor;
using UnityEngine;
using System;
using UnityEditor.IMGUI.Controls;

namespace BuildingVolumes.Streaming
{

    [CustomEditor(typeof(GeometrySequenceStream))]
    [CanEditMultipleObjects]
    public class GeometryStreamGUI : Editor
    {
        GeometrySequenceStream stream;

        SerializedProperty parentTransform;

        SerializedProperty pointcloudMaterial;
        SerializedProperty meshMaterial;
        SerializedProperty pointSize;

        SerializedProperty bufferSize;
        SerializedProperty useAllThreads;
        SerializedProperty threadCount;

        SerializedProperty droppedFrame;
        SerializedProperty currentFrame;
        SerializedProperty targetFrameTiming;
        SerializedProperty currentFrameTiming;
        SerializedProperty smoothedFPS;

        bool showInfo;
        bool showBufferOptions;

        private void OnEnable()
        {
            parentTransform = serializedObject.FindProperty("parentTransform");
            
            pointcloudMaterial = serializedObject.FindProperty("pointcloudMaterial");
            meshMaterial = serializedObject.FindProperty("meshMaterial");
            pointSize = serializedObject.FindProperty("pointSize");

            bufferSize = serializedObject.FindProperty("bufferSize");
            useAllThreads = serializedObject.FindProperty("useAllThreads");
            threadCount = serializedObject.FindProperty("threadCount");

            droppedFrame = serializedObject.FindProperty("frameDropped");
            currentFrame = serializedObject.FindProperty("currentFrameIndex");
            targetFrameTiming = serializedObject.FindProperty("targetFrameTimeMs");
            currentFrameTiming = serializedObject.FindProperty("elapsedMsSinceLastFrame");
            smoothedFPS = serializedObject.FindProperty("smoothedFPS");

            stream = (GeometrySequenceStream)target;

            serializedObject.ApplyModifiedProperties();

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(parentTransform);

            EditorGUILayout.PropertyField(pointcloudMaterial);
            EditorGUILayout.PropertyField(meshMaterial);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Point size");
            float newPointSize = EditorGUILayout.Slider(pointSize.floatValue, 0.001f, 0.03f);
            if(!Mathf.Approximately(newPointSize, pointSize.floatValue))
                stream.SetPointSize(newPointSize);
            EditorGUILayout.EndHorizontal();

            showBufferOptions = EditorGUILayout.Foldout(showBufferOptions, "Buffer Options");
            if (showBufferOptions)
            {
                EditorGUILayout.PropertyField(bufferSize);
                EditorGUILayout.PropertyField(useAllThreads);
                EditorGUILayout.PropertyField(threadCount);
            }

            showInfo = EditorGUILayout.Foldout(showInfo, "Frame Info");
            if (showInfo)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(droppedFrame, new GUIContent("Dropped Frame"));
                EditorGUILayout.PropertyField(currentFrame, new GUIContent("Currently played frame"));
                EditorGUILayout.PropertyField(targetFrameTiming, new GUIContent("Target frame time in ms"));
                EditorGUILayout.PropertyField(currentFrameTiming, new GUIContent("Current frame time in ms"));
                EditorGUILayout.PropertyField(smoothedFPS, new GUIContent("Smoothed FPS"));
                EditorGUI.EndDisabledGroup();
            }           

            serializedObject.ApplyModifiedProperties();
        }
    }
}
