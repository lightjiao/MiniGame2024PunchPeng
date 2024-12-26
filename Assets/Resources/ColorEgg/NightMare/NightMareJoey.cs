using Animancer;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace PunchPeng
{
    public class NightMareJoey : SingletonMono<NightMareJoey>
    {
        public float jumpHeightMin = 2f;
        public float jumpHeightMax = 3f;
        public float jumpDuration = 0.7f;

        public AnimationClip IdleAnim;
        public AnimationClip JumpAnim;

        private AnimancerComponent m_Animancer;

        protected override void OnAwake()
        {
            m_Animancer = GetComponent<AnimancerComponent>();
            Assert.IsNotNull(m_Animancer);
            m_Animancer.Play(IdleAnim);

            var show = MathUtil.InPercent(ConfigAuto.Config_Global.Inst.data.ColorEgg.NightMareJoeyPct);
            gameObject.SetActiveEx(show);
        }

        [Button("Jump")]
        public async UniTask Jump()
        {
            _ = m_Animancer.Play(JumpAnim);

            var startY = Position.y;
            await CachedTransform.DOMoveY(startY + Random.Range(jumpHeightMin, jumpHeightMax), jumpDuration / 2)
                .SetEase(Ease.OutQuad).AsyncWaitForCompletion();

            await CachedTransform.DOMoveY(startY, jumpDuration / 2)
                .SetEase(Ease.InQuad).AsyncWaitForCompletion();

            _ = m_Animancer.Play(IdleAnim);
        }
    }
}