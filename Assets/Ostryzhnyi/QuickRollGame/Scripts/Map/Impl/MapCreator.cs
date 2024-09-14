using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Data;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Map.Impl
{
    public class MapCreator : MonoBehaviour, ILevelNumber
    {
        public const string LevelNumberKey = "LevelNumber";

        [SerializeField] private GameObject _blockPrefab;

        [Inject] private LevelData _levelData;

        private void Start()
        {
            GenerateLevel();
        }

        public int LevelNumber
        {
            get => PlayerPrefs.GetInt(LevelNumberKey, 0);
            set => PlayerPrefs.SetInt(LevelNumberKey, value);
        }

        private void GenerateLevel()
        {
            if (LevelNumber >= _levelData.Levels.Count)
                LevelNumber = 0;

            var level = _levelData.Levels[LevelNumber];

            foreach (var localPosition in level.Positions)
            {
                var worldPosition = transform.TransformPoint(localPosition);
                var newBlock = Instantiate(_blockPrefab, worldPosition, Quaternion.identity);
                newBlock.transform.parent = transform;
            }
        }
    }
}