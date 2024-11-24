using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PunchPeng
{
    public class AudioManager : SingletonMono<AudioManager>
    {
        [SerializeField] private AudioSourceHelper m_2DAudioSource;
        [SerializeField] private AudioSourceHelper m_3DAudioSource;

        private AudioSourceHelper m_BGMSource;

        protected override void OnAwake()
        {
            m_BGMSource = Instantiate(m_2DAudioSource, CachedTransform);
        }

        public async UniTask PlayBGM(string bgmRes)
        {
            var clip = await ResourceMgr.Inst.LoadAsync<AudioClip>(bgmRes);
            m_BGMSource.Play(clip, true, 0.1f);
        }

        public async UniTask Play2DSfx(string sfxRes, bool pauseBGM = false, float volum = 1)
        {
            var clip = await ResourceMgr.Inst.LoadAsync<AudioClip>(sfxRes);

            if (pauseBGM)
            {
                m_BGMSource.Pause();
            }

            var audioSource = Instantiate(m_2DAudioSource, CachedTransform);
            audioSource.Play(clip, false, volum);

            await UniTask.Delay(clip.length.ToMilliSec());
            GameObjectUtil.DestroyGo(audioSource.gameObject);

            if (pauseBGM)
            {
                m_BGMSource.Resume();
            }
        }

        public async UniTask Play3DSfx(string sfxRes, Vector3 pos, bool pauseBGM = false, float volum = 1)
        {
            var clip = await ResourceMgr.Inst.LoadAsync<AudioClip>(sfxRes);

            if (pauseBGM)
            {
                m_BGMSource.Pause();
            }

            var audioSource = Instantiate(m_3DAudioSource, CachedTransform);
            audioSource.transform.position = pos;
            audioSource.Play(clip, false, volum);

            await UniTask.Delay(clip.length.ToMilliSec());
            GameObjectUtil.DestroyGo(audioSource.gameObject);

            if (pauseBGM)
            {
                m_BGMSource.Resume();
            }
        }
    }
}