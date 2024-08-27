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
            if (LevelNumber > _levelData.Levels.Count)
                LevelNumber = 0;
            
            var level = _levelData.Levels[LevelNumber];

            foreach (var position in level.Positions)
            {
                var block = Instantiate(_blockPrefab, position, Quaternion.identity, transform).transform;
                block.localPosition = new Vector3(block.localPosition.x, 0, block.localPosition.z);
            }
        }
    }
}