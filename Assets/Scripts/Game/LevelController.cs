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
        // 金币列表
        public Dictionary<int, int> PlayerCoinScores = new();
        public List<int> Thief = new();
        private int pickCount = 9;
        
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

            CurLevelCfg = Config_Global.Inst.data.LevelCfg[GameController.Inst.CurLevel];
            await LevelManager.Inst.LoadLevelAsync(CurLevelCfg.Scene);

            GameEvent.Inst.LevelLoadPostAction?.Invoke();
        }

        public async UniTask LevelStart()
        {
            IsBooyah = false;

            var playBGM = AudioManager.Inst.PlayBGM(CurLevelCfg.BGMRes);
            await playBGM;
            await SpawnPlayersAsync();
            
            if (CurLevelCfg.Scene != "PunchPeng_Caodi")
            {
                UIController.Inst.player1CollectScore.gameObject.SetActiveEx(false);
                UIController.Inst.player2CollectScore.gameObject.SetActiveEx(false);
            }
            else
            {
                UIController.Inst.player1CollectScore.gameObject.SetActiveEx(true);
                UIController.Inst.player2CollectScore.gameObject.SetActiveEx(true);
            }

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
            await LevelManager.Inst.UnLoadCurLevel();
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
                //for (int i = 0; i < 2; i++)
                {
                    var player = await ResourceManager.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);
                    PlayerList.Add(player);
                }

                if (CurLevelCfg.Scene == "PunchPeng_Caodi")
                {
                    for (int i = 0; i < 2; i++)
                    {
                        PlayerCoinScores[i + 1] = 0;
                    }
                    for (int i = 0; i < pickCount; i++)
                    {
                        Thief.Add(0 - i - 1);
                    }
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

            if (CurLevelCfg.Scene == "PunchPeng_Caodi")
            {
                foreach (var p in PlayerList)
                {
                    foreach (var thief in Thief)
                    {
                        if (p.PlayerId == thief)
                        {
                            Transform childTransform = p.gameObject.transform.GetChild(0).GetChild(0);
                            if (childTransform != null)
                            {
                                var mesh = childTransform.GetComponent<SkinnedMeshRenderer>();
                                Color newColor = new Color(1f, 0.4196f, 0f);
                                Material mat = mesh.material;
                                mat.SetColor("_Color", newColor);
                            }
                        }
                    }
                }
            }
        }

        private void PlayerDeadToBooyah(int killer, int deadPlayer)
        {
            var collectorWinnerID = 0;
            var isCaodi = false;
            if (deadPlayer <= 0)
            {
                if (CurLevelCfg.Scene == "PunchPeng_Caodi")
                {
                    isCaodi = true;
                    collectorWinnerID = PlayerCollectCoin(killer);
                }
            }

            if (IsBooyah) return;
            if (isCaodi && deadPlayer <= 0 && collectorWinnerID == 0)
            {
                return;
            }

            if (!isCaodi && deadPlayer <= 0)
            {
                return;
            }
            IsBooyah = true;

            Player winPlayer = null;
            if (isCaodi)
            {
                if (collectorWinnerID == m_Player1.PlayerId || deadPlayer == m_Player2.PlayerId)
                {
                    winPlayer = m_Player1;
                }
                if (collectorWinnerID == m_Player2.PlayerId || deadPlayer == m_Player1.PlayerId)
                {
                    winPlayer = m_Player2;
                }
            }
            else
            {
                if (deadPlayer == m_Player1.PlayerId)
                {
                    winPlayer = m_Player2;
                }
                if (deadPlayer == m_Player2.PlayerId)
                {
                    winPlayer = m_Player1;
                }
            }
            


            BuffContainer.RemoveAllBuff();
            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.Vfx.WinnerVfx, winPlayer.Position, 10).Forget();
            AudioManager.Inst.Play2DSfx(Config_Global.Inst.data.Sfx.WinSfx, true, 0.1f).Forget();

            GameEvent.Inst.LevelBooyahPostAction?.Invoke();

            WaitToFinishLevel(winPlayer).Forget();
        }
        private int PlayerCollectCoin(int collector)
        {

            if (collector <= 0) return 0;

            PlayerCoinScores[collector]++;
            GameEvent.Inst.PlayerCollectCoinPostAction?.Invoke(collector);

            if (PlayerCoinScores != null && PlayerCoinScores[collector] < 5) return 0;

            return collector;
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