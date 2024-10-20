using Animancer;
using UnityEngine;

namespace PunchPeng
{
    public class AnimationData : MonoBehaviour
    {
        public const float FadeTime = 0.1f;

        public LinearMixerTransitionAsset.UnShared LocomotionMixer;
        public ClipTransition LandRoll;
        public ClipTransition Dead1;
        public ClipTransition Dead2;
    }
}