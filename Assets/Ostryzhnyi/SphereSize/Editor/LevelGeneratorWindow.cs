using Ostryzhnyi.SphereSize.Scripts.Map;
using UnityEditor;
using UnityEngine;

namespace Ostryzhnyi.SphereSize.Editor
{
#if UNITY_EDITOR
     public class LevelGeneratorWindow : EditorWindow
    {
        private Transform _centerElement;
        private GameObject _blockerPrefab;
        private int amountElement;
        private float _radiusBetweenBlocks;
        private float _width;
        private float _length;
        private LevelData _levelData;
        private int selectedLevelIndex = 0;

        [MenuItem("Sphere Size/Level Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelGeneratorWindow>("Level Generator");
            SceneView.duringSceneGui += window.OnSceneGUI;
        }

        private void OnEnable()
        {
            LoadSettings();
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Level Generator Settings", EditorStyles.boldLabel);

            DrawSettings();
            DrawLevelManagement();
            DrawLevelActions();
        }

        private void DrawSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            _centerElement = (Transform)EditorGUILayout.ObjectField("Center Element", _centerElement, typeof(Transform), true);
            _blockerPrefab = (GameObject)EditorGUILayout.ObjectField("Blocker Prefab", _blockerPrefab, typeof(GameObject), false);
            amountElement = EditorGUILayout.IntField("Amount of Elements", amountElement);
            _radiusBetweenBlocks = EditorGUILayout.FloatField("Radius Between Blocks", _radiusBetweenBlocks);
            _width = EditorGUILayout.FloatField("Width of Spawn Area", _width);
            _length = EditorGUILayout.FloatField("Length of Spawn Area", _length);
            EditorGUILayout.EndVertical();
        }

        private void DrawLevelManagement()
        {
            EditorGUILayout.Space();
            _levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", _levelData, typeof(LevelData), false);

            if (_levelData == null) return;

            EditorGUILayout.Space();
            GUILayout.Label("Level Management", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            if (_levelData.Levels.Count > 0)
            {
                selectedLevelIndex = EditorGUILayout.IntSlider("Select Level", selectedLevelIndex, 0, _levelData.Levels.Count - 1);
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New Level")) AddNewLevel();
            if (GUILayout.Button("Delete Selected Level")) DeleteSelectedLevel();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawLevelActions()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Level Actions", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Level")) GenerateLevel();
            if (GUILayout.Button("Save Level Data")) SaveLevelData();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Load Level Data")) LoadLevelData();
            if (GUILayout.Button("Clear current map")) ClearExistingBlocks();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (_centerElement == null) return;

            Handles.color = new Color(0, 1, 0, 0.25f);
            Vector3 center = _centerElement.position;
            Vector3 size = new Vector3(_width, 0, _length);

            Handles.color = Color.green;
            Handles.DrawWireCube(center, size);
        }

        private void LoadSettings()
        {
            _centerElement = LoadSetting<Transform>("LG_CenterElement");
            _blockerPrefab = LoadSetting<GameObject>("LG_BlockerPrefab");
            amountElement = EditorPrefs.GetInt("LG_AmountElement", 10);
            _radiusBetweenBlocks = EditorPrefs.GetFloat("LG_RadiusBetweenBlocks", 2f);
            _width = EditorPrefs.GetFloat("LG_Width", 5f);
            _length = EditorPrefs.GetFloat("LG_Length", 15f);
            selectedLevelIndex = EditorPrefs.GetInt("LG_SelectedLevelIndex", 0);

            string levelDataPath = EditorPrefs.GetString("LG_LevelDataPath", string.Empty);
            if (!string.IsNullOrEmpty(levelDataPath))
            {
                _levelData = AssetDatabase.LoadAssetAtPath<LevelData>(levelDataPath);
            }
        }

        private void SaveSettings()
        {
            SaveSetting("LG_CenterElement", _centerElement);
            SaveSetting("LG_BlockerPrefab", _blockerPrefab);
            EditorPrefs.SetInt("LG_AmountElement", amountElement);
            EditorPrefs.SetFloat("LG_RadiusBetweenBlocks", _radiusBetweenBlocks);
            EditorPrefs.SetFloat("LG_Width", _width);
            EditorPrefs.SetFloat("LG_Length", _length);
            EditorPrefs.SetInt("LG_SelectedLevelIndex", selectedLevelIndex);

            if (_levelData != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(_levelData);
                EditorPrefs.SetString("LG_LevelDataPath", assetPath);
            }
        }

