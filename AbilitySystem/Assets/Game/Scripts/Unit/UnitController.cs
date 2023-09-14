// ----- C#
using System;
using System.Collections;
using System.Collections.Generic;

// ----- Unity
using UnityEngine;

// ----- User Defined
using InGame.ForUnit.Manage.ForUI;

namespace InGame.ForUnit.Manage
{
    public class UnitController : MonoBehaviour
    {
        // --------------------------------------------------
        // Components
        // --------------------------------------------------
        [Header("Joy Pad")]
        [SerializeField] private JoyPad _joyPad = null;

        [Header("Unit Collection")]
        [SerializeField] private Transform _unitCreateParents = null;

        // --------------------------------------------------
        // Variables
        // --------------------------------------------------
        // ----- Const
        private const float ROTATE_VALUE = 0.5f;

        // ----- Private
        private Unit  _targetUnit    = null;
        private float _unitMoveValue = 0.0f;

        // --------------------------------------------------
        // Property
        // --------------------------------------------------
        public Unit   TargetUnit => _targetUnit;
        public JoyPad JoyPad     => _joyPad;

        // --------------------------------------------------
        // Functions - Nomal
        // --------------------------------------------------
        // ----- Public
        public Unit CreateTargetUnit(Unit targetUnit, Transform spawnTrans = null)
        {
            if (_targetUnit != null)
            {
                Debug.LogError($"<color = red>[UnitController.CreateTargetUnit] 이미 PlayableUnit이 존재합니다.</color>");
                return null;
            }

            _targetUnit = Instantiate(targetUnit, _unitCreateParents);

            if (spawnTrans != null)
                _targetUnit.transform.position = spawnTrans.position;

            SetJoyPad();

            // Unit Animate 초기화
            _joyPad.onMouseDownEvent += () => { ChangeToUnitState(Unit.EState.Run);  };
            _joyPad.onMouseUpEvent   += () => { ChangeToUnitState(Unit.EState.Idle); };

            // Unit Ability 초기화

            return _targetUnit;
        }

        public void ChangeToUnitState(Unit.EState unitState, Action doneCallBack = null)
        {
            if (_targetUnit == null)
                return;

            _targetUnit.ChangeToUnitState(unitState, doneCallBack);
        }

        public void SetJoyPad()
        {
            if (_targetUnit == null)
            {
                Debug.LogError($"<color = red>[UnitController.SetJoyPad] Target Unit이 할당되지 않았습니다.</color>");
                return;
            }

            _joyPad.OnInit           (_targetUnit);
            _joyPad.UsedJoyStickEvent(true);

            UsedJoyPad(true);
        }

        public void ChangeUnitSpeed(float moveSpeed, float roatateSpeed = ROTATE_VALUE)
        {
            _unitMoveValue = moveSpeed;
            _joyPad.ChangeMoveFactors(moveSpeed, roatateSpeed);
        }

        public void UsedJoyPad(bool isOn)
        {
            _joyPad.UsedJoyStickEvent(isOn);

            if (!isOn) _joyPad.FrameRect.gameObject.SetActive(isOn);
        }
    }
}