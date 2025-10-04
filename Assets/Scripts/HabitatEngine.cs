using System;
using System.Collections.Generic;

namespace HabitatEngine
{
    /*===============================================Clases de Soporte========================================*/
    public class HabitatState
    {
        public HashSet<ResourceType> AvailableResources = new HashSet<ResourceType>();
    }
    public class ValidationResult
    {
        public bool IsValid;
        public string Message;

        public ValidationResult(bool isValid, string message)
        {
            this.IsValid = isValid;
            this.Message = message;
        }
    }
    /*========================================================================================================*/


    /*==============================================Clases Principales=========================================*/
    public abstract class Component
    {
        private string Nombre;
        private int ID;
        private float Mass;
        private float Volume;
        private int Importance;
        protected ComponentState CurrentState;
        public List<ResourceUsage> Resources; //Agrega recursos 
        public Dictionary<string, object> Properties; //Diccionario de los DATOS de los componentes individuales

        public Component(string nombre, int id, float mass, float volume, int importance)
        {
            this.Nombre = nombre;
            this.ID = id;
            this.Mass = mass;
            this.Volume = volume;
            this.Importance = importance;
            this.Resources = new List<ResourceUsage>();
            this.Properties = new Dictionary<string, object>();
            this.CurrentState = ComponentState.Uninitialized;
        }
        public virtual void Initialize() { this.CurrentState = ComponentState.Active; }
        public string GetNombre() { return this.Nombre; }
        public int GetID() { return this.ID; }
        public float GetMass() { return this.Mass; }
        public float GetVolume() { return this.Volume; }
        public int GetImportance() { return this.Importance; }
        public ComponentState GetCurrentState() { return this.CurrentState; }
        public void UpdateState(ComponentState newState, string message = "") { this.CurrentState = newState; } //Verifica las reglas aplicadas a cada componente
    }
    /*========================================================================================================*/

    /*===============================================Clases Secundarias=======================================*/
    public enum ResourceType { Power, Water, Oxygen, Data }
    public enum ComponentState { Uninitialized, Active, Disabled, Error }
    public class ResourceUsage { public ResourceType Type; public float Rate; }
    /*=======================================================================================================*/

    /*=============================================Logica de Conexion Nodos=================================*/
    public interface IMethodLogicNodesComponentDefault
    {
        ValidationResult RequirementsNode(HabitatState currentState);
        void NodeLogic(HabitatState currentState);
        ValidationResult VerificationNode();
    }
    /*=====================================================================================================*/

