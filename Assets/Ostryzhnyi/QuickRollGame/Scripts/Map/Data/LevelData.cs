using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Map.Data
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data", order = 1)]
    public class LevelData : ScriptableObject
    {
        public List<BlockerPositions> Levels = new();
    }

    [Serializable]
    public class BlockerPositions
    {
        public Vector3[] Positions = Array.Empty<Vector3>();
    }
}