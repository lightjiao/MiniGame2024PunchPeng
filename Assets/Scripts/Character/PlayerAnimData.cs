using Animancer;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerAnimData : MonoBehaviour
    {
        public const float FadeTime = 0.1f;

        public LinearMixerTransitionAsset.UnShared LocomotionMixer;
        public ClipTransition Dead1;
        public ClipTransition Dead2;
        public ClipTransition HeadAttack;
        public ClipTransition PunchAttack;
        public ClipTransition BeHit;
    }
}