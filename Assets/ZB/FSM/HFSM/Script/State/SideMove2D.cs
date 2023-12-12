using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.FSM.ObjectHFSM
{
    public class SideMove2D<ID> : State<ID>
    {
        private Rigidbody2D rigidbody2D;
        private InputAssist<int> sideInput;
        private float movePower;
        private float maxMoveSpeed;
        private float stopBreakMultiple;

        protected override void UpdateLogic()
        {
            if (sideInput.value == 0)
            {
                rigidbody2D.velocity =
                    new Vector2(rigidbody2D.velocity.x * stopBreakMultiple, rigidbody2D.velocity.y);
            }
            else
            {
                rigidbody2D.AddForce(Vector2.right * movePower * sideInput.value);
            }

            rigidbody2D.velocity =
                new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxMoveSpeed, maxMoveSpeed), rigidbody2D.velocity.y);

            //Debug.Log(sideInput.value);
        }

        public SideMove2D
            (ID name,
            Rigidbody2D rigidbody2D,
            InputAssist<int> sideInput,
            float movePower = 100,
            float maxMoveSpeed = 5,
            float stopBreakMultiple = 0.25f,
            UnityAction enterAction = null, UnityAction exitAction = null, UnityAction stayAction = null) :
            base(name, enterAction, exitAction, stayAction)
        {
            this.rigidbody2D = rigidbody2D;
            this.sideInput = sideInput;
            this.movePower = movePower;
            this.maxMoveSpeed = maxMoveSpeed;
        }
    }
}