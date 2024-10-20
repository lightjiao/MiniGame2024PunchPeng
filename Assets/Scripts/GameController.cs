using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConfigAuto;

namespace PunchPeng
{
    public class GameController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Resources.LoadAsync(Config_Player.Inst.PlayerPrefab);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}