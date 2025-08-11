using UnityEngine;
using UnityEngine.InputSystem;

public class MaoMagica : MonoBehaviour
{
    public InputAction MoveAction;
    public Rigidbody2D rigidbody2d;
    Vector2 move;
    public int Maxheath = 5;
    public int currentHeath;
    public float speed = 0.3f;
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        currentHeath = Maxheath;
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    public void ChangeHeath (int amount)
    {
        currentHeath = Mathf.Clamp(currentHeath + amount, 0, Maxheath); 
    }
}
