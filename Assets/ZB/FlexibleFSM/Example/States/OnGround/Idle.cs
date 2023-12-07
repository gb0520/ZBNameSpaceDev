using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class Idle : OnGround
    {
        protected override void OnEnter()
        {
            Debug.Log("Idle / OnEnter");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x * 0.1f, 0);
        }

        protected override void OnExit()
        {
            Debug.Log("Idle / OnExit");
        }

        public Idle(Rigidbody2D rigidbody) : base(rigidbody) { Comment = "Idle"; }
    }
}