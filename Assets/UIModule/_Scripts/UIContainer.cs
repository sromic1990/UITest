using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIModule.Enums;

namespace UIModule.Scripts
{
    public class UIContainer : MonoBehaviour 
    {
        public static UIContainer Instance;

        public List<UIElement> UIObjects;
        public RectTransform Canvas;
        public List<RectTransform> Panels;

        void Awake()
        {
            Instance = this;
            //DEPENDENCY must have ONLY 1 canvas in scene.
            Canvas = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        }

        public void HideAll()
        {
            for (int i = 0; i < UIObjects.Count; i++)
            {
                UIObjects[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < Panels.Count; i++)
            {
                Panels[i].gameObject.SetActive(false);
            }
        }

        public void UnParentAll()
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                Panels[i].parent = Canvas;
            }
            
            for(int i = 0; i < UIObjects.Count; i++)
            {
                UIObjects[i].GetComponent<RectTransform>().parent = Canvas;
            }
        }
    }
}