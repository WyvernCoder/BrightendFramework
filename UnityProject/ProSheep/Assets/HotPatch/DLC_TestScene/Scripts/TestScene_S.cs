using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HotPatch.DLC_TestScene.Scripts
{
    public class TestScene_S : MonoBehaviour
    {
        private void Update()
        {
            var curPos = transform.position;
            curPos += new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f));
            transform.position = curPos;
        }
    }
}
