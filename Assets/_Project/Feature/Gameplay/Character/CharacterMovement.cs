using UnityEngine;

public class CharacterMovement : MoveSystemBase, IUpdateManager
{
    [SerializeField] protected Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] protected Animator animator;
    [SerializeField] public Joystick joystick;

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    protected override void OnEnable()
    {
        UpdateManager.instance.Register(this);
    }

    protected override void OnDisable()
    {
        UpdateManager.instance.UnRegister(this);
    }

    public void UpdateMe()
    {
        MoveAction();
    }

    protected override void MoveAction()
    {
        // Lấy input từ joystick
        float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();

        moveInput = new Vector2(moveX, moveY).normalized;

        // set anim
        animator.SetBool("IsMoving", moveInput.magnitude > 0);

        // lật nhân vật
        Quaternion rotation = gameObject.transform.rotation;
        if (moveX < 0)
        {
            rotation.y = 180;
            gameObject.transform.rotation = rotation;
        }
        else if (moveX > 0.01f)
        {
            rotation.y = 0;
            gameObject.transform.rotation = rotation;
        }
    }
}
