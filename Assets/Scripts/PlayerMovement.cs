﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public float ForwardSpeed = 4f;
	public float JumpForce = 10f;
	public float Gravity = 9.82f;

	public LayerMask GroundMask;

	public GameObject bullet;
	public Vector3 bulletSpeed;
	CharacterController characterController;
	private float jumpVelocity = 0f;
	private bool shouldJump = false;
	private bool shouldDash = false;

	public Animator Animator;

	void Start() {
		characterController = GetComponent<CharacterController> ();
	}

	float dashTimer = 0f;
	bool wasGrounded = false;
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			Application.LoadLevel (0);
		}

		bool grounded = Physics.Raycast(new Ray(transform.position, Vector3.down), 1.0f, GroundMask);
		Debug.DrawRay(transform.position, Vector3.down, !grounded ? Color.red : Color.green);
		float forwardSpeed = ForwardSpeed;

		if (shouldDash) {
			dashTimer += Time.deltaTime;
			Animator.Play("Dash");
		} else if (grounded) {
			Animator.Play("Run");
		}

		if (shouldDash && dashTimer < 0.75f) {
			forwardSpeed = 0f;
		} else if (shouldDash && dashTimer < 1.0f && dashTimer > 0.75f) {
			forwardSpeed = ForwardSpeed * 4f;
		} else if (shouldDash && dashTimer > 1.0f) {
			shouldDash = false;
			dashTimer = 0f;
		}

		jumpVelocity -= Gravity * Time.deltaTime;

		if (shouldJump) {
			jumpVelocity = JumpForce;
			shouldJump = false;
		}

		Vector3 force = new Vector3 (0f, jumpVelocity, 0);

		//Debug.Log ("Force: " + force);
		characterController.Move ((Vector3.right * forwardSpeed + force)*Time.deltaTime);
		wasGrounded = grounded;
	}

	public void doAction(string action) {
		bool grounded = Physics.Raycast(new Ray(transform.position, Vector3.down), 1.0f, GroundMask);

		if (action == "JUMP") {
			shouldJump = true;
			Animator.Play("Jump");
		}
		if (action == "SHOOT") {
			//flytta
			GameObject zeBullet = Instantiate(bullet,transform.position, Quaternion.identity) as GameObject;
			zeBullet.GetComponent<Rigidbody> ().AddForce (bulletSpeed);
		}
		if (action == "DASH") {
			shouldDash = true;
		}
	}

	public void OnControllerColliderHit() {

	}
	public void die() {
		this.enabled = false;
		Debug.LogError ("DIEDEIDIEIDEI"); //TODO: dö
	}

	void FixedUpdate() {

	}

}
