using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro; // Asegúrate de tener esta referencia si usas TextMeshPro

public class DynamicTextUpdater : MonoBehaviour
{
    public LocalizeStringEvent localizeStringEvent;

    // Puedes inyectar las dependencias de LocalizedString si deseas actualizar dinámicamente
    private LocalizedString localizedString = new LocalizedString();

    void Start()
    {
        // Si necesitas inicializar algo
    }

    // Método para actualizar el texto dinámicamente
    public void UpdateLocalizedString(string newKey)
    {
        // Cambia la clave del LocalizedString
        localizedString.TableReference = "TextsTables"; // Reemplaza con el nombre de tu tabla
        localizedString.TableEntryReference = newKey;

        // Asigna el LocalizedString al evento de localización
        localizeStringEvent.StringReference = localizedString;

        // Opcional: Forzar una actualización del evento
        //localizeStringEvent.OnEnable();
    }
}

