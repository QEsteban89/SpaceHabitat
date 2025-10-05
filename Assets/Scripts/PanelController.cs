using UnityEngine;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    // Lista de todos los GameObjects de los paneles de opciones
    // (Ej: el panel que contiene a Regulador, Radiador Térmico, etc.)
    [Tooltip("Arrastra aquí todos los GameObjects de los paneles secundarios que deben ser mutuamente exclusivos.")]
    public List<GameObject> optionPanels;

    private void Start()
    {
        // Asegura que todos los paneles estén desactivados al inicio
        // Esto es redundante si ya lo hiciste en el editor, pero es buena práctica.
        DeactivateAllPanels();
    }

    // Método público llamado por los botones para mostrar un panel específico
    public void SetActivePanel(GameObject panelToActivate)
    {
        // 1. Desactiva todos los paneles gestionados por este controlador.
        DeactivateAllPanels();

        // 2. Si el panel que se quiere activar no es nulo, actívalo.
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
            Debug.Log("Panel activado: " + panelToActivate.name);
        }
    }

    // Método interno para desactivar todos los paneles
    private void DeactivateAllPanels()
    {
        foreach (GameObject panel in optionPanels)
        {
            if (panel != null && panel.activeSelf) // Solo desactiva si está activo
            {
                panel.SetActive(false);
            }
        }
        Debug.Log("Todos los paneles secundarios han sido desactivados.");
    }
}