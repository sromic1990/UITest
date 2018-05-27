using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UIModule.Enums;
using UIModule.Scripts;
using Sourav.Utilities.Scripts;

namespace UIModule.EditorUtils
{
    public class UIModule : EditorWindow 
    {
        public static UIModule Instance;

        public Screens currentScreen;

        [MenuItem("ProjectUtility/UIModule/Load Module")]
        static void Init()
        {
            if (Instance == null)
            {
                Instance = GetWindow<UIModule>();
                Instance.Show();
            }
            else
            {
                Instance.Focus();
            }


            //Check For Singleton UIContainer
            CheckGameObjectsPresetInScene<UIContainer> checkGameObject = new CheckGameObjectsPresetInScene<UIContainer>();
            GameObjectSearchResult gSearchResult = checkGameObject.CheckForGameObject();
            GameObject gObj = null;

            if (gSearchResult.numberOfObjects >= 1)
            {
                gObj = gSearchResult.foundGameObjects[0];
                if (gObj != null)
                {
                    gObj.SetActive(true);
                }
                if (gSearchResult.numberOfObjects > 1)
                {
                    for (int i = 1; i < gSearchResult.foundGameObjects.Count; i++)
                    {
                        DestroyImmediate(gSearchResult.foundGameObjects[i]);
                    }
                }
                Show_ObjectAlreadyExist("UI Container Module");
            }
            else
            {
                CreatePrefabInstance createPrefab = new CreatePrefabInstance("Prefabs/UIContainer");
            }

            //Check For Singleton ScreenManager
            CheckGameObjectsPresetInScene<ScreenManager> checkGameObject2 = new CheckGameObjectsPresetInScene<ScreenManager>();
            GameObjectSearchResult gSearchResult2 = checkGameObject2.CheckForGameObject();
            GameObject gObj2 = null;

            if (gSearchResult2.numberOfObjects >= 1)
            {
                gObj2 = gSearchResult2.foundGameObjects[0];
                if (gObj2 != null)
                {
                    gObj2.SetActive(true);
                }
                if (gSearchResult2.numberOfObjects > 1)
                {
                    for (int i = 1; i < gSearchResult2.foundGameObjects.Count; i++)
                    {
                        DestroyImmediate(gSearchResult2.foundGameObjects[i]);
                    }
                }
                Show_ObjectAlreadyExist("ScreenManager Module");
            }
            else
            {
                CreatePrefabInstance createPrefab = new CreatePrefabInstance("Prefabs/ScreenManager");
            }


        }

        private void OnGUI()
        {
            currentScreen = (Screens)EditorGUILayout.EnumPopup("Choose Screen: ", currentScreen);
            if(GUILayout.Button("SetUIElement"))
            {
                SetUIElement();
            }
        }

        private void SetUIElement()
        {
            if(Selection.gameObjects != null)
            {
                for (int j = 0; j < Selection.gameObjects.Length; j++)
                {
                    if(Selection.gameObjects[j].GetComponent<RectTransform>() != null)
                    {
                        if(Selection.gameObjects[j].GetComponent<global::UIModule.Scripts.UIElement>() != null)
                        {
                            global::UIModule.Scripts.UIElement uiElement = Selection.gameObjects[j].GetComponent<global::UIModule.Scripts.UIElement>();
                            int index = -1;
                            if(uiElement.screenElements != null)
                            {
                                for (int i = 0; i < uiElement.screenElements.Count; i++)
                                {
                                    if(uiElement.screenElements[i].Screen.Equals(currentScreen))
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                
                                if(index != -1)
                                {
                                    uiElement.screenElements[index].TransformData = GetCurrentRectTransform(uiElement.GetComponent<RectTransform>());
                                }
                                else
                                {
                                    AddScreenElementData(uiElement);
                                }
                            }
                            else
                            {
                                uiElement.screenElements = new List<ScreenElement>();
                                AddScreenElementData(uiElement);
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Missing component.", "Selected element does not have a UIElement component. Please select the correct element and try again.", "Ok"); 
                        }
                        
                        //Do the operations
                        //EditorUtility.DisplayDialog("UI Object selected.", "The object you selected contains Rect Transform component.", "Ok");
                        
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("No UI Object selected.", "The object you selected does not contain Rect Transform component. Please select again.", "Ok");
                    }
                }

            }
            else
            {
                EditorUtility.DisplayDialog("No object selected", "No UI object selected, please try again.", "Ok");
            }
        }

        private RectTransformData GetCurrentRectTransform(RectTransform rectTransform)
        {
            RectTransformData rData = new RectTransformData();

            rData.Rotation = rectTransform.rotation;
            rData.Position = rectTransform.localPosition;
            rData.Scale = rectTransform.localScale;
            rData.AnchorMin = rectTransform.anchorMin;
            rData.AnchorMax = rectTransform.anchorMax;
            rData.SizeDelta = rectTransform.sizeDelta;
            rData.Pivot = rectTransform.pivot;

            return rData;
        }

        private void AddScreenElementData(global::UIModule.Scripts.UIElement uiElement)
        {
            ScreenElement screenElement = new ScreenElement(currentScreen);
            screenElement.TransformData = GetCurrentRectTransform(uiElement.GetComponent<RectTransform>());
            uiElement.screenElements.Add(screenElement);
        }

        public static void Show_ObjectAlreadyExist(string module)
        {
            EditorUtility.DisplayDialog(module +" Already Present In Scene", module+ " is already present in scene. Cannot create more than one active module.", "Ok");
        }

    }

}
