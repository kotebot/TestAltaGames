using Ostryzhnyi.SphereSize.Scripts.Map;
using Ostryzhnyi.Tools.Editor;
using UnityEditor;
using UnityEngine;

namespace Ostryzhnyi.SphereSize.Editor
{
    public class LevelGeneratorWindow : EditorWindow
    {
        private LevelGenerator _levelGenerator;
        private Vector2 _scrollPosition;
        private int _numberOfLevelsToGenerate = 1;

        [MenuItem("Tools/Level Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelGeneratorWindow>("Level Generator");
            window.Initialize();
            SceneView.duringSceneGui += window.OnSceneGUI;
        }

        private void Initialize()
        {
            _levelGenerator = new LevelGenerator();
            LoadSettings();
        }

        private void OnEnable()
        {
            if (_levelGenerator == null)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.Space();
            GUILayout.Label("Level Generator Settings", EditorStyles.boldLabel);

            DrawSettings();
            DrawLevelManagement();
            DrawLevelActions();

            EditorGUILayout.EndScrollView();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            _levelGenerator?.OnSceneGUI(sceneView);
        }

        private void DrawSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            _levelGenerator.CenterElement = (Transform)EditorGUILayout.ObjectField("Center Element", _levelGenerator.CenterElement, typeof(Transform), true);
            _levelGenerator.BlockerPrefab = (GameObject)EditorGUILayout.ObjectField("Blocker Prefab", _levelGenerator.BlockerPrefab, typeof(GameObject), false);

            DrawRandomizableValue("Amount of Elements", _levelGenerator.AmountElement);
            DrawRandomizableValue("Radius Between Blocks", _levelGenerator.RadiusBetweenBlocks);

            _levelGenerator.Width = EditorGUILayout.FloatField("Width of Spawn Area", _levelGenerator.Width);
            _levelGenerator.Length = EditorGUILayout.FloatField("Length of Spawn Area", _levelGenerator.Length);
            EditorGUILayout.EndVertical();
        }

