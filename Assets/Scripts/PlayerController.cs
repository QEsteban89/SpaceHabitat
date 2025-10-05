using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    /*------------------------------------------------------Atributos Player--------------------------------------------------*/
    [Tooltip("La velocidad de movimiento del jugador.")]
    [SerializeField] private float speed = 5.0f; 

    /*-------------------------------------------------------Instancias.......................................................*/
    private Rigidbody rgPlayer;
    private Vector3 moveInput;

    void Awake()
    {
        rgPlayer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveZ = Input.GetAxisRaw("Vertical");   

        moveInput = new Vector3(moveX, 0f, moveZ);
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }
    void FixedUpdate()
    {
        Vector3 finalVelocity = moveInput * speed * Time.fixedDeltaTime;
        rgPlayer.MovePosition(rgPlayer.position + finalVelocity);
    }
}