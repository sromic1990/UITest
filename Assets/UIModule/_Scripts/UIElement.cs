using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIModule.Enums;
using System;

namespace UIModule.Scripts
{
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public class UIElement : MonoBehaviour 
    {
        public string name;
        public List<ScreenElement> screenElements;

        void Awake()
        {
            name = gameObject.name;
        }
    }

    [Serializable]
    public class ScreenElement
    {
        public Screens Screen;
        public RectTransform Panel;
        public RectTransformData TransformData;
        public int ElementInPanel;

        public ScreenElement(Screens screen)
        {
            this.Screen = screen;
            Panel = null;
            TransformData = new RectTransformData();
            ElementInPanel = -1;
        }
    }

    [Serializable]
    public class RectTransformData
    {
        public Quaternion Rotation;
        public Vector3 Position;
        public Vector3 Scale;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 SizeDelta;
        public Vector2 Pivot;

        public RectTransformData()
        {
            Rotation = Quaternion.identity;
            Position = Vector3.zero;
            Scale = Vector3.zero;
            AnchorMin = Vector2.zero;
            AnchorMax = Vector2.zero;
            SizeDelta = Vector2.zero;
            Pivot = Vector2.zero;
        }
    }
}