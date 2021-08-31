using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
public class ScenarioPrefabsLoaderWindow : EditorWindow
{
    Vector2 scrollPos;
    GameObject selectedPrefabGameObject;

    #region scenarioPrefabsVariables
    string[] scenarioPrefabs = new string[0];
    bool[] scenarioPrefabsSelectedToSpawn = new bool[0];
    bool hasScenarioPrefabsFetched;
    string scenarioPrefabsPath = "Assets/Prefabs/ScenarioPrefabs";
    AnimBool scenarioPrefabAnimBool;
    AnimBool scenarioPrefabInstantiatedAnimBool;
    string[] uniqueLevelPrefabs = new string[0];
    bool[] uniqueLevelPrefabsSelectedToDelete = new bool[0];
    string[] containerPrefabsSpawned = new string[0];
    bool[] containerPrefabsSelectedToDelete = new bool[0];
    string[] containablePrefabs = {"CheckPoint","DialogPoint", "ItemCrate", "Keycard", "RestorePoint", "SavingPoint"};
    string[] containerPrefabs = {"LevelCheckpoints", "LevelDialogPoints", "LevelItemCrates", "LevelKeycards", "LevelRestorePoints", "LevelSavingPoints"};
    bool[] containersSpawned = new bool[6];
    string[] prefabsContained = new string[0];
    string[] otherPrefabs = new string[0];
    bool[] otherPrefabsSelectedToDelete = new bool[0];
    #endregion

    [MenuItem("Improvus/Scenario Prefabs Manager Window")]
    public static void OpenWindow()
    {
        var window = GetWindow(typeof(ScenarioPrefabsLoaderWindow)) as ScenarioPrefabsLoaderWindow;
        window.Show();
    }

    private void OnEnable()
    {
        scenarioPrefabAnimBool = SetNewAnimBool(scenarioPrefabAnimBool);
        scenarioPrefabInstantiatedAnimBool = SetNewAnimBool(scenarioPrefabInstantiatedAnimBool);
    }

