using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mryildirim.Utilities;
using UnityEngine;

namespace Mryildirim.CoreGame
{
    public class RotateTiles : MonoBehaviour
    {
        private int _rotationCount;
        private bool _stopRotation;
        private bool _canRotateAgain  = true;
        private Vector2 _startPosition, _endPosition;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private Camera _camera;

        private void OnEnable()
        {
            EventManager.OnTileMatch += StopRotation;
        }

        private void OnDisable()
        {
            EventManager.OnTileMatch -= StopRotation;
        }

        void Update()
        {
            StartRotation();
        }

        private void StartRotation()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(Tiles.SelectedTiles.Count <= 0) return;
                
                _endPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                
                if (Vector2.Distance(_startPosition, _endPosition) < 0.5f || !_canRotateAgain) return;

                _stopRotation = false;

                StartCoroutine(Rotate());
            }
        }
        
        private IEnumerator Rotate()
        {
            yield return new WaitForSeconds(_rotationDuration);
            
            transform.DORotate(CalculateRotationDirection(), _rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (_rotationCount < 2)
                    {
                        if (!_stopRotation)
                        {
                            _rotationCount++;
                            StartCoroutine(Rotate());
                        }
                        else
                        {
                            _canRotateAgain = true;
                            Outline outline = GetComponent<Outline>();
                            outline.ToggleOutlines(false);
                        }
                    }
                    else
                    {
                        _rotationCount = 0;
                        _canRotateAgain = true;
                    }
                });
            
            _canRotateAgain = false;
        }
        
        private int _rotationDirection;
        private Vector3 CalculateRotationDirection()
        {
            if (_canRotateAgain)
            {
                if (_startPosition.x > _endPosition.x)
                    _rotationDirection = 1;
                else
                    _rotationDirection = -1; 
            }
            
            const float rotationAngle = 120;
            return transform.rotation.eulerAngles + new Vector3(0,0, _rotationDirection * rotationAngle);
        }

        private void StopRotation()
        {
            _stopRotation = true;
        }
    }
}