    /*============================================Componentes Existentes=====================================*/
    //[1] Energía [Nodo Nivel 0 - Superior]
    public class Generador : Component, IMethodLogicNodesComponentDefault
    {
        public Generador(int id, string nombre) : base(nombre, id, 5000f, 50f, 10) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { return new ValidationResult(true, "El recurso energia se implemento."); }
        public void NodeLogic(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { currentState.AvailableResources.Add(ResourceType.Power); } }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Generador es valido!."); }
    }
    public class Breaker : Component, IMethodLogicNodesComponentDefault
    {
        public Breaker(int id, string nombre) : base(nombre, id, 10f, 0.2f, 6) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso energia."); } return new ValidationResult(true, "Se encontro recurso de energia."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Conexión Hecha."); }
    }

    //[2] Almacenamiento [Nodo Nivel 0 - Superior]
    public class TanqueAgua : Component, IMethodLogicNodesComponentDefault
    {
        public TanqueAgua(int id, string nombre) : base(nombre, id, 1000f, 15f, 8) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { return new ValidationResult(true, "El recurso agua se implemento."); }
        public void NodeLogic(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Water)) { currentState.AvailableResources.Add(ResourceType.Water); } }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Tanque es valido."); }
    }
    public class RefrigeradorAlimentos : Component, IMethodLogicNodesComponentDefault
    {
        public RefrigeradorAlimentos(int id, string nombre) : base(nombre, id, 500f, 10f, 4) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } if (!currentState.AvailableResources.Contains(ResourceType.Water)) { return new ValidationResult(false, "Se requiere del recurso WATER."); } return new ValidationResult(true, "Se encontraron los recursos WATER y POWER."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Conexión válida."); }
    }

    //[3] Soporte Vital [Nodo Nivel 1 - Importante]
    public class ECLSS : Component, IMethodLogicNodesComponentDefault
    {
        public ECLSS(int id, string nombre) : base(nombre, id, 800f, 20f, 9) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } if (!currentState.AvailableResources.Contains(ResourceType.Water)) { return new ValidationResult(false, "Se requiere del recurso WATER."); } return new ValidationResult(true, "Se encontraron los recursos de WATER y POWER."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "ECLSS es valido."); }
    }
    public class Purificador : Component, IMethodLogicNodesComponentDefault
    {
        public Purificador(int id, string nombre) : base(nombre, id, 600f, 15f, 7) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } if (!currentState.AvailableResources.Contains(ResourceType.Water)) { return new ValidationResult(false, "Se requiere del recurso WATER."); } return new ValidationResult(true, "Se encontraron los recursos de WATER y POWER."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Purificador es valido."); }
    }

    //[4] Control Térmico [Nodo Nivel 1 - Importante]
    public class ReguladorTermico : Component, IMethodLogicNodesComponentDefault
    {
        public ReguladorTermico(int id, string nombre) : base(nombre, id, 50f, 5f, 7) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Data)) { currentState.AvailableResources.Add(ResourceType.Data); } }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Regulador Térmico es valido."); }
    }
    public class PanelRadiadorExterno : Component, IMethodLogicNodesComponentDefault
    {
        public PanelRadiadorExterno(int id, string nombre) : base(nombre, id, 200f, 10f, 5) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Data)) { return new ValidationResult(false, "Requiere un Regulador Térmico para funcionar."); } return new ValidationResult(true, "Regulador Térmico encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Panel Radiador es valido."); }
    }

    //[5] Comunicación [Nodo Nivel 2 - Secundario]
    public class AntenaDeComunicacion : Component, IMethodLogicNodesComponentDefault
    {
        public AntenaDeComunicacion(int id, string nombre) : base(nombre, id, 300f, 12f, 6) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Antena es valida."); }
    }
    public class RouterInterno : Component, IMethodLogicNodesComponentDefault
    {
        public RouterInterno(int id, string nombre) : base(nombre, id, 5f, 0.1f, 4) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Router es valido."); }
    }

    //[6] Preparación de Alimentos [Nodo Nivel 2 - Secundario]
    public class Hidroponico : Component, IMethodLogicNodesComponentDefault
    {
        public Hidroponico(int id, string nombre) : base(nombre, id, 400f, 18f, 5) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } if (!currentState.AvailableResources.Contains(ResourceType.Water)) { return new ValidationResult(false, "Se requiere del recurso WATER."); } return new ValidationResult(true, "Recursos POWER y WATER encontrados."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Hidropónico es valido."); }
    }
    public class EstacionCocina : Component, IMethodLogicNodesComponentDefault
    {
        public EstacionCocina(int id, string nombre) : base(nombre, id, 250f, 8f, 3) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Estación de Cocina es valida."); }
    }

    //[7] Atención Médica [Nodo Nivel 3 - Terciario]
    public class EstacionMedica : Component, IMethodLogicNodesComponentDefault
    {
        public EstacionMedica(int id, string nombre) : base(nombre, id, 700f, 22f, 8) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } if (!currentState.AvailableResources.Contains(ResourceType.Data)) { return new ValidationResult(false, "Se requiere del recurso DATA."); } return new ValidationResult(true, "Recursos POWER y DATA encontrados."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Estación Médica es valida."); }
    }
    public class KitPrimerosAuxilios : Component, IMethodLogicNodesComponentDefault
    {
        public KitPrimerosAuxilios(int id, string nombre) : base(nombre, id, 15f, 0.5f, 7) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { return new ValidationResult(true, "Componente pasivo, no requiere nada."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Kit es valido."); }
    }

    //[8] Sueño y Ejercicio [Nodo Nivel 3 - Terciario]
    public class LiteraDeAstronautas : Component, IMethodLogicNodesComponentDefault
    {
        public LiteraDeAstronautas(int id, string nombre) : base(nombre, id, 100f, 3f, 2) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Litera es valida."); }
    }
    public class CaminadoraEspacial : Component, IMethodLogicNodesComponentDefault
    {
        public CaminadoraEspacial(int id, string nombre) : base(nombre, id, 250f, 4f, 6) { }
        public ValidationResult RequirementsNode(HabitatState currentState) { if (!currentState.AvailableResources.Contains(ResourceType.Power)) { return new ValidationResult(false, "Se requiere del recurso POWER."); } return new ValidationResult(true, "Recurso POWER encontrado."); }
        public void NodeLogic(HabitatState currentState) { }
        public ValidationResult VerificationNode() { return new ValidationResult(true, "Caminadora es valida."); }
    }
    /*======================================================================================================================================*/
    /*==========================================================Registro Componentes========================================================*/

    public static class ComponentRegistry //Registra los componentes en un todo
    {
        private static readonly Dictionary<string, Type> componentTypes = new Dictionary<string, Type>
        {
            // Energía
            { "Generador", typeof(Generador) },
            { "Breaker", typeof(Breaker) },

            // Almacenamiento
            { "TanqueAgua", typeof(TanqueAgua) },
            { "RefrigeradorAlimentos", typeof(RefrigeradorAlimentos) },

            // Soporte Vital
            { "ECLSS", typeof(ECLSS) },
            { "Purificador", typeof(Purificador) },

            // Control Térmico
            { "ReguladorTermico", typeof(ReguladorTermico) },
            { "PanelRadiadorExterno", typeof(PanelRadiadorExterno) },

            // Comunicación
            { "AntenaDeComunicacion", typeof(AntenaDeComunicacion) },
            { "RouterInterno", typeof(RouterInterno) },
            
            // Alimentos
            { "Hidroponico", typeof(Hidroponico) },
            { "EstacionCocina", typeof(EstacionCocina) },

            // Médica
            { "EstacionMedica", typeof(EstacionMedica) },
            { "KitPrimerosAuxilios", typeof(KitPrimerosAuxilios) },

            // Ejercicio
            { "LiteraDeAstronautas", typeof(LiteraDeAstronautas) },
            { "CaminadoraEspacial", typeof(CaminadoraEspacial) }
        };
        public static List<string> GetAvailableComponentNames() //Devuelve nombres
        {
            return new List<string>(componentTypes.Keys);
        }
        public static Component CreateInstance(string componentName, int id, string instanceName) //Selecciona un componente a raiz de un nombre
        {
            if (componentTypes.ContainsKey(componentName))
            {
                Type type = componentTypes[componentName];
                return (Component)Activator.CreateInstance(type, id, instanceName);
            }
            return null;
        }
    }
    public static class ComponentValidator //Validar componentes con las reglas pre hechas (Escaner de verificacion)
    {

        public static void Validate(Component component, HabitatState currentState)
        {
            if (component is IMethodLogicNodesComponentDefault logicComponent)
            {
                ValidationResult result = logicComponent.RequirementsNode(currentState);

                if (result.IsValid)
                {
                    component.UpdateState(ComponentState.Active);
                }
                else
                {
                    component.UpdateState(ComponentState.Disabled);
                }
            }
            else
            {
                component.UpdateState(ComponentState.Active);
            }
        }
    }
    /*======================================================================================================================================*/


}
