using UnityEngine;
using System.Collections.Generic;
using System.Text; 

namespace HabitatEngine
{
    public class HabitatEngineController : MonoBehaviour
    {
        private List<Component> allComponents = new List<Component>();
        private HabitatState currentHabitatState = new HabitatState();

        // =================================================================================
        // PASO 1: SIMULAR EL MENÚ DE CONSTRUCCIÓN (Mostrar el "Tablero")
        // =================================================================================

        /// <summary>
        /// GUÍA PARA LA GUI (Acción 1): Muestra todos los componentes disponibles del catálogo.
        /// La GUI llamaría a esto para llenar su menú de botones.
        /// </summary>
        public void ShowAvailableComponents()
        {
            Debug.Log("<color=cyan>--- Componentes Disponibles en el Catálogo ---</color>");
            List<string> componentNames = ComponentRegistry.GetAvailableComponentNames();
            foreach (var name in componentNames)
            {
                Debug.Log("- " + name);
            }
        }

        // =================================================================================
        // PASO 2: SIMULAR LA CREACIÓN Y CONFIGURACIÓN
        // =================================================================================

        /// <summary>
        /// GUÍA PARA LA GUI (Acción 2): El usuario elige un componente del menú.
        /// Lo creamos y mostramos sus propiedades ANTES y DESPUÉS de la configuración.
        /// </summary>
        /// <returns>El componente ya configurado, listo para ser "arrastrado".</returns>
        public Component CreateAndConfigureComponent(string componentName, int id)
        {
            Debug.Log($"<color=yellow>--- Configurando un '{componentName}' (ID: {id}) ---</color>");

            // 1. Validar que el nombre exista y crear la instancia.
            Component newComponent = ComponentRegistry.CreateInstance(componentName, id, $"{componentName}-{id}");
            if (newComponent == null)
            {
                Debug.LogError($"Error: El componente '{componentName}' no existe en el catálogo.");
                return null;
            }

            // 2. Mostrar los datos por defecto (el "JSON" que mencionaste).
            Debug.Log("<b>Estado por Defecto (antes de añadir recursos):</b>");
            PrintComponentDetails(newComponent);

            // 3. Simular la configuración de recursos por parte del usuario.
            // En la GUI real, esto vendría de campos de texto o sliders.
            // Aquí, lo hacemos manualmente para la prueba.
            if (componentName == "Generador")
            {
                newComponent.Resources.Add(new ResourceUsage { Type = ResourceType.Power, Rate = 500f });
            }
            if (componentName == "RefrigeradorAlimentos")
            {
                newComponent.Resources.Add(new ResourceUsage { Type = ResourceType.Power, Rate = -5f });
                newComponent.Resources.Add(new ResourceUsage { Type = ResourceType.Water, Rate = -1f });
            }
            // ... (añadir más configuraciones para otras pruebas) ...

            // 4. Mostrar los datos DESPUÉS de la modificación.
            Debug.Log("<b>Estado Final (después de añadir recursos):</b>");
            PrintComponentDetails(newComponent);

            return newComponent;
        }

        // =================================================================================
        // PASO 3: SIMULAR EL "ARRASTRAR Y SOLTAR" (La Colocación)
        // =================================================================================

        /// <summary>
        /// GUÍA PARA LA GUI (Acción 3): El usuario suelta el componente en el hábitat.
        /// Esta es la acción que dispara toda la lógica de validación.
        /// </summary>
        public void PlaceComponentInHabitat(Component componentToPlace)
        {
            if (componentToPlace == null) return;

            Debug.Log($"<color=green>--- Colocando '{componentToPlace.GetNombre()}' en el hábitat ---</color>");
            allComponents.Add(componentToPlace);

            // 1. Recalcular el estado base del hábitat (encontrar nodos raíz como Generador).
            RecalculateBaseResources();

            // 2. Validar el componente que acabamos de añadir.
            ComponentValidator.Validate(componentToPlace, currentHabitatState);

            // 3. Mostrar el resultado final.
            string status = componentToPlace.GetCurrentState() == ComponentState.Active ? "<color=lime>ACTIVO</color>" : "<color=red>DESACTIVADO</color>";
            Debug.Log($"<b>Resultado de la colocación: {componentToPlace.GetNombre()} está ahora {status}.</b>");

            // Extra: Volver a validar todos los demás por si este nuevo componente los afecta.
            // (Ej: Añadir un generador debería activar componentes que antes estaban desactivados).
            RecalculateAllComponentStates();
        }

        // =================================================================================
        // MÉTODOS INTERNOS DEL MOTOR (El trabajo sucio)
        // =================================================================================

        private void RecalculateBaseResources()
        {
            currentHabitatState.AvailableResources.Clear();
            foreach (var component in allComponents)
            {
                if (component is IMethodLogicNodesComponentDefault logicComponent)
                {
                    logicComponent.NodeLogic(currentHabitatState);
                }
            }
        }

        private void RecalculateAllComponentStates()
        {
            foreach (var component in allComponents)
            {
                ComponentValidator.Validate(component, currentHabitatState);
            }
        }

        // Nuestro "impresor de JSON"
        private void PrintComponentDetails(Component component)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"  Nombre: {component.GetNombre()}, ID: {component.GetID()}");
            sb.AppendLine($"  Masa: {component.GetMass()} kg, Volumen: {component.GetVolume()} m³, Importancia: {component.GetImportance()}");
            sb.AppendLine("  Recursos:");
            if (component.Resources.Count == 0)
            {
                sb.AppendLine("    - Ninguno");
            }
            else
            {
                foreach (var resource in component.Resources)
                {
                    sb.AppendLine($"    - Tipo: {resource.Type}, Tasa: {resource.Rate}");
                }
            }
            Debug.Log(sb.ToString());
        }

        // =================================================================================
        // EL GUION DE PRUEBA (Lo que se ejecuta al darle a Play)
        // =================================================================================
        void Start()
        {
            // --- INICIO DE LA DEMO ---
            Debug.Log("=====================================================");
            Debug.Log("INICIO DE LA SIMULACIÓN DE LA INTERFAZ DE USUARIO");
            Debug.Log("=====================================================");

            // 1. El usuario abre el juego y ve el menú de construcción.
            ShowAvailableComponents();

            // 2. El usuario intenta construir un Refrigerador PRIMERO. Debería fallar.
            Component refrigerador = CreateAndConfigureComponent("RefrigeradorAlimentos", 101);
            PlaceComponentInHabitat(refrigerador); // Esperamos que quede DESACTIVADO.

            // 3. El usuario se da cuenta de su error y ahora construye un Generador.
            Component generador = CreateAndConfigureComponent("Generador", 1);
            PlaceComponentInHabitat(generador); // Esperamos que quede ACTIVO.

            // 4. El usuario ahora construye un Tanque de Agua.
            Component tanque = CreateAndConfigureComponent("TanqueAgua", 201);
            PlaceComponentInHabitat(tanque); // Esperamos que quede ACTIVO.

            // 5. El usuario intenta colocar el Refrigerador de nuevo.
            Debug.Log("<color=yellow>--- Intentando colocar el Refrigerador de nuevo ahora que hay recursos ---</color>");
            RecalculateAllComponentStates(); // Forzamos una re-validación de todo.
            string finalStatus = refrigerador.GetCurrentState() == ComponentState.Active ? "<color=lime>ACTIVO</color>" : "<color=red>DESACTIVADO</color>";
            Debug.Log($"<b>Resultado final: {refrigerador.GetNombre()} está ahora {finalStatus}.</b>"); // Esperamos que ahora esté ACTIVO.
        }
    }
}