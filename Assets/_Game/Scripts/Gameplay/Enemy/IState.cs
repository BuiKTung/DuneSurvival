using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Gameplay.Enemy
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }
}