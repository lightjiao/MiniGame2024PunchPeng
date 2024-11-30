using ConfigAuto;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PunchPeng
{
    public class LevelController : SingletonMono<LevelController>, IBuffOwner
    {
        // 包含了AI与玩家的列表
        [HideInInspector] public List<Player> PlayerList = new();
        [ReadOnly] public Player m_Player1;
        [ReadOnly] public Player m_Player2;
        [ReadOnly] public int CopyPlayerInputAICount;
        [ReadOnly] public IntAsBool DisableAIBevAttack;
        [ReadOnly][ShowInInspector] public bool IsBooyah { get; private set; }

        public BuffContainer BuffContainer { get; private set; }
        public Config_Global.LevelCfg CurLevelCfg { get; private set; }

        protected override void OnAwake()
        {
            Inst = this;
            Application.targetFrameRate = Config_Global.Inst.data.TargetFrameRate;
            GameEvent.Inst.PlayerDeadPostAction += PlayerDeadToBooyah;
            _ = ScoreboardManager.Inst;
            BuffContainer = new BuffContainer(this);
        }

        private void Update()
        {
            BuffContainer.Update(Time.deltaTime);
            VfxManager.Inst.OnUpdate(Time.deltaTime);
        }

        public async UniTask LevelLoad()
        {
            VfxManager.Inst.ReleaseAll();

            CurLevelCfg = Config_Global.Inst.data.LevelCfg[GameFlowController.Inst.CurLevel];
            await LevelMgr.Inst.LoadLevelAsync(CurLevelCfg.Scene);

            GameEvent.Inst.LevelLoadPostAction?.Invoke();
        }

        public async UniTask LevelStart()
        {
            IsBooyah = false;

            var playBGM = AudioManager.Inst.PlayBGM(CurLevelCfg.BGMRes);
            await playBGM;
            await SpawnPlayersAsync();

            foreach (var buffId in CurLevelCfg.LevelBuffs ?? CollectionUtil.EmptyListInt)
            {
                BuffContainer.AddBuff(buffId);
            }

            foreach (var player in PlayerList)
            {
                foreach (var buffId in Config_Global.Inst.data.EveryLevelPlayerBuffs ?? CollectionUtil.EmptyListInt)
                {
                    player.BuffContainer.AddBuff(buffId);
                }

                foreach (var buffId in CurLevelCfg.PlayerBuffs ?? CollectionUtil.EmptyListInt)
                {
                    player.BuffContainer.AddBuff(buffId);
                }
            }
        }

        private async UniTask LevelEnd()
        {
            GameEvent.Inst.LevelEndPreAction?.Invoke();
            AudioManager.Inst.StopBgm();

            m_Player1 = null;
            m_Player2 = null;
            foreach (var item in PlayerList)
            {
                GameObjectUtil.DestroyGo(item.gameObject);
            }
            PlayerList.Clear();
            await LevelMgr.Inst.UnLoadCurLevel();
        }

        private async UniTask SpawnPlayersAsync()
        {
            foreach (var player in PlayerList)
            {
                Destroy(player.gameObject);
            }
            PlayerList.Clear();

            //var testOneAI = true;
            //if (testOneAI)
            //{
            //    for (int i = 0; i < 1; i++)
            //    {
            //        var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);
            //        PlayerList.Add(player);
            //    }
            //}
            //else
            {
                for (int i = 0; i < CurLevelCfg.PawnCount; i++)
                //for (int i = 0; i < 3; i++)
                {
                    var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);
                    PlayerList.Add(player);
                }

                var protectedLoopCnt = 100;
                m_Player1 = PlayerList.RandomOne();
                do
                {
                    protectedLoopCnt--;
                    m_Player2 = PlayerList.RandomOne();
                } while (m_Player1 == m_Player2 && protectedLoopCnt > 0);

                if (protectedLoopCnt <= 0)
                {
                    Log.Error("初始化玩家失败，死循环了");
                }

                m_Player1.PlayerId = 1;
                m_Player2.PlayerId = 2;
            }

            var aiId = -1;
            foreach (var player in PlayerList)
            {
                player.Position = Vector3Util.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
                player.Forward = Vector3Util.Rand2DDir();

                if (player != m_Player1 && player != m_Player2)
                {
                    player.PlayerId = aiId;
                    aiId--;
                    player.name += $" AI:[{aiId}]";
                    player.SetIsAI();
                }
                else
                {
                    player.name += $" Player:[{player.PlayerId}]";
                }
            }
        }

        private void PlayerDeadToBooyah(int killer, int deadPlayer)
        {
            if (deadPlayer <= 0) return;

            if (IsBooyah) return;
            IsBooyah = true;

            Player winPlayer = null;
            if (deadPlayer == m_Player1.PlayerId)
            {
                winPlayer = m_Player2;
            }
            if (deadPlayer == m_Player2.PlayerId)
            {
                winPlayer = m_Player1;
            }

            BuffContainer.RemoveAllBuff();
            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.Vfx.WinnerVfx, winPlayer.Position, 10).Forget();
            AudioManager.Inst.Play2DSfx(Config_Global.Inst.data.Sfx.WinSfx, true, 0.1f).Forget();

            GameEvent.Inst.LevelBooyahPostAction?.Invoke();

            WaitToFinishLevel(winPlayer).Forget();
        }

        private async UniTask WaitToFinishLevel(Player winPlayer)
        {
            // wait attack anim to finish
            await UniTask.Delay(0.5f.ToMilliSec());
            winPlayer.PlayAnim(winPlayer.m_AnimData.Cheer);

            await UniTask.Delay(5f.ToMilliSec());
            await LevelEnd();
        }
    }
}