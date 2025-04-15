using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 lookDirection;

    [Header("Movement Infor")] [SerializeField]
    private float walkSpeed;

    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float verticalVelocity;
    [SerializeField] private float gravityScale = 9.81f;
    public Vector3 movementDirection;
    public Vector2 moveInput { get; private set; }
    private bool isRunning;
    private float speed;

    private void Start()
    {
        player = GetComponent<Player>();
        AssigbInputEvent();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        var xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        var zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);
        animator.SetFloat(Constant.X_VELOCITY, xVelocity, .1f, Time.deltaTime);
        animator.SetFloat(Constant.Z_VELOCITY, zVelocity, .1f, Time.deltaTime);
        var playAnimRun = isRunning && Vector3.Distance(movementDirection, Vector3.zero) > 0;
        animator.SetBool(Constant.isRunning, playAnimRun);
    }

    private void ApplyRotation()
    {
        lookDirection = player.aim.GetMouseHitInfor().point - transform.position;
        lookDirection.y = 0f;
        lookDirection.Normalize();
        var desiredRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (Vector3.Distance(movementDirection, Vector3.zero) > 0)
            characterController.Move(movementDirection * (Time.deltaTime * speed));
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded == false)
        {
            verticalVelocity -= gravityScale * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -.5f;
        }
    }

    #region AssignInput

    private void AssigbInputEvent()
    {
        controls = player.controls;
        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += _ => moveInput = Vector2.zero;
        controls.Character.Run.performed += _ =>
        {
            isRunning = true;
            speed = runSpeed;
        };
        controls.Character.Run.canceled += _ =>
        {
            isRunning = false;
            speed = walkSpeed;
        };
    }

    #endregion
}