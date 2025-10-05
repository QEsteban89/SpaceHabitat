using UnityEngine;

public class GridController : MonoBehaviour
{
    public Transform highlighter;
    public float gridSize = 1.0f;

    private Vector3 lastValidPosition;
    private bool isPositionValid = false;


    private void OnEnable()
    {
        InputManager.OnLeftClick += HandleLeftClick;
        InputManager.OnRightClick += HandleRightClick;
    }

    private void OnDisable()
    {
        InputManager.OnLeftClick -= HandleLeftClick;
        InputManager.OnRightClick -= HandleRightClick;
    }

    private void Start()
    {
        if (highlighter != null)
        {
            highlighter.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            isPositionValid = true;
            highlighter.gameObject.SetActive(true);

            float snappedX = Mathf.FloorToInt(hitInfo.point.x / gridSize) * gridSize;
            float snappedZ = Mathf.FloorToInt(hitInfo.point.z / gridSize) * gridSize;

            lastValidPosition = new Vector3(snappedX + gridSize / 2, 0.1f, snappedZ + gridSize / 2);
            highlighter.position = lastValidPosition;
        }
        else
        {
            isPositionValid = false;
            highlighter.gameObject.SetActive(false);
        }
    }

    /*---------------------------------------------------------------------------Funcion de los botones del raton-------------------------------------------*/
    private void HandleLeftClick()
    {
        // Solo hacemos algo si el cursor está sobre una posición válida.
        if (isPositionValid)
        {
            Debug.Log($"<color=lime>CLIC IZQUIERDO: Colocar objeto en {lastValidPosition}</color>");
            // AQUÍ es donde tu amigo llamaría a la lógica para instanciar el objeto real.
        }
    }
    private void HandleRightClick()
    {
        if (isPositionValid)
        {
            Debug.Log($"<color=orange>CLIC DERECHO: Abrir menú de propiedades en {lastValidPosition}</color>");
            // AQUÍ es donde iría la lógica para abrir la ventana de propiedades del objeto.
        }
    }
}