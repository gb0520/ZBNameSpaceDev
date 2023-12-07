using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class InWater : State
    {
        Rigidbody2D rigidbody;

        protected override void OnEnter()
        {
            Debug.Log("InWater / OnEnter");
            rigidbody.gravityScale = 0.5f;
        }

        protected override void OnExit()
        {
            Debug.Log("InWater / OnExit");
        }

        public InWater(Rigidbody2D rigidbody) : base("InWater")
        {
            this.rigidbody = rigidbody;
        }
    }
}