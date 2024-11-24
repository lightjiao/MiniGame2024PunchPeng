using Cysharp.Threading.Tasks;
using System.Threading;

namespace PunchPeng
{
    public class BuffAICountDownAttack : BuffIntervalAction
    {
        protected override void OnInit()
        {
            base.OnInit();
            m_CfgInterval = GameController.Inst.m_CurLevelCfg.CountdownAttackInterval;
        }

        protected override void InvervalTick()
        {
            AICountDownAttack(m_DestroyCts).Forget();
        }

        private async UniTask AICountDownAttack(CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested) return;

            // TODO: show UI and sfx
            await AudioManager.Inst.Play2DSfx(ConfigAuto.Config_Global.Inst.data.Sfx.CoundDown321Sfx);

            if (cts.IsCancellationRequested) return;

            foreach (var player in GameController.Inst.PlayerList)
            {
                if (!player.IsAI) continue;
                player.InputAttack = true;
            }
        }
    }
}