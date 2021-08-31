using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public class EssentialsSpawnerWindow : EditorWindow
{
    bool hasFindButtonBeenPressed;
    bool[] selectedEssentialsToSpawn;
    bool[] selectedEssentialsInstantiatedToDelete;
    string[] essentialPrefabs;
    bool isAnEssentialOnScene;
    string prefabPath = "Assets/Prefabs/Essentials";
    Vector2 scrollPos;
    [MenuItem("Improvus/Essentials Spawner Window")]
    public static void OpenWindow()
    {
        var window = GetWindow(typeof(EssentialsSpawnerWindow)) as EssentialsSpawnerWindow;
        window.Show();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
        EditorGUILayout.HelpBox("This Window permits you to instantiate on scene the Essential Prefabs in order to the don't have any critical problem designing the level.", MessageType.Info);
        EditorGUILayout.Space();

        GUILayout.Label("Essential Prefabs", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        if(GUILayout.Button("Find Essential Prefabs"))
        {
            essentialPrefabs = getPrefabs();
            selectedEssentialsToSpawn = new bool[essentialPrefabs.Length];
            selectedEssentialsInstantiatedToDelete = new bool [essentialPrefabs.Length];
            hasFindButtonBeenPressed = true;
            isAnEssentialOnScene = CheckIfIsAnEssentialOnScene();
        }

        EditorGUILayout.Space();

        if(hasFindButtonBeenPressed)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            for(int i = 0; i < essentialPrefabs.Length; i++)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                selectedEssentialsToSpawn[i] = EditorGUILayout.Toggle(selectedEssentialsToSpawn[i]);
                if(selectedEssentialsToSpawn[i]) EditorGUILayout.LabelField(essentialPrefabs[i], selectedItemStyle);
                else EditorGUILayout.LabelField(essentialPrefabs[i]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        else
        {
            EditorGUILayout.HelpBox("You must press the 'Find Essential Prefabs' button in order to show to available Essential Prefabs", MessageType.Info);
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Select All Essentials"))
        {
            for(int i = 0; i < selectedEssentialsToSpawn.Length; i++)
            {
                selectedEssentialsToSpawn[i] = true;
            }
        }
        if(GUILayout.Button("Deselect All Essentials"))
        {
            for(int i = 0; i < selectedEssentialsToSpawn.Length; i++)
            {
                selectedEssentialsToSpawn[i] = false;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox("Press the following button to instantiate the selected Essential Prefabs on Scene.\nHave in mind that each Essential Prefab can only be spawned once.", MessageType.Info);
        if(GUILayout.Button("Spawn Essentials"))
        {
            SpawnEssentials();
            if(!isAnEssentialOnScene) isAnEssentialOnScene = true;
        }

        EditorGUILayout.Space();
        GUILayout.Label("Essential Prefabs on Scene", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if(!isAnEssentialOnScene)
        {
            EditorGUILayout.HelpBox("You need at least a Essential Prefab Spawned on Scene in order to show this section.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            for(int i = 0; i < essentialPrefabs.Length; i++)
            {
                if(CheckIfEssentialIsSpawnedOnScene(essentialPrefabs[i]))
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    selectedEssentialsInstantiatedToDelete[i] = EditorGUILayout.Toggle(selectedEssentialsInstantiatedToDelete[i]);
                    if(selectedEssentialsInstantiatedToDelete[i]) EditorGUILayout.LabelField(essentialPrefabs[i], selectedItemStyle);
                    else EditorGUILayout.LabelField(essentialPrefabs[i]);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Select All Essentials"))
            {
                for(int i = 0; i < selectedEssentialsInstantiatedToDelete.Length; i++)
                {
                    if(CheckIfEssentialIsSpawnedOnScene(essentialPrefabs[i])) selectedEssentialsInstantiatedToDelete[i] = true;
                }
            }
            if(GUILayout.Button("Deselect All Essentials"))
            {
                for(int i = 0; i < selectedEssentialsInstantiatedToDelete.Length; i++)
                {
                    if(CheckIfEssentialIsSpawnedOnScene(essentialPrefabs[i])) selectedEssentialsInstantiatedToDelete[i] = false;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if(GUILayout.Button("Delete Selected Essential Prefabs on Scene"))
            {
                for(int i = 0; i < essentialPrefabs.Length; i++)
                {
                    if(CheckIfEssentialIsSpawnedOnScene(essentialPrefabs[i]) && selectedEssentialsInstantiatedToDelete[i])
                    {
                        DestroyEssential(essentialPrefabs[i]);
                        selectedEssentialsInstantiatedToDelete[i] = false;
                    }
                }
                isAnEssentialOnScene = CheckIfIsAnEssentialOnScene();
            }
        }
        EditorGUILayout.EndScrollView();
        if (GUI.changed) EditorSceneManager.MarkAllScenesDirty();
    }

    private string[] getPrefabs()
    {
        string[] prefabs = new string[0];
        string[] prefabGuids = AssetDatabase.FindAssets("", new[] {prefabPath});
        prefabs = new string[prefabGuids.Length];

        for(int i = 0; i < prefabs.Length; i++)
        {
            GameObject essentialPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabGuids[i]), typeof(GameObject));
            prefabs[i] = essentialPrefab.name;
        }
        return prefabs;
    }

    private static GUIStyle selectedItemStyle
    {
        get
        {
            GUIStyle item = new GUIStyle();
            item.fontStyle = FontStyle.Bold;
            return item;
        }
    }

    private void SpawnEssentials()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("", new[] {prefabPath});
        for(int i = 0; i < prefabGuids.Length; i++)
        {
            if(selectedEssentialsToSpawn[i])
            {
                GameObject prefabToSpawn = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabGuids[i]), typeof(GameObject));
                if(!CheckIfEssentialIsSpawnedOnScene(prefabToSpawn.name)) PrefabUtility.InstantiatePrefab(prefabToSpawn);
                selectedEssentialsToSpawn[i] = false;
            }
        }
    }

    private bool CheckIfEssentialIsSpawnedOnScene(string prefab)
    {
        GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
        for(int i = 0; i < sceneObjects.Length; i++)
        {
            if(sceneObjects[i].name == prefab)
            {
                return true;
            }
        }
        return false;
    }

    private void DestroyEssential(string prefab)
    {
        GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
        for(int i = 0; i < sceneObjects.Length; i++)
        {
            if(sceneObjects[i].name == prefab)
            {
                SceneAsset.DestroyImmediate(sceneObjects[i]);
                break;
            }
        }
    }

    private bool CheckIfIsAnEssentialOnScene()
    {
        for(int i = 0; i < essentialPrefabs.Length; i++)
        {
            if(CheckIfEssentialIsSpawnedOnScene(essentialPrefabs[i])) return true;
        }
        return false;
    }
}
