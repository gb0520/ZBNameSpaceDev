using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class Jump : OnGround
    {
        private float jumpPower = 100;

        protected override void OnEnter()
        {
            Debug.Log("Jump / OnEnter");
            rigidbody.AddForce(Vector2.up * jumpPower);
        }

        protected override void OnExit()
        {
            Debug.Log("Jump / OnExit");
        }

        public Jump(Rigidbody2D rigidbody) : base(rigidbody) { Comment = "Jump"; }
    }
}