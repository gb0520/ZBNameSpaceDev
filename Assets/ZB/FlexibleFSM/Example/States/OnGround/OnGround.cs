using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class OnGround : State
    {
        protected Rigidbody2D rigidbody;

        protected override void OnEnter()
        {
            Debug.Log("OnGround / OnEnter");
            rigidbody.gravityScale = 2;
        }

        protected override void OnExit()
        {
            Debug.Log("OnGround / OnExit");
        }

        public OnGround(Rigidbody2D rigidbody) : base("OnGround")
        {
            this.rigidbody = rigidbody;
        }
    }
}