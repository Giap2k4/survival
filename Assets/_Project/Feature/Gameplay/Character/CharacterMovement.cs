using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    [SerializeField] protected Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] protected Animator animator;
    [SerializeField] private Joystick joystick; 

    void Update()
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

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}
