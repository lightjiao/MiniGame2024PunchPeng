using System;
using UnityEngine;

namespace PunchPeng
{
    public abstract class AbstractView
    {
        public static T Create<T>(GameObject go) where T : AbstractView
        {
            var instance = Activator.CreateInstance<T>();
            instance.Init(go);
            return instance;
        }

        protected abstract void Init(GameObject go);
    }

    public class BlankView : AbstractView
    {
        protected override void Init(GameObject go)
        {
        }
    }
}