namespace _Game.Scripts.Gameplay.Enemy
{
    public class IdleState_Melee : EnemyState
    {
        private Enemy_Melee enemy;
        public IdleState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
        {
            enemy = enemyBase as Enemy_Melee;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateTimer = enemyBase.idleTime;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnExecute()
        {
            base.OnExecute();
            if(stateTimer < 0)
                stateMachine.ChangeState(enemy.moveState);
        }
    }
}
