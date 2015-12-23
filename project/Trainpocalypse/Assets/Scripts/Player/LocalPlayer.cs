﻿using Funk.Data;
using UnityEngine;

namespace Funk.Player
{
    public class LocalPlayer : IPlayer
    {
        private const string PREFIX_AXIS_HORIZONTAL = "Horizontal";
        private const string PREFIX_AXIS_VERTICAL = "Vertical";
        private const string PREFIX_DPAD_UP = "DpadUp";
        private const string PREFIX_DPAD_DOWN = "DpadDown";
        private const string PREFIX_DPAD_LEFT = "DpadLeft";
        private const string PREFIX_DPAD_RIGHT = "DpadRight";

        private string _name;
        private string _inputSuffix;
        private PlayerAction _noAction;
        private PlayerAction _moveU;
        private PlayerAction _moveD;
        private PlayerAction _moveL;
        private PlayerAction _moveR;

        private float _axisH;
        private float _axisV;
        private bool _dpadU;
        private bool _dpadD;
        private bool _dpadL;
        private bool _dpadR;

        private PlayerAction _previousAction;

        public LocalPlayer(PlayerData playerData)
        {
            _name = playerData.Name;
            _inputSuffix = ((byte)playerData.InputMap).ToString();
            _noAction = new PlayerAction { PlayerName = _name, Type = ActionType.None };
            _moveU = new PlayerAction { PlayerName = _name, Type = ActionType.MoveUp };
            _moveD = new PlayerAction { PlayerName = _name, Type = ActionType.MoveDown };
            _moveL = new PlayerAction { PlayerName = _name, Type = ActionType.MoveLeft };
            _moveR = new PlayerAction { PlayerName = _name, Type = ActionType.MoveRight };
            _previousAction = _noAction;
        }

        public PlayerAction GetAction()
        {
            _axisH = Input.GetAxis(PREFIX_AXIS_HORIZONTAL + _inputSuffix);
            _axisV = Input.GetAxis(PREFIX_AXIS_VERTICAL + _inputSuffix);
            _dpadU = false; //Input.GetButtonDown(PREFIX_DPAD_UP + _inputSuffix);
            _dpadD = false; //nput.GetButtonDown(PREFIX_DPAD_DOWN + _inputSuffix);
            _dpadL = false; //nput.GetButtonDown(PREFIX_DPAD_LEFT + _inputSuffix);
            _dpadR = false; //nput.GetButtonDown(PREFIX_DPAD_RIGHT + _inputSuffix);

            if ((_axisH < 0 || _dpadL) && (!_previousAction.Equals(_moveL)))
            {
                _previousAction = _moveL;
                return _moveL;
            }

            if ((_axisH > 0 || _dpadR) && (!_previousAction.Equals(_moveR)))
            {
                _previousAction = _moveR;
                return _moveR;
            }

            if ((_axisV < 0 || _dpadU) && (!_previousAction.Equals(_moveU)))
            {
                _previousAction = _moveU;
                return _moveU;
            }

            if ((_axisV > 0 || _dpadD) && (!_previousAction.Equals(_moveD)))
            {
                _previousAction = _moveD;
                return _moveD;
            }

            return _noAction;
        }
    }
}