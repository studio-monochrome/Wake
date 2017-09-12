﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Move))]
[RequireComponent(typeof(WorldManager))]
[RequireComponent(typeof(CheckpointManager))]
[RequireComponent(typeof(CollectibleManager))]

public class Player : MonoBehaviour
{
    [HideInInspector]
    public GameObject onRope;

	[Range(0, 25)]
	public float respawnTime = 3.0f;
	private CheckpointManager _checkpointManager;
	private WorldManager _worldManager;
    private Move _moveScript;
    private Jump _jumpScript;
    private InputManager _input;

    private void Awake()
    {
		_checkpointManager = GetComponent<CheckpointManager> ();
		_worldManager = GetComponent<WorldManager> ();

        _moveScript = GetComponent<Move>();
        _jumpScript = GetComponent<Jump>();
		_worldManager = GetComponent<WorldManager>();
        _input = GetComponent<InputManager>();
        onRope = null;
    }

    // FixedUpdate is called once per frame after physics have applied
    private void FixedUpdate()
    {
        //_moveScript.MoveCharacter();
        _moveScript.MoveAnalog(_input.movementX);
    }

    //Check for key inputs every frame
    private void Update()
    {
        if (_input.jump)
            _jumpScript.JumpUp();

        if (_input.Switch)
            _worldManager.SwitchWorld();

        #if UNITY_STANDALONE || UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    _moveScript.MoveLeft();
        //if (Input.GetKeyUp(KeyCode.LeftArrow))
        //    _moveScript.MoveLeft();
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //    _moveScript.MoveRight();
        //if (Input.GetKeyUp(KeyCode.RightArrow))
        //    _moveScript.MoveRight();

        //if (Input.GetButtonDown("Jump"))
        //    _jumpScript.JumpUp();
        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //    _worldManager.SwitchWorld();
        #endif
    }

	void Respawn(GameObject checkpoint) {
		
		transform.position = checkpoint.transform.position;

        _worldManager.ResetRopes();
	}

    //Check when the player collides with an object
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Danger")
        {
            Respawn(_checkpointManager.GetLastCheckpoint());
        }
    }

    void OnTriggerEnter2D(Collider2D hit) {
	if (hit.tag == "Checkpoint") {
		_checkpointManager.SetNewCheckpoint(hit.gameObject);
	}

	if (hit.tag == "Danger") {
		Respawn(_checkpointManager.GetLastCheckpoint());
	}
    } 
}




