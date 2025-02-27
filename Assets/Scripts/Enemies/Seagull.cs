using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Examples.Common;


[RequireComponent(typeof(Rigidbody))]

public class Seagull : MonoBehaviour
{
    [SerializeField]
    public float _moveTime = 3.0f;
    float previousT = 0f;   // um Peaks zu ermitteln, um Seagull zu flippen
    bool flipped;

    [SerializeField]
    private Vector3 _offset;

    private Rigidbody _rigidbody;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;


    public float moveTime
    {
        get { return _moveTime; }
        set { _moveTime = Mathf.Max(1.0f, value); }
    }

    public Vector3 offset
    {
        get { return _offset; }
        set { _offset = value; }
    }
    private void FlipSeagull()
    {
        transform.Rotate(Vector3.up, 180f);
    }

    #region MONOBEHAVIOUR

    public void OnValidate()
    {
        moveTime = _moveTime;
    }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;

        _startPosition = transform.position;
        _targetPosition = _startPosition + offset;
    }

    public void FixedUpdate()
    {
        var t = Utils.EaseInOut(Mathf.PingPong(Time.time, _moveTime), _moveTime);
        var p = Vector3.Lerp(_startPosition, _targetPosition, t);

        if (!flipped && t < previousT)
        {
            FlipSeagull();
            flipped = true;
        }
        if (flipped && t > previousT)
        {
            FlipSeagull();
            flipped = false;
        }

        _rigidbody.MovePosition(p);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawCube(_startPosition + offset, new Vector3(3, 3, 3));
        }
        else
        {
            Gizmos.DrawCube(transform.position + offset, new Vector3(3, 3, 3));
        }

    }
}
