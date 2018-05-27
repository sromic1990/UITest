using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIModule.Enums;
using System;
using Sourav.Utilities.Scripts.Attributes;
using UIElement = UIModule.Scripts.UIElement;

namespace UIModule.Scripts
{
    public class ScreenManager : MonoBehaviour 
    {
        public static ScreenManager Instance;

        //List<PanelElement> panelElements;
        public List<ScreenElements> screenElements;

        [SerializeField]
        [HideInInspector]
        public Screens currentScreen;

        void Awake()
        {
            Instance = this;
            //panelElements = new List<PanelElement>();
        }

        void Start()
        {
            List<UIElement> elements = UIContainer.Instance.UIObjects;
            SetUIElements(elements);
            SortUIElements();
        }

        void SetUIElements(List<UIElement> elements)
        {
            screenElements = new List<ScreenElements>();


            for (int i = 0; i < elements.Count; i++)
            {
                for (int j = 0; j < elements[i].screenElements.Count; j++)
                {
                    //Search if there are any element in the screen elements matching the current Screen
                    int indexK = -1;
                    int indexM = -1;
                    for (int k = 0; k < screenElements.Count; k++)
                    {
                        if (screenElements[k].Screen.Equals(elements[i].screenElements[j].Screen))
                        {
                            indexK = k;
                            //found object and found screen
                            for (int m = 0; m < screenElements[k].Panels.Count; m++)
                            {
                                if (screenElements[k].Panels[m].Equals(elements[i].screenElements[j].Panel))
                                {
                                    indexM = m;
                                    //found the correct object and correct panel
                                }
                            }
                        }
                    }

                    if (indexK != -1)
                    {
                        if (indexM != -1)
                        {
                            UITemps temps = new UITemps();
                            temps.UIElement = elements[i].GetComponent<RectTransform>();
                            temps.UIElementData = elements[i].screenElements[j].TransformData;
                            temps.ElementId = elements[i].screenElements[j].ElementInPanel;
                            screenElements[indexK].Panels[indexM].UIElements.Add(temps);
                        }
                        else
                        {
                            PanelObj Panel = new PanelObj();
                            Panel.Panel = elements[i].screenElements[j].Panel;
                            UITemps temps = new UITemps();
                            temps.UIElement = elements[i].GetComponent<RectTransform>();
                            temps.UIElementData = elements[i].screenElements[j].TransformData;
                            temps.ElementId = elements[i].screenElements[j].ElementInPanel;
                            Panel.UIElements.Add(temps);
                            screenElements[indexK].Panels.Add(Panel);
                        }
                    }
                    else
                    {
                        ScreenElements element = new ScreenElements(elements[i].screenElements[j].Screen);
                        PanelObj Panel = new PanelObj();
                        Panel.Panel = elements[i].screenElements[j].Panel;
                        UITemps temps = new UITemps();
                        temps.UIElement = elements[i].GetComponent<RectTransform>();
                        temps.UIElementData = elements[i].screenElements[j].TransformData;
                        temps.ElementId = elements[i].screenElements[j].ElementInPanel;
                        Panel.UIElements.Add(temps);
                        element.Panels.Add(Panel);
                        screenElements.Add(element);
                    }
                }
            }

        }

        void SortUIElements()
        {
            for (int i = 0; i < screenElements.Count; i++)
            {
                for (int j = 0; j < screenElements[i].Panels.Count; j++)
                {
                    screenElements[i].Panels[j].Sort();
                }
            }
        }

        public void PrepareScreen(Screens screen)
        {
            UIContainer.Instance.HideAll();
            UIContainer.Instance.UnParentAll();

            Debug.Log("PrepareScreen");

            for (int i = 0; i < screenElements.Count; i++)
            {
                if(screen.Equals(screenElements[i].Screen))
                {
                    Debug.Log("ScreenFound");
                    Debug.Log("Number of PanelObj = "+screenElements[i].Panels.Count);
                    for (int j = 0; j < screenElements[i].Panels.Count; j++)
                    {
                        screenElements[i].Panels[j].Panel.gameObject.SetActive(false);
                        for (int k = 0; k < screenElements[i].Panels[j].UIElements.Count; k++)
                        {
                            screenElements[i].Panels[j].UIElements[k].UIElement.parent = screenElements[i].Panels[j].Panel;
                            SetRectTransformData(screenElements[i].Panels[j].UIElements[k]);
                            screenElements[i].Panels[j].UIElements[k].UIElement.gameObject.SetActive(true);
                        }
                    }
                }
            }

            ShowScreen(screen);
        }

        private void ShowScreen(Screens screen)
        {
            for (int i = 0; i < screenElements.Count; i++)
            {
                if (screen.Equals(screenElements[i].Screen))
                {
                    for (int j = 0; j < screenElements[i].Panels.Count; j++)
                    {
                        screenElements[i].Panels[j].Panel.gameObject.SetActive(true);
                    }
                }
            }

            currentScreen = screen;
        }

        private void SetRectTransformData(UITemps uiTemp)
        {
            uiTemp.UIElement.pivot = uiTemp.UIElementData.Pivot;
            uiTemp.UIElement.anchorMin = uiTemp.UIElementData.AnchorMin;
            uiTemp.UIElement.anchorMax = uiTemp.UIElementData.AnchorMax;
            uiTemp.UIElement.rotation = uiTemp.UIElementData.Rotation;
            uiTemp.UIElement.localPosition = uiTemp.UIElementData.Position;
            uiTemp.UIElement.localScale = uiTemp.UIElementData.Scale;
            uiTemp.UIElement.sizeDelta = uiTemp.UIElementData.SizeDelta;
        }

        [Button("Show Main Menu")]
        public void Test()
        {
            Debug.Log("Test");
            PrepareScreen(Screens.MainMenu);
        }

        [Button("Show Settings")]
        public void Test2()
        {
            PrepareScreen(Screens.Settings);
        }


    }

    [Serializable]
    public class ScreenElements
    {
        public Screens Screen;
        public List<PanelObj> Panels;

        public ScreenElements(Screens screen)
        {
            this.Screen = screen;
            Panels = new List<PanelObj>();
        }
    }
    [Serializable]
    public class PanelObj
    {
        public RectTransform Panel;
        public List<UITemps> UIElements;

        public PanelObj()
        {
            Panel = null;
            UIElements = new List<UITemps>(); 
        }

        public void Sort()
        {
            List<int> indexList = new List<int>();
            List<UITemps> temp = new List<UITemps>();

            for (int i = 0; i < UIElements.Count; i++)
            {
                indexList.Add(UIElements[i].ElementId); 
            }

            indexList.Sort();

            for (int i = indexList.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < UIElements.Count; j++)
                {
                    if(UIElements[j].ElementId == indexList[i] /*&& !(temp.Contains(UIElements[j]))*/)
                    {
                        temp.Add(UIElements[j]);
                        break;
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                string str = string.Format("temp = {0} name = {1} ", i.ToString(), temp[i].UIElement.name);
                Debug.Log(str);
                Debug.Log("i = "+i);
            }

            UIElements = temp;
        }
    }
    [Serializable]
    public class UITemps
    {
        public RectTransform UIElement;
        public RectTransformData UIElementData;
        public int ElementId;

        public UITemps()
        {
            UIElement = null;
            UIElementData = new RectTransformData();
            ElementId = -1;
        }
    }
}