using Ostryzhnyi.QuickRollGame.Scripts.Map.Data;
using Ostryzhnyi.Tools.Editor;
using UnityEditor;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Editor
{
    public class LevelGenerator
    {
        public Transform CenterElement { get; set; }
        public GameObject BlockerPrefab { get; set; }
        public RandomizableValue AmountElement { get; set; } = new();
        public RandomizableValue RadiusBetweenBlocks { get; set; } = new();
        public float Width { get; set; }
        public float Length { get; set; }
        public LevelData LevelData { get; set; }
        public int SelectedLevelIndex { get; set; }

        public void GenerateLevel()
        {
            if (CenterElement == null || BlockerPrefab == null)
            {
                Debug.LogError("Center Element or Blocker Prefab is not set.");
                return;
            }

            ClearExistingBlocks();

            var elements = Mathf.RoundToInt(AmountElement.GetValue());
            var positions = new Vector3[elements];

            for (var i = 0; i < elements; i++)
            {
                var spawnPosition = GenerateValidPosition(positions, i);
                if (spawnPosition == Vector3.zero) continue;

                var newBlock = Object.Instantiate(BlockerPrefab, spawnPosition, Quaternion.identity);
                newBlock.transform.parent = CenterElement;
                positions[i] = CenterElement.InverseTransformPoint(spawnPosition);
            }

            if (LevelData != null && SelectedLevelIndex < LevelData.Levels.Count)
            {
                LevelData.Levels[SelectedLevelIndex].Positions = positions;
                EditorUtility.SetDirty(LevelData);
            }
        }

        public void GenerateMultipleLevels(int numberOfLevels)
        {
            if (numberOfLevels <= 0)
            {
                Debug.LogError("Number of levels to generate must be greater than 0.");
                return;
            }

            for (var i = 0; i < numberOfLevels; i++)
            {
                Debug.Log($"Generating level {i + 1} of {numberOfLevels}...");

                GenerateLevel();

                SaveLevelData();

                if (i < numberOfLevels - 1) AddNewLevel();
            }

            Debug.Log($"{numberOfLevels} levels generated successfully.");
        }

        public void SaveLevelData()
        {
            if (LevelData == null || SelectedLevelIndex >= LevelData.Levels.Count)
            {
                Debug.LogError("No LevelData asset assigned or invalid level selected.");
                return;
            }

            var positions = new Vector3[CenterElement.childCount];
            for (var i = 0; i < CenterElement.childCount; i++) positions[i] = CenterElement.GetChild(i).localPosition;

            LevelData.Levels[SelectedLevelIndex].Positions = positions;
            EditorUtility.SetDirty(LevelData);
            Debug.Log("Level data saved.");
        }

        public void LoadLevelData()
        {
            if (LevelData == null || SelectedLevelIndex >= LevelData.Levels.Count)
            {
                Debug.LogError("No LevelData asset assigned or invalid level selected.");
                return;
            }

            ClearExistingBlocks();

            foreach (var localPosition in LevelData.Levels[SelectedLevelIndex].Positions)
            {
                var worldPosition = CenterElement.TransformPoint(localPosition);
                var newBlock = Object.Instantiate(BlockerPrefab, worldPosition, Quaternion.identity);
                newBlock.transform.parent = CenterElement;
            }

            Debug.Log("Level data loaded.");
        }

        public void ClearExistingBlocks()
        {
            for (var i = CenterElement.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(CenterElement.GetChild(i).gameObject);
        }

        public void AddNewLevel()
        {
            if (LevelData != null)
            {
                LevelData.Levels.Add(new BlockerPositions());
                SelectedLevelIndex = LevelData.Levels.Count - 1;
                EditorUtility.SetDirty(LevelData);
            }
        }

        public void DeleteSelectedLevel()
        {
            if (LevelData == null || SelectedLevelIndex >= LevelData.Levels.Count) return;

            LevelData.Levels.RemoveAt(SelectedLevelIndex);
            SelectedLevelIndex = Mathf.Clamp(SelectedLevelIndex - 1, 0, LevelData.Levels.Count - 1);
            EditorUtility.SetDirty(LevelData);
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (CenterElement == null) return;

            Handles.color = new Color(0, 1, 0, 0.25f);
            var center = CenterElement.position;
            var size = new Vector3(Width, 0, Length);

            Handles.color = Color.green;
            Handles.DrawWireCube(center, size);
        }

        private Vector3 GenerateValidPosition(Vector3[] positions, int currentIndex)
        {
            var attempts = 0;
            var radius = RadiusBetweenBlocks.GetValue();
            while (attempts < 1000)
            {
                var xPos = Random.Range(-Width / 2, Width / 2);
                var zPos = Random.Range(-Length / 2, Length / 2);
                var spawnPosition = CenterElement.position + new Vector3(xPos, 0, zPos);

                if (IsPositionValid(spawnPosition, positions, currentIndex, radius)) return spawnPosition;
                attempts++;
            }

            Debug.LogWarning("Could not find valid position after 1000 attempts.");
            return Vector3.zero;
        }


        private bool IsPositionValid(Vector3 position, Vector3[] positions, int currentIndex, float radius)
        {
            for (var i = 0; i < currentIndex; i++)
                if (Vector3.Distance(position, positions[i]) < radius)
                    return false;
            return true;
        }
    }
}