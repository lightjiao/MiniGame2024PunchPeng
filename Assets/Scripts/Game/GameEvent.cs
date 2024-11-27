using Cysharp.Threading.Tasks;
using System;

namespace PunchPeng
{
    public class GameEvent : Singleton<GameEvent>
    {
        protected override void OnInit()
        {
        }

        public Action LevelPreloadPost;
        public Action LevelStartPost;
        public Action LevelBooyahPost;
        public Action LevelEndPre;
        public Action<int, int> PlayerDeadPre;  // <killerId, deadPlayerId>
        public Action<int, int> PlayerDeadPost; // <killerId, deadPlayerId>
    }
}