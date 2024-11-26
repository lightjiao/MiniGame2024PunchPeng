using Cysharp.Threading.Tasks;
using System.Threading;

namespace PunchPeng
{
    public class BuffAICountDownAttack : BuffIntervalAction
    {
        protected override void OnBuffStart()
        {
            base.OnBuffStart();
            GameController.Inst.DisableAIBevAttack.RefCnt++;
        }

        protected override void OnBuffEnd()
        {
            GameController.Inst.DisableAIBevAttack.RefCnt--;
            base.OnBuffEnd();
        }

        protected override void InvervalTick()
        {
            AICountDownAttack(m_DestroyCts).Forget();
        }

        private async UniTask AICountDownAttack(CancellationTokenSource cts)
        {
            if (cts.IsCancellationRequested) return;

            // TODO: show UI and sfx
            UIController.Inst.PlayCoundDown321Anim().Forget();
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