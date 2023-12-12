using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.FSM.ObjectHFSM.Test
{
    public class TestPlayer : MonoBehaviour
    {
        public enum StateInfo
        {
            none,
            main,

            OnAir_SideMove,

            OnGround,
            OnGround_SideMove,
            OnGround_Jump,

            OnWater,
            OnWater_SideMove,
            OnWater_Jump,
        }

        public enum Enviroment
        {
            ground,
            air,
            water
        }

        [SerializeField] private ZBInput.UserInput userInput;
        [SerializeField] private Rigidbody2D rigidbody2D;

        [SerializeField] private StateInfo mainState;
        [SerializeField] private StateInfo onGroundState;
        [SerializeField] private StateInfo onWaterState;
        [SerializeField] private int move;

        [SerializeField] private Enviroment nowEnvironment;
        [SerializeField] private CollisionCheck.CheckLayer groundCheck;
        [SerializeField] private CollisionCheck.CheckLayer waterCheck;

        private Machine<StateInfo> mainMachine = new Machine<StateInfo>(StateInfo.main);
        private Machine<StateInfo> subMachine_onGround;
        private Machine<StateInfo> subMachine_onWater;

        private InputAssist<int> sideInput = new InputAssist<int>();
        private InputAssist<bool> jumpInput = new InputAssist<bool>();

        public void OnGroundVariation(bool isEnter)
        {
            //¶¥¿¡ ´êÀ» ¶§
            if (isEnter)
            {
                if (!waterCheck.Touching)
                {
                    nowEnvironment = Enviroment.ground;
                    mainMachine.SwapState(StateInfo.OnGround);
                }
            }

            //¶¥¿¡¼­ ¶³¾îÁú ¶§
            else
            {
                if (!waterCheck.Touching)
                {
                    nowEnvironment = Enviroment.air;
                    mainMachine.SwapState(StateInfo.OnAir_SideMove);
                }
            }
        }
        public void OnWaterVariation(bool isEnter)
        {
            //¹°¿¡ ´êÀ» ¶§
            if (isEnter)
            {
                nowEnvironment = Enviroment.water;
                mainMachine.SwapState(StateInfo.OnWater);
            }

            //¹°¿¡¼­ ¶³¾îÁú ¶§
            else
            {
                if (groundCheck.Touching)
                {
                    nowEnvironment = Enviroment.ground;
                    mainMachine.SwapState(StateInfo.OnGround);
                }
                else
                {
                    nowEnvironment = Enviroment.air;
                    mainMachine.SwapState(StateInfo.OnAir_SideMove);
                }
            }
        }

        private void Awake()
        {
            InitMachine();
            InitInput();
        }
        private void Update()
        {
            userInput.OnUpdate();
            mainMachine.OnUpdate();
        }
        private void InitMachine()
        {
            //* SubMachine-onGround
            subMachine_onGround = new Machine<StateInfo>(StateInfo.OnGround, () => mainState = StateInfo.OnGround);
            //AddState
            subMachine_onGround.AddState
            (
                true,
                //walk
                new SideMove2D<StateInfo>(
                    StateInfo.OnGround_SideMove,
                    rigidbody2D,
                    sideInput,
                    50, 3, 0.25f,
                    () => onGroundState = StateInfo.OnGround_SideMove),
                //jump
                new Jump2D<StateInfo>(
                    StateInfo.OnGround_Jump,
                    rigidbody2D,
                    jumpInput,
                    500, 0.25f,
                    () => onGroundState = StateInfo.OnGround_Jump
                    )
            );
            //AddTransition
            subMachine_onGround.AddTransition
            (
                //sideMove->Jump
                new Transition<StateInfo>
                (
                    StateInfo.OnGround_SideMove,
                    StateInfo.OnGround_Jump,
                    () => jumpInput.value
                ),
                //Jump->sideMove
                new Transition<StateInfo>
                (
                    StateInfo.OnGround_Jump,
                    StateInfo.OnGround_SideMove,
                    () => { return !jumpInput.value && nowEnvironment == Enviroment.ground; }
                )
            );

            //* SubMachine-onWater
            //AddState
            subMachine_onWater = new Machine<StateInfo>(StateInfo.OnWater, () => mainState = StateInfo.OnWater);
            subMachine_onWater.AddState
            (
                true,
                //walk
                new SideMove2D<StateInfo>(
                    StateInfo.OnWater_SideMove,
                    rigidbody2D,
                    sideInput,
                    5, 1, 0.75f,
                    () => onWaterState = StateInfo.OnWater_SideMove
                    ),
                //jump
                new Jump2D<StateInfo>(
                    StateInfo.OnWater_Jump,
                    rigidbody2D,
                    jumpInput,
                    50, 0.6f,
                    () => onWaterState = StateInfo.OnWater_Jump
                    )
            );
            //AddTransition
            subMachine_onWater.AddTransition
            (
                //sideMove->Jump
                new Transition<StateInfo>
                (
                    StateInfo.OnWater_SideMove,
                    StateInfo.OnWater_Jump,
                    () => jumpInput.value
                ),
                //Jump->sideMove
                new Transition<StateInfo>
                (
                    StateInfo.OnWater_Jump,
                    StateInfo.OnWater_SideMove,
                    () => true
                )
            );

            //* mainMachine
            //¤¤AddSubMachine
            mainMachine.AddSubMachine(subMachine_onGround, subMachine_onWater);
            //¤¤AddState
            //  ¤¤OnAir_SideMove
            mainMachine.AddState
            (
                true,
                new SideMove2D<StateInfo>
                (
                    StateInfo.OnAir_SideMove,
                    rigidbody2D,
                    sideInput,
                    30, 3, 1,
                    () => mainState = StateInfo.OnAir_SideMove
                )
            );

            subMachine_onGround.Active();
            subMachine_onWater.Active();
            mainMachine.Active();
        }
        private void InitInput()
        {
            userInput.GetKeyInput("jump").AddDownEvent(() => jumpInput.value = true);
            userInput.GetKeyInput("jump").AddUpEvent(() => jumpInput.value = false);
            userInput.GetDirection2DInput("side").AddVector2Delegate(MoveInputLogic);
        }
        private void MoveInputLogic(Vector2 vector2)
        {
            sideInput.value = (int)vector2.x;
            move = (int)vector2.x;
        }
    }
}