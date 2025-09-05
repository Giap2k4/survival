using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // tốc độ di chuyển
    [SerializeField]
    protected Rigidbody2D rb;
    private Vector2 moveInput;          // hướng di chuyển (x,y)

    [SerializeField]
    protected Animator animator;

    void Update()
    {
        // Lấy input từ bàn phím
        float moveX = Input.GetAxisRaw("Horizontal");  // A/D hoặc ←/→
        float moveY = Input.GetAxisRaw("Vertical");    // W/S hoặc ↑/↓

        moveInput = new Vector2(moveX, moveY).normalized; // chuẩn hóa để không chạy chéo nhanh hơn

        // set anim
        animator.SetBool("IsMoving", moveInput.magnitude > 0);

        // set anim Die: animator.SetBool("IsDie", true);
        Quaternion rotation = gameObject.transform.rotation;
        if (moveX < 0)
        {
            rotation.y = 180;
            gameObject.transform.rotation = rotation;
        } else if (moveX > 0.1)
        {
            rotation.y = 0;
            gameObject.transform.rotation = rotation;
        }
    }

    void FixedUpdate()
    {
        // Di chuyển bằng Rigidbody2D
        rb.velocity = moveInput * moveSpeed;
    }
}
