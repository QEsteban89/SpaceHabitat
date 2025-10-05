using UnityEngine;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    // Lista de todos los GameObjects de los paneles de opciones
    // (Ej: el panel que contiene a Regulador, Radiador T�rmico, etc.)
    [Tooltip("Arrastra aqu� todos los GameObjects de los paneles secundarios que deben ser mutuamente exclusivos.")]
    public List<GameObject> optionPanels;

    private void Start()
    {
        // Asegura que todos los paneles est�n desactivados al inicio
        // Esto es redundante si ya lo hiciste en el editor, pero es buena pr�ctica.
        DeactivateAllPanels();
    }

    // M�todo p�blico llamado por los botones para mostrar un panel espec�fico
    public void SetActivePanel(GameObject panelToActivate)
    {
        // 1. Desactiva todos los paneles gestionados por este controlador.
        DeactivateAllPanels();

        // 2. Si el panel que se quiere activar no es nulo, act�valo.
        if (panelToActivate != null)
        {
            panelToActivate.SetActive(true);
            Debug.Log("Panel activado: " + panelToActivate.name);
        }
    }

    // M�todo interno para desactivar todos los paneles
    private void DeactivateAllPanels()
    {
        foreach (GameObject panel in optionPanels)
        {
            if (panel != null && panel.activeSelf) // Solo desactiva si est� activo
            {
                panel.SetActive(false);
            }
        }
        Debug.Log("Todos los paneles secundarios han sido desactivados.");
    }
}