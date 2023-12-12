using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class Jump2D<ID> : State<ID>
    {
        private Rigidbody2D rigidbody2D;
        private InputAssist<bool> jumpInput;
        private float jumpPower;
        private float jumpBreakMultiple;
        private bool jumpCanceled;

        protected override void EnterLogic()
        {
            rigidbody2D.AddForce(Vector2.up * jumpPower);
            jumpCanceled = false;
        }
        protected override void UpdateLogic()
        {
            Debug.Log(jumpInput.value);
            if (!jumpInput.value && !jumpCanceled)
            {
                Debug.Log("BREAK");
                jumpCanceled = true;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * jumpBreakMultiple);
            }
        }

        public Jump2D
            (ID name,
            Rigidbody2D rigidbody2D,
            InputAssist<bool> jumpInput,
            float jumpPower = 50,
            float jumpBreakMultiple = 0.25f,
            UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction)
        {
            this.rigidbody2D = rigidbody2D;
            this.jumpInput = jumpInput;
            this.jumpPower = jumpPower;
            this.jumpBreakMultiple = jumpBreakMultiple;
        }
    }
}