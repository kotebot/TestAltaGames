using Ostryzhnyi.DI;
using Ostryzhnyi.SphereSize.Scripts.Map.Api;
using UnityEngine;

namespace Ostryzhnyi.SphereSize.Scripts.Map.Impl
{
    public class MapCreator : MonoBehaviour, ILevelNumber
    {
        public const string LevelNumberKey = "LevelNumber";
        public int LevelNumber { get => PlayerPrefs.GetInt(LevelNumberKey, 0); set => PlayerPrefs.SetInt(LevelNumberKey, value); }
        
        [Inject] private LevelData _levelData;

        [SerializeField] private GameObject _blockPrefab;

        private void Start()
        {
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            if (LevelNumber >= _levelData.Levels.Count)
                LevelNumber = 0;
            
            var level = _levelData.Levels[LevelNumber];

            foreach (Vector3 localPosition in level.Positions)
            {
                Vector3 worldPosition = transform.TransformPoint(localPosition);
                GameObject newBlock = Instantiate(_blockPrefab, worldPosition, Quaternion.identity);
                newBlock.transform.parent = transform;
            }
        }
    }
}