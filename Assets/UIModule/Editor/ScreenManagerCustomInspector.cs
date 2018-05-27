using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UIModule.Scripts;
using Sourav.Utilities.Scripts.Attributes;
using System;
using System.Reflection;

namespace UIModule.EditorUtils
{
    [CustomEditor(typeof(ScreenManager))]
    public class ScreenManagerCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Type type = target.GetType();
            MethodInfo[] methods = type.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                ButtonAttribute attribute = (ButtonAttribute)Attribute.GetCustomAttribute(methods[i], typeof(ButtonAttribute));
                if (attribute != null)
                {
                    DrawButtonAndInvokeMethod(attribute, methods[i]);
                }
            }

            EditorGUILayout.Space();

            DrawDefaultInspector();
            EditorGUILayout.Space();
            GUI.enabled = false;
            SerializedProperty property = serializedObject.FindProperty("currentScreen");
            EditorGUILayout.PropertyField(property);
            GUI.enabled = true;
        }

        void DrawButtonAndInvokeMethod(ButtonAttribute attribute, MethodInfo methodInfo)
        {
            if (GUILayout.Button(attribute.ButtonName.Equals("") ? methodInfo.Name : attribute.ButtonName))
            {
                foreach (var item in targets)
                {
                    methodInfo.Invoke(item, null);
                }
            }
        }
    }

    
}