using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace PunchPeng
{
    public class BuffAICountDownAttack : BuffIntervalAction
    {
        protected override void OnBuffStart()
        {
            base.OnBuffStart();
            LevelController.Inst.DisableAIBevAttack++;
        }

        protected override void OnBuffEnd()
        {
            LevelController.Inst.DisableAIBevAttack--;
            base.OnBuffEnd();
        }

        protected override void OnIntervalTick()
        {
            AICountDownAttack(m_DestroyCts).Forget();
        }

        private async UniTask AICountDownAttack(CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested) return;

            UIController.Inst.PlayCoundDown321Anim().Forget();
            await AudioManager.Inst.Play2DSfx(ConfigAuto.Config_Global.Inst.data.Sfx.CoundDown321Sfx);

            if (cts.IsCancellationRequested) return;

            foreach (var player in LevelController.Inst.PlayerList)
            {
                if (!player.IsAI) continue;
                RandomDelayAttack(player, cts).Forget();
            }
        }

        private async UniTask RandomDelayAttack(Player player, CancellationTokenSource cts)
        {
            await UniTask.DelayFrame(Random.Range(0, 15));

            if (cts.IsCancellationRequested || player == null || player.IsDead)
            {
                return;
            }

            player.InputAttack = true;
        }
    }
}