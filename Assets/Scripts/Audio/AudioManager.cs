using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class AudioManager : SingletonMono<AudioManager>
    {
        [SerializeField] private AudioSourceHelper m_2DAudioSource;
        [SerializeField] private AudioSourceHelper m_3DAudioSource;

        private AudioSourceHelper m_BGMSource;
        private Dictionary<string, int> m_SfxCnt = new();

        protected override void OnAwake()
        {
            m_BGMSource = Instantiate(m_2DAudioSource, CachedTransform);
            // 写个简单的池子，避免游戏中卡顿
        }

        public async UniTask PlayBGM(string bgmRes)
        {
            if (m_BGMSource == null)
            {
                m_BGMSource = Instantiate(m_2DAudioSource, CachedTransform);
            }

            var clip = await ResourceManager.Inst.LoadAsync<AudioClip>(bgmRes);
            m_BGMSource.Play(clip, true, 0.1f);
        }

        public void StopBgm()
        {
            if (m_BGMSource != null)
            {
                m_BGMSource.Stop();
                Destroy(m_BGMSource.gameObject);
            }

            m_BGMSource = null;
        }

        public void PauseBGM()
        {
            if (m_BGMSource != null)
            {
                m_BGMSource.Pause();
            }
        }

        public void ResumeBGM()
        {
            if (m_BGMSource != null)
            {
                m_BGMSource.Resume();
            }
        }

        public async UniTask Play2DSfx(string sfxRes, bool pauseBGM = false, float volum = 1)
        {
            if (!CanPlaySfx(sfxRes)) return;
            AddCount(sfxRes);

            var clip = await ResourceManager.Inst.LoadAsync<AudioClip>(sfxRes);

            if (pauseBGM) PauseBGM();

            var audioSource = Instantiate(m_2DAudioSource, CachedTransform);
            audioSource.Play(clip, false, volum);

            await UniTask.Delay(clip.length.ToMilliSec());
            GameObjectUtil.DestroyGo(audioSource.gameObject);

            if (pauseBGM) ResumeBGM();

            RemoveCount(sfxRes);
        }

        public async UniTask Play3DSfx(string sfxRes, Vector3 pos, bool pauseBGM = false, float volum = 1)
        {
            if (!CanPlaySfx(sfxRes)) return;
            AddCount(sfxRes);

            var clip = await ResourceManager.Inst.LoadAsync<AudioClip>(sfxRes);

            if (pauseBGM) PauseBGM();

            var audioSource = Instantiate(m_3DAudioSource, CachedTransform);
            audioSource.transform.position = pos;
            audioSource.Play(clip, false, volum);

            await UniTask.Delay(clip.length.ToMilliSec());
            GameObjectUtil.DestroyGo(audioSource.gameObject);

            if (pauseBGM) ResumeBGM();

            RemoveCount(sfxRes);
        }

        private bool CanPlaySfx(string sfx)
        {
            m_SfxCnt.TryGetValue(sfx, out var count);
            return count < 10;
        }

        private void RemoveCount(string sfx)
        {
            if (!m_SfxCnt.ContainsKey(sfx))
            {
                return;
            }
            m_SfxCnt[sfx]--;
        }

        private void AddCount(string sfx)
        {
            if (!m_SfxCnt.ContainsKey(sfx))
            {
                m_SfxCnt[sfx] = 0;
            }
            m_SfxCnt[sfx]++;
        }
    }
}