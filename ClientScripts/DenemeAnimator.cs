using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DenemeAnimator : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    Animator anim;
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        _groundChecker = transform.GetChild(0);
    }


    public bool comboPossible;
    int comboStep;

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");

        if (_inputs != Vector3.zero)
        {
            transform.forward = _inputs;
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim.GetLayerWeight(2) == 1)
            {
                SwordAttack();
            }
            if (anim.GetLayerWeight(4) == 1)
            {
                ArcherAttack();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (anim.GetLayerWeight(4) == 1)
            {
                anim.SetTrigger("archerAttack");
                anim.SetBool("archerAim", false);
            }
        }
        //if (Input.GetButtonDown("Jump") && _isGrounded)
        //{
        //    _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        //}

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("changeWeapon");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    private void ArcherAttack()
    {
        transform.LookAt(Util.VectorUtil(Camera.main.transform.forward, 0));

        anim.SetBool("archerAim", true);
    }

    public void SwordAttack()
    {
        Debug.Log(comboStep);
        Debug.Log(comboPossible);
        if (comboStep == 0)
        {
            anim.Play("SwordAttack1");
            comboStep = 1;
            return;
        }
        if (comboStep != 0)
        {
            if (comboPossible)
            {
                comboStep += 1;
                comboPossible = false;
            }
        }
    }
    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
            anim.Play("SwordAttack2");
        if (comboStep == 3)
            anim.Play("SwordAttack3");
    }
    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }
    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}
