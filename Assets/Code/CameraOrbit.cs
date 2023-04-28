using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    [SerializeField] private float _lookSensitivity;
    [SerializeField] private float _minXLook;
    [SerializeField] private float _maxXLook;
    [SerializeField] private Transform _CameraAnchor;
    [SerializeField] private bool _invertXRotation;

    private float _currentXRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void CameraLook()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        transform.eulerAngles += Vector3.up * (x * _lookSensitivity);

        if (_invertXRotation)
        {
            _currentXRotation += y * _lookSensitivity;
        }
        else
        {
            _currentXRotation -= y * _lookSensitivity;
        }

        _currentXRotation = Mathf.Clamp(_currentXRotation, _minXLook, _maxXLook);

        var clampedAngle = _CameraAnchor.eulerAngles;
        clampedAngle.x = _currentXRotation;
        _CameraAnchor.eulerAngles = clampedAngle;
    }
}