    private void OnGUI()
    {
        if(uniqueLevelPrefabs.Length == 0)
        {
            uniqueLevelPrefabs = GetGameObjectsByTag(uniqueLevelPrefabs, "UniqueLevelObject");
            uniqueLevelPrefabsSelectedToDelete = SetBoolArrayLength(uniqueLevelPrefabsSelectedToDelete, uniqueLevelPrefabs.Length);
        }
        if(containerPrefabsSpawned.Length == 0)
        {
            containerPrefabsSpawned = GetGameObjectsByTag(containerPrefabsSpawned, "Container");
            containerPrefabsSelectedToDelete = SetBoolArrayLength(containerPrefabsSelectedToDelete, containerPrefabsSpawned.Length);
        }
        if(otherPrefabs.Length == 0)
        {
            otherPrefabs = SetOtherPrefabsArray(scenarioPrefabs, otherPrefabs);
            otherPrefabsSelectedToDelete = SetBoolArrayLength(otherPrefabsSelectedToDelete, otherPrefabs.Length);
        }
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
        #region Scenario Prefabs
        EditorGUILayout.HelpBox("On this window you can instantiate or delete all the prefabs which are considered necessary for the correct level function. Most of this prefabs will be called by the Essential Prefabs so, in order to avoid errors, at least most of this prefabs must be instantiated on scene.", MessageType.Info);
        EditorGUILayout.Space();
        if(GUILayout.Button("Find Scenario Prefabs"))
        {
            scenarioPrefabs = GetPrefabsOnAssetDatabase(scenarioPrefabs, scenarioPrefabsPath);
            scenarioPrefabsSelectedToSpawn = new bool[scenarioPrefabs.Length];
            hasScenarioPrefabsFetched = true;
        }
        EditorGUILayout.Space();
        if(!hasScenarioPrefabsFetched)
        {
            EditorGUILayout.HelpBox("You must click the 'Find Scenario Prefabs' button in order to show the scenario prefabs in this window.",MessageType.Info);
        }
        else
        {
            scenarioPrefabAnimBool.target = EditorGUILayout.Foldout(scenarioPrefabAnimBool.target,"Scenario Prefabs");
            if(scenarioPrefabAnimBool.target == true)
            {
                EditorGUILayout.BeginFadeGroup(scenarioPrefabAnimBool.faded);
                EditorGUILayout.HelpBox("On this grid, you can select the Scenario Prefabs to Spawn. Have in mind that Prefabs with a 'Container' or 'UniqueLevelObject' tag can only be instantiated once on Scene.\n\nWhen you select a Prefab, this will display an ObjectField where you can see the Prefab Location.", MessageType.Info);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("GameObject Name", selectedItemStyle);
                EditorGUILayout.LabelField("GameObject Tag", selectedItemStyle);
                EditorGUILayout.EndHorizontal();
                for(int i = 0; i < scenarioPrefabs.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    selectedPrefabGameObject = (GameObject) AssetDatabase.LoadAssetAtPath(scenarioPrefabsPath+"/"+scenarioPrefabs[i]+".prefab" , typeof(GameObject));
                    scenarioPrefabsSelectedToSpawn[i] = EditorGUILayout.Toggle(scenarioPrefabsSelectedToSpawn[i], GUILayout.Width(10f));
                    if(scenarioPrefabsSelectedToSpawn[i])
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(scenarioPrefabs[i], selectedItemStyle);
                        EditorGUILayout.ObjectField(selectedPrefabGameObject, typeof(GameObject));
                        EditorGUILayout.EndVertical();
                    } 
                    else EditorGUILayout.LabelField(scenarioPrefabs[i], labelItemStyle);
                    if(selectedPrefabGameObject.tag == "Untagged") EditorGUILayout.LabelField("");
                    else EditorGUILayout.LabelField(selectedPrefabGameObject.tag, labelItemTagStyle);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                if(GUILayout.Button("Select All Scenario Prefabs"))
                {
                    scenarioPrefabsSelectedToSpawn = SetAllBoolItemsOnArray(scenarioPrefabsSelectedToSpawn, true);
                }
                if(GUILayout.Button("Deselect All ScenarioPrefabs"))
                {
                    scenarioPrefabsSelectedToSpawn = SetAllBoolItemsOnArray(scenarioPrefabsSelectedToSpawn, false);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("Click on the following button in order to Spawn the selected prefabs", MessageType.Info);
                if(GUILayout.Button("Spawn Scenario Prefabs"))
                {
                    for(int i = 0; i < scenarioPrefabsSelectedToSpawn.Length; i++)
                    {
                        if(scenarioPrefabsSelectedToSpawn[i])
                        {
                            SpawnPrefab(scenarioPrefabsPath, i);
                            scenarioPrefabsSelectedToSpawn[i] = false;
                            uniqueLevelPrefabs = GetGameObjectsByTag(uniqueLevelPrefabs, "UniqueLevelObject");
                            uniqueLevelPrefabsSelectedToDelete = SetBoolArrayLength(uniqueLevelPrefabsSelectedToDelete, uniqueLevelPrefabs.Length);
                            containerPrefabsSpawned = GetGameObjectsByTag(containerPrefabsSpawned, "Container");
                            containerPrefabsSelectedToDelete = SetBoolArrayLength(containerPrefabsSelectedToDelete, containerPrefabsSpawned.Length);
                            otherPrefabs = SetOtherPrefabsArray(scenarioPrefabs, otherPrefabs);
                            otherPrefabsSelectedToDelete = SetBoolArrayLength(otherPrefabsSelectedToDelete, otherPrefabs.Length);
                        }
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            #endregion

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(5);
            if(!CheckIfAPrefabIsOnScene(scenarioPrefabs))
            {
                EditorGUILayout.HelpBox("You need at least a Scenario Prefab instantiated in order to see this section", MessageType.Warning);
            }
            else
            {
               
                scenarioPrefabInstantiatedAnimBool.target = EditorGUILayout.Foldout(scenarioPrefabInstantiatedAnimBool.target,"Prefabs Instantiated");
                if(scenarioPrefabInstantiatedAnimBool.target)
                {
                    GUILayout.Space(2);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Space(2);
                    
                    #region Unique Level Prefabs
                    GUILayout.Label("Unique Level Prefabs", EditorStyles.boldLabel);
                    if(uniqueLevelPrefabs.Length <= 0)
                    {
                       EditorGUILayout.HelpBox("You need at least a Prefab with 'Unique Level Object' tag in order to show this section.", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        for(int i = 0; i < uniqueLevelPrefabs.Length; i++)
                        {
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            uniqueLevelPrefabsSelectedToDelete[i] = EditorGUILayout.Toggle(uniqueLevelPrefabsSelectedToDelete[i]);
                            if(uniqueLevelPrefabsSelectedToDelete[i]) EditorGUILayout.LabelField(uniqueLevelPrefabs[i], selectedItemStyle);
                            else EditorGUILayout.LabelField(uniqueLevelPrefabs[i],labelItemStyle);
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginHorizontal();
                        if(GUILayout.Button("Select All Unique Level Objects"))
                        {
                            uniqueLevelPrefabsSelectedToDelete = SetAllBoolItemsOnArray(uniqueLevelPrefabsSelectedToDelete, true);
                        }
                        if(GUILayout.Button("Deselect All Unique Level Objects"))
                        {
                            uniqueLevelPrefabsSelectedToDelete = SetAllBoolItemsOnArray(uniqueLevelPrefabsSelectedToDelete, false);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Click on the following button in order to Delete the selected Unique Level Objects on Scene",MessageType.Info);
                        if(GUILayout.Button("Delete Selected Unique Level Objects"))
                        {
                            for(int i = 0; i < uniqueLevelPrefabsSelectedToDelete.Length; i++)
                            {
                                if(uniqueLevelPrefabsSelectedToDelete[i]) DeleteSelectedGameObjectOnScene(uniqueLevelPrefabs[i]);
                            }
                            uniqueLevelPrefabs = GetGameObjectsByTag(uniqueLevelPrefabs, "UniqueLevelObject");
                            uniqueLevelPrefabsSelectedToDelete = SetBoolArrayLength(uniqueLevelPrefabsSelectedToDelete, uniqueLevelPrefabs.Length);
                        }
                    }
                    EditorGUILayout.Space();
                    #endregion

                    GUILayout.Space(2);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Space(2);

                    #region Container Prefabs
                    GUILayout.Label("Container Prefabs", EditorStyles.boldLabel);
                    EditorGUILayout.HelpBox("On this section you'll see not only the 'Container' Prefabs instantiated on scene, but also all the prefabs which can be 'Containable Prefabs'. Which are the following:\n\n° CheckPoint\n° DialogPoint\n° ItemCrate\n° Keycard\n° RestorePoint\n° SavingPoint\n\nWhen you instantiate a 'Containable Prefab' and his container is on scene, this new 'Containable Prefab' instantiated will automatically be a child from his container (in order for its script work correctly when you play the game).\n\nAlso have in mind that if you choose to delete a Container and it has Containable Prefabs as childs, it will also delete its childs.", MessageType.Info);
                    if(containerPrefabsSpawned.Length <= 0)
                    {
                       EditorGUILayout.HelpBox("You need at least a Prefab with 'Container' tag in order to show this section.", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        containersSpawned = CheckForContainersSpawned(containerPrefabsSpawned, containersSpawned);
                        for(int i = 0; i < containerPrefabsSpawned.Length; i++)
                        {
                            int index = -1;
                            for(int j = 0; j < containerPrefabs.Length; j++)
                            {
                                if(containerPrefabsSpawned[i] == containerPrefabs[j])
                                {
                                    index = j;
                                    break;
                                }
                            }
                            if(index > -1) ContainGameObjects(containerPrefabsSpawned[i], index);
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            containerPrefabsSelectedToDelete[i] = EditorGUILayout.Toggle(containerPrefabsSelectedToDelete[i]);
                            EditorGUILayout.BeginVertical();
                            if(containerPrefabsSelectedToDelete[i]) EditorGUILayout.LabelField(containerPrefabsSpawned[i], selectedItemStyle);
                            else EditorGUILayout.LabelField(containerPrefabsSpawned[i],labelItemStyle);
                            if(containersSpawned[i])
                            {
                                prefabsContained = GetContainables(prefabsContained, i);
                            }
                            for(int j = 0; j < prefabsContained.Length; j++)
                            {
                                EditorGUILayout.LabelField("° "+prefabsContained[j]);
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginHorizontal();
                        if(GUILayout.Button("Select All Container Objects"))
                        {
                            containerPrefabsSelectedToDelete = SetAllBoolItemsOnArray(containerPrefabsSelectedToDelete, true);
                        }
                        if(GUILayout.Button("Deselect All Container Objects"))
                        {
                            containerPrefabsSelectedToDelete = SetAllBoolItemsOnArray(containerPrefabsSelectedToDelete, false);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Click on the following button in order to Delete the selected Container Objects on Scene",MessageType.Info);
                        if(GUILayout.Button("Delete Selected Container Objects"))
                        {
                            for(int i = 0; i < containerPrefabsSelectedToDelete.Length; i++)
                            {
                                if(containerPrefabsSelectedToDelete[i]) DeleteSelectedGameObjectOnScene(containerPrefabsSpawned[i]);
                            }
                            containerPrefabsSpawned = GetGameObjectsByTag(containerPrefabsSpawned, "Container");
                            containerPrefabsSelectedToDelete = SetBoolArrayLength(containerPrefabsSelectedToDelete, containerPrefabsSpawned.Length);
                        }
                    }
                    EditorGUILayout.Space();
                    #endregion

                    GUILayout.Space(2);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Space(2);
                    
                    #region Other Prefabs
                    GUILayout.Label("Other Prefabs", EditorStyles.boldLabel);
                    if(otherPrefabs.Length <= 0)
                    {
                        EditorGUILayout.HelpBox("You need at least a Prefab with doesn't have a 'Unique Level Object' or 'Container tag and must not be a 'Containable Prefab' in order to show this section.", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        for(int i = 0; i < otherPrefabs.Length; i++)
                        {
                            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                            otherPrefabsSelectedToDelete[i] = EditorGUILayout.Toggle(otherPrefabsSelectedToDelete[i]);
                            if(otherPrefabsSelectedToDelete[i]) EditorGUILayout.LabelField(otherPrefabs[i], selectedItemStyle);
                            else EditorGUILayout.LabelField(otherPrefabs[i],labelItemStyle);
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginHorizontal();
                        if(GUILayout.Button("Select All Container Objects"))
                        {
                            otherPrefabsSelectedToDelete = SetAllBoolItemsOnArray(otherPrefabsSelectedToDelete, true);
                        }
                        if(GUILayout.Button("Deselect All Container Objects"))
                        {
                            otherPrefabsSelectedToDelete = SetAllBoolItemsOnArray(otherPrefabsSelectedToDelete, false);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Click on the following button in order to Delete the selected Other Scenario GameObjects on Scene",MessageType.Info);
                        if(GUILayout.Button("Delete Selected Other Scenario Game Objects"))
                        {
                            for(int i = 0; i < otherPrefabsSelectedToDelete.Length; i++)
                            {
                                if(otherPrefabsSelectedToDelete[i]) DeleteSelectedGameObjectOnScene(otherPrefabs[i]);
                            }
                            otherPrefabs = SetOtherPrefabsArray(scenarioPrefabs, otherPrefabs);
                            otherPrefabsSelectedToDelete = SetBoolArrayLength(otherPrefabsSelectedToDelete, otherPrefabs.Length);
                        }
                    }
                    #endregion
                }
            }
            
        }

        EditorGUILayout.EndScrollView();
        if (GUI.changed) EditorSceneManager.MarkAllScenesDirty();    
    }

    private static GUIStyle selectedItemStyle
    {
        get
        {
            GUIStyle item = new GUIStyle();
            item.alignment = TextAnchor.MiddleCenter;
            item.fontStyle = FontStyle.Bold;
            return item;
        }
    }

    private static GUIStyle labelItemStyle
    {
        get
        {
            GUIStyle item = new GUIStyle();
            item.alignment = TextAnchor.MiddleCenter;
            return item;
        }
    }

    private static GUIStyle labelItemTagStyle
    {
        get
        {
            GUIStyle item = new GUIStyle();
            item.alignment = TextAnchor.MiddleCenter;
            item.normal.textColor = Color.blue;
            return item;
        }
    }

    private AnimBool SetNewAnimBool(AnimBool animBool)
    {
        animBool = new AnimBool(false);
        animBool.valueChanged.AddListener(Repaint);
        return animBool;
    }

    private bool[] SetAllBoolItemsOnArray(bool[] boolArray, bool flag)
    {
        for(int i = 0; i < boolArray.Length; i++)
        {
            boolArray[i] = flag;
        }
        return boolArray;
    }


    private string[] GetPrefabsOnAssetDatabase(string[] prefabArray, string prefabPath) //Devuelve un array de strings de los prefabs dentro del path asignado
    {
        prefabArray = new string[0];
        string[] prefabGuids = AssetDatabase.FindAssets("", new[] {prefabPath});
        prefabArray = new string[prefabGuids.Length];
        for(int i = 0; i < prefabArray.Length; i++)
        {
            GameObject newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabGuids[i]), typeof(GameObject));
            prefabArray[i] = newPrefab.name;
        }
        return prefabArray;
    }

    private void SpawnPrefab(string prefabPath, int index) //Spawnea el prefab
    {
        string[] prefabGuids = AssetDatabase.FindAssets("", new[] {prefabPath});
        GameObject prefabToSpawn = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefabGuids[index]), typeof(GameObject));
        if(!CheckIfPrefabIsSpawnedOnScene(prefabToSpawn.name)) PrefabUtility.InstantiatePrefab(prefabToSpawn);
        else if(!CheckTag(prefabToSpawn, "UniqueLevelObject") && !CheckTag(prefabToSpawn, "Container")) PrefabUtility.InstantiatePrefab(prefabToSpawn);
    }

    public bool CheckTag(GameObject prefab, string tag)
    {
        if(prefab.tag == tag) return true;
        else return false;
    }

    
    private bool CheckIfPrefabIsSpawnedOnScene(string prefab) //Chequeo si el prefab está instanciado en la escena
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

    private bool CheckIfAPrefabIsOnScene(string[] prefabs)
    {
         GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
         for(int i = 0; i < sceneObjects.Length; i++)
         {
             for(int j = 0; j < prefabs.Length; j++)
             {
                 if(sceneObjects[i].name == prefabs[j]) return true;
             }
         }

         return false;
    }

    private string[] GetGameObjectsByTag(string[] prefabs, string tag)
    {
        prefabs = new string[0];
        List<GameObject> sceneGameObjects = new List<GameObject>();
        GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
        for(int i = 0; i < sceneObjects.Length;i++)
        {
            if(CheckTag(sceneObjects[i], tag))
            {
                sceneGameObjects.Add(sceneObjects[i]);
            }
        }
        sceneGameObjects.Sort(sortByName);
        if(sceneGameObjects.Count > 0)
        {
            prefabs = new string[sceneGameObjects.Count];
            for(int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i] = sceneGameObjects[i].name;
            }
        }
        return prefabs;
    }

    public static int sortByName(GameObject item1, GameObject item2)
    {
        return(item1.name.CompareTo(item2.name));
    }

    private string[] SetOtherPrefabsArray(string[] prefabs, string[] otherPrefabs)
    {
        if(prefabs.Length <= 0) return otherPrefabs;
        else
        {
            List<GameObject> sceneGameObjects = new List<GameObject>();
            GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
            for(int i = 0; i < sceneObjects.Length; i++)
            {
                for(int j = 0; j < prefabs.Length; j++)
                {
                    if(sceneObjects[i].name == prefabs[j])
                    {
                        if(!CheckTag(sceneObjects[i], "UniqueLevelObject") && !CheckTag(sceneObjects[i], "Container")) 
                        {
                            if(!CheckIfIsContainable(sceneObjects[i]))
                            {
                                sceneGameObjects.Add(sceneObjects[i]);
                                break;
                            }
                        }
                    }
                }
            }
            sceneGameObjects.Sort(sortByName);
            otherPrefabs = new string[sceneGameObjects.Count];
            for(int i = 0; i < sceneGameObjects.Count; i++)
            {
                otherPrefabs[i] = sceneGameObjects[i].name;
            }
        }
        return otherPrefabs;
    }

    private void DeleteSelectedGameObjectOnScene(string selectedGameObject)
    {
        GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
        for(int i = 0; i < sceneObjects.Length;i++)
        {
            if(sceneObjects[i].name == selectedGameObject)
            {
                DestroyImmediate(sceneObjects[i]);
                break;
            }
        }
    }

    private bool[] SetBoolArrayLength(bool[] boolArray, int arrayLength)
    {
        boolArray = new bool[arrayLength];
        boolArray = SetAllBoolItemsOnArray(boolArray, false);
        return boolArray;
    }

    private bool[] CheckForContainersSpawned(string[] containers, bool[] boolArray)
    {
        if(containers.Length <=0) return boolArray;
        else
        {
            for(int i = 0; i < containers.Length; i++)
            {
                for(int j = 0; j < containerPrefabs.Length; j++)
                {
                    if(containers[i] == containerPrefabs[j])
                    {
                        boolArray[i] = true;
                        break;
                    }
                }
            }
        }
        return boolArray;
    }

    private bool CheckIfIsContainable(GameObject item)
    {
        for(int i = 0; i < containablePrefabs.Length; i++)
        {
            if(item.name == containablePrefabs[i]) return true;
        }
        return false;
    }

    private string[] GetContainables(string[] containables, int containableIndex)
    {
        GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
        List<GameObject> sceneGameObjects = new List<GameObject>();
        for(int i = 0; i < sceneObjects.Length; i++)
        {
            if(CheckIfIsContainable(sceneObjects[i]))
            {
                if(sceneObjects[i].name == containablePrefabs[containableIndex]) sceneGameObjects.Add(sceneObjects[i]);
            }
        }
        sceneGameObjects.Sort(sortByName);
        containables = new string[sceneGameObjects.Count];
        for(int i = 0; i < containables.Length; i++)
        {
            containables[i] = sceneGameObjects[i].name;
        }
        return containables;
    }

    private void ContainGameObjects(string container, int containableIndex)
    {
         GameObject[] sceneObjects = SceneAsset.FindObjectsOfType<GameObject>();
         GameObject containerGameObject = null;

         for(int i = 0; i < sceneObjects.Length; i++)
         {
             if(sceneObjects[i].name == container)
             {
                 containerGameObject = sceneObjects[i];
             }
         }

         for(int i = 0; i < sceneObjects.Length; i++)
         {
            if(sceneObjects[i].name == containablePrefabs[containableIndex])
            {
                sceneObjects[i].transform.SetParent(containerGameObject.transform);
            }
         }
    }
}
