using UnityEngine;

namespace _Game.Scripts.Utilities
{
    public static class ConstantString 
    {
        public static class AnimationParameter
        {
            public const string XVelocity = "xVelocity";
            public const string ZVelocity = "zVelocity";
            public const string IsRunning = "isRunning";
            public const string Fire = "Fire";
            public const string Reload = "Reload";
            public const string ReloadSpeed = "ReloadSpeed";    
            public const string EquipType = "EquipType";
            public const string EquipWeapon = "EquipWeapon";
            public const string EquipSpeed = "EquipSpeed";
            public const string BusyEquipingWeapon = "BusyEquipingWeapon";

            #region EnemyParameter

            public const string IdleState = "Idle";
            public const string MoveState = "Move";
            public const string ChaseState = "Chase";
            public const string RecoveryState = "Recovery";
            public const string AttackState = "Attack";
            public const string DeadState = "Dead";
            public const string Dodge = "Dodge";
            
            public const string AttackAnimationSpeed = "AttackAnimationSpeed";
            public const string AttackIndex = "AttackIndex";
            public const string ChaseIndex = "ChaseIndex";
            public const string RecoveryIndex = "RecoveryIndex";
            public const string SlashAttackIndex = "SlashAttackIndex";
            
            public const int SlashAttackCount = 6;

            #endregion
        }
    }
}
