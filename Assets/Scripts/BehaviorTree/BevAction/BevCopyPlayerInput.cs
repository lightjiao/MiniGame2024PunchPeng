using UnityEngine;

namespace PunchPeng
{
    public class BevCopyPlayerInput : BevDurationAction
    {
        private int m_CopyedPlayerId;

        private bool m_AlreadyHasCopyAI = false;

        public override void OnStart()
        {
            m_CopyedPlayerId = Random.Range(0, 2);
            m_CfgDuration = Random.Range(1f, 3f);

            m_AlreadyHasCopyAI = GameController.Inst.HasCopyAI;
            GameController.Inst.HasCopyAI.RefCnt++;

            base.OnStart();
        }

        public override TaskStatus OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (m_AlreadyHasCopyAI)
            {
                return TaskStatus.Success;
            }

            m_Player.InputMoveDir = PlayerInputManagerHelper.Inst.GetPlayerInputData(m_CopyedPlayerId).MoveDir;
            m_Player.InputRun = PlayerInputManagerHelper.Inst.GetPlayerInputData(m_CopyedPlayerId).Run;

            return TaskStatusByDuration;
        }

        public override void OnEnd()
        {
            GameController.Inst.HasCopyAI.RefCnt--;
            m_Player.InputMoveDir = Vector3.zero;
            m_Player.InputRun = false;

            base.OnEnd();
        }
    }
}