        private void DrawRandomizableValue(string label, RandomizableValue value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            value.Mode = (RandomizableValue.RandomMode)EditorGUILayout.EnumPopup(value.Mode, GUILayout.MaxWidth(150));

            switch (value.Mode)
            {
                case RandomizableValue.RandomMode.Constant:
                    value.ConstantValue = EditorGUILayout.FloatField(value.ConstantValue);
                    break;
                case RandomizableValue.RandomMode.RandomBetweenTwoConstants:
                    value.MinValue = EditorGUILayout.FloatField(value.MinValue);
                    value.MaxValue = EditorGUILayout.FloatField(value.MaxValue);
                    break;
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLevelManagement()
        {
            EditorGUILayout.Space();
            _levelGenerator.LevelData = (LevelData)EditorGUILayout.ObjectField("Level Data", _levelGenerator.LevelData, typeof(LevelData), false);

            if (_levelGenerator.LevelData == null) return;

            EditorGUILayout.Space();
            GUILayout.Label("Level Management", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            if (_levelGenerator.LevelData.Levels.Count > 0)
            {
                _levelGenerator.SelectedLevelIndex = EditorGUILayout.IntSlider("Select Level", _levelGenerator.SelectedLevelIndex, 0, _levelGenerator.LevelData.Levels.Count - 1);
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New Level")) _levelGenerator.AddNewLevel();
            if (GUILayout.Button("Delete Selected Level")) _levelGenerator.DeleteSelectedLevel();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawLevelActions()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Level Actions", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Level")) _levelGenerator.GenerateLevel();
            if (GUILayout.Button("Save Level Data")) _levelGenerator.SaveLevelData();
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Load Level Data")) _levelGenerator.LoadLevelData();
            if (GUILayout.Button("Clear current map")) _levelGenerator.ClearExistingBlocks();

            EditorGUILayout.Space();
            GUILayout.Label("Generate Multiple Levels", EditorStyles.boldLabel);
            _numberOfLevelsToGenerate = EditorGUILayout.IntField("Number of Levels", _numberOfLevelsToGenerate);
            if (GUILayout.Button("Generate Multiple Levels"))
            {
                _levelGenerator.GenerateMultipleLevels(_numberOfLevelsToGenerate);
            }

            SaveSettings();
        }

        private void LoadSettings()
        {
            _levelGenerator.CenterElement = LoadSetting<Transform>("LG_CenterElement");
            _levelGenerator.BlockerPrefab = LoadSetting<GameObject>("LG_BlockerPrefab");
            if (_levelGenerator.BlockerPrefab == null)
            {
                _levelGenerator.BlockerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Ostryzhnyi/SphereSize/Scripts/Blockers/Impl/Prefabs/Blocker.prefab");
            }

            _levelGenerator.AmountElement = LoadRandomizableValue("LG_AmountElement");
            if (_levelGenerator.AmountElement == null || _levelGenerator.AmountElement.Mode == 0)
            {
                _levelGenerator.AmountElement = new RandomizableValue
                {
                    Mode = RandomizableValue.RandomMode.RandomBetweenTwoConstants,
                    MinValue = 10f,
                    MaxValue = 15f
                };
            }

            _levelGenerator.RadiusBetweenBlocks = LoadRandomizableValue("LG_RadiusBetweenBlocks");
            if (_levelGenerator.RadiusBetweenBlocks == null || _levelGenerator.RadiusBetweenBlocks.Mode == 0)
            {
                _levelGenerator.RadiusBetweenBlocks = new RandomizableValue
                {
                    Mode = RandomizableValue.RandomMode.RandomBetweenTwoConstants,
                    MinValue = 0.8f,
                    MaxValue = 2f
                };
            }

            _levelGenerator.LevelData = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/Ostryzhnyi/SphereSize/Scripts/Map/Data/LevelData.asset");

            _levelGenerator.Width = EditorPrefs.GetFloat("LG_Width", 5f);
            _levelGenerator.Length = EditorPrefs.GetFloat("LG_Length", 15f);

            _levelGenerator.SelectedLevelIndex = EditorPrefs.GetInt("LG_SelectedLevelIndex", 0);
        }

        private void SaveSettings()
        {
            SaveSetting("LG_CenterElement", _levelGenerator.CenterElement);
            SaveSetting("LG_BlockerPrefab", _levelGenerator.BlockerPrefab);
            SaveRandomizableValue("LG_AmountElement", _levelGenerator.AmountElement);
            SaveRandomizableValue("LG_RadiusBetweenBlocks", _levelGenerator.RadiusBetweenBlocks);
            EditorPrefs.SetFloat("LG_Width", _levelGenerator.Width);
            EditorPrefs.SetFloat("LG_Length", _levelGenerator.Length);
            EditorPrefs.SetInt("LG_SelectedLevelIndex", _levelGenerator.SelectedLevelIndex);

            if (_levelGenerator.LevelData != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(_levelGenerator.LevelData);
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

        private RandomizableValue LoadRandomizableValue(string key)
        {
            var value = new RandomizableValue();
            value.Mode = (RandomizableValue.RandomMode)EditorPrefs.GetInt(key + "_Mode", 0);
            value.ConstantValue = EditorPrefs.GetFloat(key + "_Constant", 1f);
            value.MinValue = EditorPrefs.GetFloat(key + "_Min", 0f);
            value.MaxValue = EditorPrefs.GetFloat(key + "_Max", 1f);
            return value;
        }

        private void SaveRandomizableValue(string key, RandomizableValue value)
        {
            EditorPrefs.SetInt(key + "_Mode", (int)value.Mode);
            EditorPrefs.SetFloat(key + "_Constant", value.ConstantValue);
            EditorPrefs.SetFloat(key + "_Min", value.MinValue);
            EditorPrefs.SetFloat(key + "_Max", value.MaxValue);
        }
    }
}