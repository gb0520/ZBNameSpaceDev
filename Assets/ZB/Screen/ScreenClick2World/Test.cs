using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Screen
{
    public class Test : MonoBehaviour
    {
        [SerializeField] ScreenRay2World ray2World;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click!");
                ray2World.RayShotByMouse();
            }
        }
    }
}