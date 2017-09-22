﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour {

    private IPlayer _player;
    private Rigidbody2D _rb;
    private GameObject _parentRope;
    [HideInInspector]
    public int index;

    void Awake()
    {
        _player = FindObjectOfType<IPlayer>();
        _rb = GetComponent<Rigidbody2D>();
        _parentRope = transform.parent.gameObject;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_player.onRope == null && _player.onLadder == false)
        {
            if (other.gameObject == _player.gameObject)
            {
                _player.onRope = gameObject;
                _player.transform.parent = transform;
                _player.GetComponent<Collider2D>().isTrigger = true;
                RelativeJoint2D _playerHJ = _player.gameObject.AddComponent<RelativeJoint2D>();
                _playerHJ.enableCollision = false;
                _playerHJ.maxForce = 200;
                _playerHJ.maxTorque = 200;
                _playerHJ.autoConfigureOffset = false;
                _playerHJ.linearOffset = new Vector2(0.2f, 0f);
                _playerHJ.angularOffset = 0;
                _playerHJ.connectedBody = _rb;
            }
        }
    }

    public void ExitRope()
    {
        if (_player.onRope == gameObject)
        {
            _player.onRope = null;
            _player.transform.parent = null;
            _player.GetComponent<Collider2D>().isTrigger = false;
            _player.GetComponent<Rigidbody2D>().simulated = true;
            _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            print(_rb.velocity);
            if (_rb.velocity.x > 0)
                _player.GetComponent<Rigidbody2D>().AddForce(_rb.velocity * 1.3f + new Vector2(1f ,2f), ForceMode2D.Impulse);
            else
                _player.GetComponent<Rigidbody2D>().AddForce(_rb.velocity * 1.3f + new Vector2(-1f, 2f), ForceMode2D.Impulse);

            Destroy(_player.GetComponent<Joint2D>());
        }
    }

    public void TogglePlayerCollision(bool enable)
    {
		if (_player.GetComponent<Collider2D>())
		{
				if (!enable)
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _player.GetComponent<Collider2D>());
				else
					Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _player.GetComponent<Collider2D>(), false);
		}
    }
}
