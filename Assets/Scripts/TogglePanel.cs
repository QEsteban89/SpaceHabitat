using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    // Asigna el objeto que quieres mostrar/ocultar (PanelOpcioness) aqu� desde el Inspector.
    public GameObject targetPanel;

    void Start()
    {
        // 1. Obtener el componente Button de este GameObject (donde est� adjunto el script)
        Button btn = GetComponent<Button>();

        // 2. Conectar el evento OnClick del bot�n a la funci�n ToggleVisibility
        if (btn != null)
        {
            btn.onClick.AddListener(ToggleVisibility);
        }

        // 3. Asegurar que el panel de destino inicie oculto (invisible)
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
    }

    // M�todo que se ejecuta cada vez que se pulsa el bot�n.
    void ToggleVisibility()
    {
        if (targetPanel == null)
        {
            Debug.LogError("Error: El objeto de destino (targetPanel) no est� asignado en el Inspector del bot�n.");
            return;
        }

        // 1. Alternar el estado: si estaba activo, se desactiva, y viceversa.
        bool isCurrentlyActive = targetPanel.activeSelf;
        targetPanel.SetActive(!isCurrentlyActive);

        // 2. Obtener el nuevo estado para registrarlo en la consola.
        bool newPanelState = targetPanel.activeSelf;

        // 3. Registrar el estado en la Consola (Validaci�n Booleana).
        Debug.Log("Panel alternado. El estado actual de '" + targetPanel.name + "' es: " + newPanelState);
    }
}