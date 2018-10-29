using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class FixedState : IState<Inputs>
    {
        Dictionary<Inputs, IState<Inputs>> _transitions;

        float _targetX;
        public float targetX { set { _targetX = value; } }

        float _targetY;
        public float targetY { set{ _targetY = value; } }

        float _currentX;
        float _currentY;
        Transform transform;
        Transform _target;

        float _xRotationSpeed;
        float _currentXRotationSpeed;
        float _yRotationSpeed;
        float _currentYRotationSpeed;

        float _targetDistance;
        float _positionSmoothness;


        float _distance;

        public float targetDistance { set { _targetDistance = value; } }

        public FixedState(Transform t, float xRotationSpeed,float yRotationSpeed ,Transform target)
        {
            transform = t;
            _xRotationSpeed = xRotationSpeed;
            _yRotationSpeed = yRotationSpeed;
            _target = target;
        }

        public void Enter()
        {
            _currentX = transform.eulerAngles.y;
            _currentY = transform.eulerAngles.x;
            _currentXRotationSpeed = _targetX - _currentX < 0 ? -_xRotationSpeed : _xRotationSpeed;
            //_currentYRotationSpeed = _targetY - _currentY < 0 ? -_yRotationSpeed : _yRotationSpeed;
            _currentYRotationSpeed = _yRotationSpeed * Mathf.Pow(_targetY - _currentY,2)/35f;

            _distance = Vector3.Distance(transform.position, _target.position);
            _positionSmoothness = 0.01f;
            //Debug.Log(_currentX);
        }

        public void Execute()
        {
            /*if(Mathf.Abs(_targetX - _currentX) < _xRotationSpeed * Time.deltaTime * 10)
            {
                _currentX = _targetX;
                _positionSmoothness = _positionSmoothness < 1? _positionSmoothness + Time.deltaTime: 1;
            }
            else
            {
                if (_currentX > 360) _currentX -= 360;
                _currentX += _currentXRotationSpeed * Time.deltaTime;
                
            }
            Debug.Log("Diference: " + Mathf.Abs(_targetY - _currentY) + "Comparation: " + _yRotationSpeed * Time.deltaTime * 2);
            if (Mathf.Abs(_targetY - _currentY) < _yRotationSpeed * Time.deltaTime * 2)
            {
                _currentY = _targetY;
            }
            else
            {
                if (_currentY > 360) _currentY -= 360;
                _currentY += _currentYRotationSpeed * Time.deltaTime;
            }

            _distance = Mathf.Lerp(_distance, _targetDistance, Time.deltaTime);

            var dir = new Vector3(0, 0, -_distance);
            Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
            var targetPosition = _target.position + rotation * dir;

            transform.position = Vector3.Lerp(transform.position, targetPosition, _positionSmoothness);
            transform.LookAt(_target);*/
            var d = _positionSmoothness < 0.3f ? 5 : 1;
            _positionSmoothness = _positionSmoothness < 1 ? _positionSmoothness + Time.deltaTime/d : 1;
            //Debug.Log(_positionSmoothness);

            var dir = new Vector3(0, 0, -_targetDistance);
            var rotation = Quaternion.Euler(_targetY, _targetX, 0);
            var targetPosition = _target.position + rotation * dir;

            transform.position = Vector3.Lerp(transform.position, targetPosition, _positionSmoothness);

            transform.LookAt(_target);
            if(Mathf.Abs(Vector3.Distance(transform.position, targetPosition)) < 0.5f)
            {
                EventManager.DispatchEvent(GameEvent.CAMERA_FIXPOS_END);
            }
        }

        public void Exit()
        {
            
        }

        public Dictionary<Inputs, IState<Inputs>> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }
    }

}
