using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private VariableJoystick variableJoystick = null;

    [SerializeField] private GameObject playerRoot;

    private CinemachineVirtualCamera followCamera;
    private CinemachineVirtualCamera aimCamera;


    public float speed = 10;

    [SerializeField] private Transform followCameraObject;
    [SerializeField] private Transform aimCameraObject;
    private Transform aimLookPos;

    [SerializeField] private Text fpsText = null;

    private Vector3 _moveDirection = Vector3.zero;

    private Animator animator = null;

    public void InitializeCamera(CinemachineVirtualCamera _followCamera, CinemachineVirtualCamera _aimCamera)
    {
        followCamera = _followCamera;
        aimCamera = _aimCamera;

        followCamera.Follow = followCameraObject;

        aimCamera.Follow = aimCameraObject;

        aimLookPos = GameObject.Find("LookObject").transform;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        float current = (int)(Time.frameCount / Time.time);
        fpsText.text = current.ToString() + " FPS";

        bool isOnAim = animator.GetBool("archerAim");

        if (isOnAim)
        {
            _moveDirection =
    Util.VectorUtil(aimCamera.transform.forward, transform.position.y) * variableJoystick.Vertical
     + Util.VectorUtil(aimCamera.transform.right, transform.position.y) * variableJoystick.Horizontal
     + transform.position;
        }
        else
        {
            _moveDirection =
 Util.VectorUtil(followCamera.transform.forward, transform.position.y) * variableJoystick.Vertical
  + Util.VectorUtil(followCamera.transform.right, transform.position.y) * variableJoystick.Horizontal
  + transform.position;
        }


        _moveDirection = Util.VectorUtil(_moveDirection, transform.position.y);



        //****************************************ANIMATOR DENEME -----------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("archerAim", true);
            AnimateTheCameraToClose();
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            AnimateTheCameraToFar();
            animator.SetTrigger("archerAttack");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("changeWeapon");
            SetAnimatorLayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("changeWeapon");
            SetAnimatorLayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("changeWeapon");
            SetAnimatorLayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animator.SetTrigger("takeBow");
            SetAnimatorLayer(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            animator.SetTrigger("changeWeapon");
            SetAnimatorLayer(5);
        }

        if (isOnAim)
        {
            float h = variableJoystick.Horizontal;
            float v = variableJoystick.Vertical;

            Vector3 inputDirection = new Vector3(h, 0, v);

            var cameraForward = aimCamera.transform.forward;
            var cameraRight = aimCamera.transform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            Vector3 desiredDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            AnimateThePlayer(desiredDirection);
        }


    }
    void FixedUpdate()
    {
        /**-----------*/
        bool isMoving = variableJoystick.Direction.normalized.magnitude > 0.1f;

        bool isOnAim = animator.GetBool("archerAim");

        bool isRun = animator.GetBool("run");

        if (isOnAim)
        {
            ClientSend.PlayerInAim(Util.VectorUtil(aimLookPos.position, transform.position.y), AimType.BowAim, isMoving, _moveDirection);
        }
        else if (isMoving)
            ClientSend.PlayerMoveDirection(_moveDirection, isMoving);
        if (isMoving && !isOnAim)
        {
            if (!isRun)
                ClientSend.PlayerAnimation("run", AnimType.boolAnim, true);
        }
        else
        {
            if (isRun)
                ClientSend.PlayerAnimation("run", AnimType.boolAnim, false);
        }
    }
    void SetAnimatorLayer(int index)
    {
        for (int i = 1; i < 5; i++)
        {
            if (i == index)
            {
                animator.SetLayerWeight(i, 1);
            }
            else
            {
                animator.SetLayerWeight(i, 0);
            }
        }
    }
 
    private void AnimateThePlayer(Vector3 desiredDirection)
    {
        if (!animator)
            return;

        Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        float forw = Vector3.Dot(movement, transform.forward);
        float stra = Vector3.Dot(movement, transform.right);

        animator.SetFloat("AimX", forw);
        animator.SetFloat("AimY", stra);
    }
    private void AnimateTheCameraToClose()//aim halindeki kamera ayarı
    {
        followCamera.Priority = 1;
        aimCamera.Priority = 2;
    }
    private void AnimateTheCameraToFar()//normal kamera ayarı
    {
        followCamera.Priority = 2;
        aimCamera.Priority = 1;
    }
   
}
