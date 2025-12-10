using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveSystemBase : MonoBehaviour
{
    public float moveSpeed = 0;
    protected abstract void MoveAction(); // xử lý di chuyển
    public void SetSpeed(float speed) => moveSpeed = speed;
    protected abstract void OnEnable(); // đăng ký vào update
    protected abstract void OnDisable(); // hủy đăng ký update
}
