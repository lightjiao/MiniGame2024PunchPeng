using Animancer;
using UnityEngine;

namespace PunchPeng
{
    public class PlayerAnimData : MonoBehaviour
    {
        public const float FadeTime = 0.1f;

        public LinearMixerTransitionAsset.UnShared LocomotionDefault;
        public LinearMixerTransitionAsset.UnShared Locomotion2;
        public LinearMixerTransitionAsset.UnShared Locomotion3;
        public LinearMixerTransitionAsset.UnShared LocomotionIce;
        public ClipTransition HeadAttack;
        public ClipTransition PunchAttack;
        public ClipTransition BeHit;
        public ClipTransition Dead;
        public ClipTransition Cheer;
        public ClipTransition AttackIdle;
    }
}