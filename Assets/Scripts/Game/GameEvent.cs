using Cysharp.Threading.Tasks;
using System;

namespace PunchPeng
{
    public class GameEvent : Singleton<GameEvent>
    {
        protected override void OnInit()
        {
        }

        public Action LevelLoadPostAction;
        public Action LevelStartPostAction;
        public Action LevelBooyahPostAction;
        public Action LevelEndPreAction;
        public Action<int, int> PlayerDeadPreAction;  // <killerId, deadPlayerId>
        public Action<int, int> PlayerDeadPostAction; // <killerId, deadPlayerId>
    }
}