        private T LoadSetting<T>(string key) where T : UnityEngine.Object
        {
            return EditorPrefs.HasKey(key) ? EditorUtility.InstanceIDToObject(EditorPrefs.GetInt(key)) as T : null;
        }

        private void SaveSetting<T>(string key, T obj) where T : UnityEngine.Object
        {
            if (obj != null)
            {
                EditorPrefs.SetInt(key, obj.GetInstanceID());
            }
        }

        private void GenerateLevel()
        {
            if (_centerElement == null || _blockerPrefab == null)
            {
                Debug.LogError("Center Element or Blocker Prefab is not set.");
                return;
            }
            
            ClearExistingBlocks();

            Vector3[] positions = new Vector3[amountElement];

            for (int i = 0; i < amountElement; i++)
            {
                Vector3 spawnPosition = GenerateValidPosition(positions, i);
                if (spawnPosition == Vector3.zero) continue;

                GameObject newBlock = Instantiate(_blockerPrefab, spawnPosition, Quaternion.identity);
                newBlock.transform.parent = _centerElement;
                positions[i] = spawnPosition;
            }

            if (_levelData != null && selectedLevelIndex < _levelData.Levels.Count)
            {
                _levelData.Levels[selectedLevelIndex].Positions = positions;
                EditorUtility.SetDirty(_levelData);
            }

            SaveSettings();
        }

        private Vector3 GenerateValidPosition(Vector3[] positions, int currentIndex)
        {
            int attempts = 0;
            while (attempts < 1000)
            {
                float xPos = Random.Range(-_width / 2, _width / 2);
                float zPos = Random.Range(-_length / 2, _length / 2);
                Vector3 spawnPosition = _centerElement.position + new Vector3(xPos, 0, zPos);

                if (IsPositionValid(spawnPosition, positions, currentIndex))
                {
                    return spawnPosition;
                }
                attempts++;
            }

            Debug.LogWarning("Could not find valid position after 1000 attempts.");
            return Vector3.zero;
        }

        private bool IsPositionValid(Vector3 position, Vector3[] positions, int currentIndex)
        {
            for (int i = 0; i < currentIndex; i++)
            {
                if (Vector3.Distance(position, positions[i]) < _radiusBetweenBlocks)
                {
                    return false;
                }
            }
            return true;
        }

        private void SaveLevelData()
        {
            if (_levelData == null || selectedLevelIndex >= _levelData.Levels.Count)
            {
                Debug.LogError("No LevelData asset assigned or invalid level selected.");
                return;
            }

            Vector3[] positions = new Vector3[_centerElement.childCount];
            for (int i = 0; i < _centerElement.childCount; i++)
            {
                positions[i] = _centerElement.GetChild(i).position;
            }

            _levelData.Levels[selectedLevelIndex].Positions = positions;
            EditorUtility.SetDirty(_levelData);
            Debug.Log("Level data saved.");

            SaveSettings();
        }

        private void LoadLevelData()
        {
            if (_levelData == null || selectedLevelIndex >= _levelData.Levels.Count)
            {
                Debug.LogError("No LevelData asset assigned or invalid level selected.");
                return;
            }

            ClearExistingBlocks();

            foreach (Vector3 blockPosition in _levelData.Levels[selectedLevelIndex].Positions)
            {
                GameObject newBlock = Instantiate(_blockerPrefab, blockPosition, Quaternion.identity);
                newBlock.transform.parent = _centerElement;
            }

            Debug.Log("Level data loaded.");

            SaveSettings();
        }

        private void ClearExistingBlocks()
        {
            for (int i = _centerElement.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_centerElement.GetChild(i).gameObject);
            }

            SaveSettings();
        }

        private void AddNewLevel()
        {
            if (_levelData != null)
            {
                _levelData.Levels.Add(new BlockerPositions());
                selectedLevelIndex = _levelData.Levels.Count - 1;
                EditorUtility.SetDirty(_levelData);
            }

            SaveSettings();
        }

        private void DeleteSelectedLevel()
        {
            if (_levelData == null || selectedLevelIndex >= _levelData.Levels.Count) return;

            _levelData.Levels.RemoveAt(selectedLevelIndex);
            selectedLevelIndex = Mathf.Clamp(selectedLevelIndex - 1, 0, _levelData.Levels.Count - 1);
            EditorUtility.SetDirty(_levelData);

            SaveSettings();
        }
    }
#endif
   
}
