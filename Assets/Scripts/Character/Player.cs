using Animancer;
using PunchPeng;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private CharacterController m_CCT;

        [SerializeField]
        private AnimancerComponent m_Animancer;

        private AnimationData m_AnimData;
    }
}