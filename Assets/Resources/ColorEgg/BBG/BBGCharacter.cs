using Animancer;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace PunchPeng
{
    public class BBGCharacter : MonoEntity
    {
        public float moveDistance = 60;
        public float moveDuration = 3f;

        public AnimationClip runAnim;
        private AnimancerComponent m_Animancer;

        private void Awake()
        {
            m_Animancer = GetComponentInChildren<AnimancerComponent>();
            Assert.IsNotNull(m_Animancer);
        }

        private void Start()
        {
            var show = MathUtil.InPercent(ConfigAuto.Config_Global.Inst.data.ColorEgg.BBGPct);
            gameObject.SetActiveEx(show);

            _ = m_Animancer.Play(runAnim);
            LoopRun().Forget();
        }

        [Button("Run")]
        private async UniTask LoopRun()
        {
            while (true)
            {
                await UniTask.Delay(Random.Range(3, 5f).ToMilliSec());

                if (this.IsDestroyed()) return;

                CachedTransform.localPosition = Vector3.zero;
                var localX = CachedTransform.localPosition.x;
                await CachedTransform.DOLocalMoveX(localX - moveDistance, moveDuration)
                    .SetEase(Ease.Linear)
                    .AsyncWaitForCompletion();
            }
        }
    }
}