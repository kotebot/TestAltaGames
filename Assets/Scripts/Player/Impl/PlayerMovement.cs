using System.Collections;
using Player.Api;
using Player.Impl.BaseClasses;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Player.Impl
{
    public class PlayerMovement : BaseMovement
    {
        [Inject] private IPlayer _player;


        protected override bool CanMove()
        {
            return _player.IsAlive;
        }
    }
}