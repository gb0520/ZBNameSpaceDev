using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.HierarchialFSM
{
    public class Walk : OnGround
    {
        float power;
        float maxSpeed;
        HorizontalInput horizontalInput;

        protected override void OnUpdate()
        {
            Debug.Log($"HORIZONTAL {horizontalInput.input}");
            rigidbody.AddForce(new Vector2(horizontalInput.input * power, rigidbody.velocity.y));
            rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxSpeed, maxSpeed), rigidbody.velocity.y);
        }

        public Walk(Rigidbody2D rigidbody, HorizontalInput horizontalInput, float power , float maxSpeed) : base(rigidbody)
        {
            this.horizontalInput = horizontalInput;
        }
    }
}