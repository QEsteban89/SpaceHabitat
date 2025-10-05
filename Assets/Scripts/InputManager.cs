using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Las definiciones de Instance y de los eventos se quedan igual, son perfectas.
    public static InputManager Instance { get; private set; }
    public static event Action OnLeftClick;
    public static event Action OnRightClick;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Botón izquierdo presionado");
            OnLeftClick?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Botón derecho presionado");
            OnRightClick?.Invoke();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Debug.Log("Rueda del ratón movida: " + scroll);
        }
    }
}