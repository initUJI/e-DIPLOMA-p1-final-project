using UnityEditor;
using UnityEngine;

// Este editor funcionará para ambas clases: TurnLeftBlock y TurnRightBlock
[CustomEditor(typeof(TurnLeftBlock))]
[CanEditMultipleObjects] // Permite editar múltiples objetos al mismo tiempo
public class TurnBlockEditor : Editor
{
    // Personaliza el Inspector para TurnLeftBlock y TurnRightBlock
    public override void OnInspectorGUI()
    {
        // Actualiza el estado del objeto serializado
        serializedObject.Update();

        // Recorre todas las propiedades, pero oculta el campo "bottomSocket"
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true); // Comienza con la primera propiedad visible
        while (property.NextVisible(false)) // Itera sobre las propiedades restantes
        {
            if (property.name != "bottomSocket") // Excluye "bottomSocket"
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }

        // Aplica los cambios hechos en el inspector
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(TurnRightBlock))]
public class TurnRightBlockEditor : TurnBlockEditor
{
    // Hereda el comportamiento del editor de TurnBlockEditor
}


