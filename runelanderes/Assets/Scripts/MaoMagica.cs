using UnityEngine;
using UnityEngine.InputSystem;

public class MaoMagica : MonoBehaviour
{
    public InputAction MoveAction;
    public Rigidbody2D rigidbody2d;
    Vector2 move;
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
