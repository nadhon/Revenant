using UnityEngine;
using UnityEngine.InputSystem;

public class MapScript : MonoBehaviour
{

    bool map_active;
    [SerializeField] GameObject map;

    bool ButtonMap;

    public PlayerInputActions PlayerInputActions { get; private set; }

    private void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Player.Map.performed += ctx => ButtonMap = true;
    }
    private void OnEnable()
    {
        PlayerInputActions.Player.Enable();
    }
    private void OnDisable()
    {
        PlayerInputActions.Player.Disable();
    }
    private void Start()
    {
        map.SetActive(false);
        map_active = false;
    }
    void Update()
    {
        if (ButtonMap)
        {
            if (map_active)
            {
                map.SetActive(false);
                map_active = false;
            }
            else
            {
                map.SetActive(true);
                map_active = true;
            }
            ButtonMap = false; // Reset the button state after handling
        }
    }
}
