using ConfigAuto;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PunchPeng
{
    public class AudioManager : SingletonMono<AudioManager>
    {
        [SerializeField]
        private AudioSource m_BGMSource;

        protected override void OnAwake()
        {
            m_BGMSource = GetComponent<AudioSource>();
            Assert.IsNotNull(m_BGMSource);
        }

        public async UniTask PlayLevelBGM(string levelName)
        {
            var bgmRes = Config_Global.Inst.data.LevelConfig.GetValueOrDefault(levelName)?.BGMRes;
            await PlayBGM(bgmRes);
        }

        public async UniTask PlayBGM(string res)
        {
            var clip = await ResourceMgr.Inst.LoadAsync<AudioClip>(res);
            m_BGMSource.clip = clip;
            m_BGMSource.loop = true;
            m_BGMSource.Play();
            m_BGMSource.volume = 0.1f;
        }

        public async UniTask PauseBGMPlaySfx(string res)
        {
            var clip = await ResourceMgr.Inst.LoadAsync<AudioClip>(res);
            var oldClip = m_BGMSource.clip;
            m_BGMSource.clip = clip;
            m_BGMSource.loop = false;
            m_BGMSource.Play();

            await UniTask.Delay(3f.ToMilliSec());

            m_BGMSource.clip = oldClip;
            m_BGMSource.loop = true;
            m_BGMSource.Play();
        }

        // TODO: play audio clip by pooled audio source
    }
}