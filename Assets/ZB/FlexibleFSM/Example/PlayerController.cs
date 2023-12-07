using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB.HierarchialFSM;

namespace ZB
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Machine machine;
        [SerializeField] private HorizontalInput horizontal;

        Idle idle;
        Jump jump;
        Walk walk;
        InWater inWater;

        private void Awake()
        {
            //* FSM ����
            machine = new HierarchialFSM.Machine();

            //���� ����
            idle = new Idle(rigidbody);
            jump = new Jump(rigidbody);
            inWater = new InWater(rigidbody);
            walk = new Walk(rigidbody, horizontal, 500, 5);

            //machine �����߰�
            machine.AddState("idle", idle, true);
            machine.AddState("jump", jump);
            machine.AddState("walk", walk);

            //��ȯ���� �߰�
            machine.AddTransition("idle", "walk", () => horizontal.input != 0);
            machine.AddTransition("walk", "idle", () => horizontal.input == 0);
            machine.AddTransition("jump", "idle", () => Mathf.Abs(rigidbody.velocity.y) < 0.1f);
        }
        private void Update()
        {
            machine.Update();

            horizontal.input = (int)Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.X))
            {
                machine.SwapState("jump");
            }
        }

    }
    [System.Serializable]
    public class HorizontalInput
    {
        public int input;
    }
}