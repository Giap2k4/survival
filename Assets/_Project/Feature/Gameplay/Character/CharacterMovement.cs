using UnityEngine;

public class CharacterMovement : MoveSystemBase
{
    [SerializeField] protected Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] protected Animator animator;
    [SerializeField] public Joystick joystick;

    private Vector3 scaleStart;
    private Vector3 scale;

    protected override void Start()
    {
        base.Start();
        scaleStart = transform.localScale;
        scale = scaleStart;
        scale.x *= -1;
    }

    protected override void MoveAction()
    {
        // Lấy input từ joystick
        float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();

        moveInput = new Vector2(moveX, moveY).normalized;

        transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

        // set anim
        animator.SetBool("IsMoving", moveInput.magnitude > 0);

        // lật nhân vật
        if (moveX < -0.01f) transform.localScale = scale;
        else if (moveX > 0.01f) transform.localScale = scaleStart;
    }
}
