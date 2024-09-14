using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ostryzhnyi.SphereSize.Scripts.Map
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<BlockerPositions> Levels = new List<BlockerPositions>();

    }

    [System.Serializable]
    public class BlockerPositions
    {
        public Vector3[] Positions = Array.Empty<Vector3>();
    }
}