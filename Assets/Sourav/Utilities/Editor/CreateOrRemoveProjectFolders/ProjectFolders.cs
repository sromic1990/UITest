using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Sourav.Utilities.EditorUtils
{
    public class ProjectFolders : Editor 
    {
        static string path = Application.dataPath + "/";

        static List<string> directoryList = new List<string>();
        static List<string> deleteList = new List<string>();

        [MenuItem("ProjectUtility/Utilities/Create Folders %#&c")]
        public static void CreateFolders()
        {
            if (EditorUtility.DisplayDialog("Create Project Folders?", "Are you sure you want to create the Project folders?", "Yes", "No"))
            {
                Create("Meshes");
                Create("Fonts");
                Create("Plugins");
                Create("Textures");
                Create("Materials");
                Create("Physics");
                Create("Resources");
                Create("Scenes");
                Create("Music");
                Create("_Scripts");
                Create("Shaders");
                Create("Sounds");
                Create("Prefabs");
                Create("Editor");
                Create("Animation");
                Create("Sprite");

                AssetDatabase.Refresh();
            }
        }

        [MenuItem("ProjectUtility/Utilities/Delete Empty Folders %#&d")]
        public static void DeleteEmptyDirectories()
        {
            if (EditorUtility.DisplayDialog("Delete Empty Folders?", "Are you sure you want to delete the empty folders from your project?", "Yes", "No"))
            {
                GetSubDirectories(path);

                for (int i = 0; i < directoryList.Count; i++)
                {
                    if(CheckIfDirectoryIsEmpty(directoryList[i]))
                    {
                        deleteList.Add(directoryList[i]);
                    }
                }

                DeleteDirectories(deleteList);

                directoryList.Clear();
                deleteList.Clear();

                AssetDatabase.Refresh();
            }
        }

        private static void Create(string folderName)
        {
            string directoryPath = path + folderName;

            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private static bool CheckIfDirectoryIsEmpty(string directoryPath)
        {
            bool isEmpty = false;

            if (Directory.GetFiles(directoryPath).Length == 0 && Directory.GetDirectories(directoryPath).Length == 0)
            {
                isEmpty = true;
            }

            return isEmpty;
        }

        private static void DeleteDirectories(List<string> directories)
        {
            for (int i = 0; i < directories.Count; i++)
            {
                Directory.Delete(directories[i]);
            }
        }

        private static void GetSubDirectories(string dir)
        {
            if(!directoryList.Contains(dir))
            {
                directoryList.Add(dir);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(dir);

            foreach (string subdirectory in subdirectoryEntries)
            {
                GetSubDirectories(subdirectory);
            }
        }
    }
    
}
