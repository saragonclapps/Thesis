using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dardo
{
    public class DardoMovement{

        //Config
        Transform _target;
        Transform transform;
        float _yVariation;
        Vector3 _originalPosition;
        float _atackSpeed;

        //range 0- 1
        float _chargeSpeed;

        //Move
        Vector3 targetPosition;
        bool isGoingUp;
        float _currentYRotation;
        float _rotationSpeed;

        //FairyMovement timmer
        float _fairyTimmer = 1;
        float _chaseTimmer = 0.5f;
        float _tick;

        public DardoMovement(Transform dardoTransform, float yVariation, float rotationSpeed, float chargeSpeed, float atackSpeed)
        {
            _yVariation = yVariation;
            transform = dardoTransform;
            _rotationSpeed = rotationSpeed;
            _chargeSpeed = chargeSpeed;
            _atackSpeed = atackSpeed;
        }

        /// <summary>
        /// Up and down Movement
        /// </summary>
        /// <param name="yShakeSpeed">Range 0 - 1</param>
	    public void FairyMovement(float yShakeSpeed, bool randomXZ)
        {
            if (Mathf.Abs(targetPosition.y - transform.position.y) < 0.1f)
            {
                targetPosition.y = targetPosition.y > transform.position.y ? targetPosition.y - _yVariation : targetPosition.y + _yVariation;
                if (randomXZ)
                {
                    targetPosition.x = _originalPosition.x + Random.Range(-2, 2);
                    targetPosition.z = _originalPosition.z + Random.Range(-2, 2); 
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, yShakeSpeed);
            }
            var dir = targetPosition - transform.position;
            var dirWithoutY = new Vector3(dir.x, 0, dir.z);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirWithoutY), Time.deltaTime);
        }

        /// <summary>
        /// Move Every second
        /// </summary>
        /// <param name="distanceToTravel"></param>
        public void ChaseMovement(float distanceToTravel)
        {
            if(_tick > _fairyTimmer)
            {
                _tick = 0;
                var rawDir = _target.position - transform.position;
                var randomizedDir = rawDir + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));

                targetPosition += randomizedDir.normalized * distanceToTravel;
            }
            else
            {
                _tick += Time.deltaTime;
            }
        }

        public void ChargeMoveInit()
        {
            targetPosition = transform.position;
            targetPosition.y = _originalPosition.y;
        }

        /// <summary>
        /// Charge Movment (circles)
        /// </summary>
        /// <param name="distance">distance to target</param>
        public void ChargeMove(float distance, float speed)
        {
            //Rect Move
            var rawMoveDir = (_target.position - transform.position).normalized;
            var moveDir = new Vector3(rawMoveDir.x, 0, rawMoveDir.z);
            targetPosition += moveDir * speed * Time.deltaTime;
            
            //Circular Move
            var rawDir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(0, _currentYRotation, 0);
            var rawTargetPos = targetPosition + rotation * rawDir;

            transform.position = Vector3.Lerp(transform.position,rawTargetPos, _chargeSpeed);
            _currentYRotation += _rotationSpeed * Time.deltaTime;
            transform.LookAt(_target);
        }

        /// <summary>
        /// Returning Movement
        /// </summary>
        /// <param name="distanceToTravel"></param>
        public void ReturningMove(float distanceToTravel)
        {
            if (_tick > _chaseTimmer)
            {
                _tick = 0;
                var rawDir = _originalPosition - transform.position;
                var randomizedDir = rawDir + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));

                targetPosition += randomizedDir.normalized * distanceToTravel;
            }
            else
            {
                _tick += Time.deltaTime;
            }
        }

        /// <summary>
        /// Move to atack
        /// </summary>
        public void AtackMove()
        {
            var dir = (_target.position - transform.position).normalized;
            transform.position += dir * _atackSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Set Target
        /// </summary>
        /// <param name="tr">Target Player</param>
        public void Configure(Transform tr, Vector3 originalPosition)
        {
            _target = tr;
            _originalPosition = originalPosition;
            targetPosition = originalPosition;
        }

    }

}
