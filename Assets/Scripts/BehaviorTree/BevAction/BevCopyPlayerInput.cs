using UnityEngine;

namespace PunchPeng
{
    public class BevCopyPlayerInput : BevDurationAction
    {
        private int m_CopyedPlayerId;

        private bool m_AlreadyHasCopyAI = false;

        public override void OnBevStart()
        {
            m_CopyedPlayerId = Random.Range(0, 2);
            m_CfgDuration = Random.Range(1f, 3f);

            m_AlreadyHasCopyAI = LevelController.Inst.CopyPlayerInputAICount > 0;
            LevelController.Inst.CopyPlayerInputAICount++;

            base.OnBevStart();
        }

        public override TaskStatus OnBevUpdate(float deltaTime)
        {
            base.OnBevUpdate(deltaTime);

            if (m_AlreadyHasCopyAI)
            {
                return TaskStatus.Success;
            }

            m_Player.InputMoveDir = PlayerInputManagerHelper.Inst.GetPlayerInputData(m_CopyedPlayerId).MoveDir;
            m_Player.InputRun = PlayerInputManagerHelper.Inst.GetPlayerInputData(m_CopyedPlayerId).Run;

            return TaskStatusByDuration;
        }

        public override void OnBevEnd()
        {
            LevelController.Inst.CopyPlayerInputAICount--;
            m_Player.InputMoveDir = Vector3.zero;
            m_Player.InputRun = false;

            base.OnBevEnd();
        }
    }
}