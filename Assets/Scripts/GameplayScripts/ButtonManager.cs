using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Button[] buttons;
    private bool[] buttonsPressed = new bool[0];

    void Start()
    {
        // Obtener todos los botones hijos
        buttons = GetComponentsInChildren<Button>();
        buttonsPressed = new bool[buttons.Length];

        // Agregar listeners a los botones
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capturar el índice en una variable local para usarlo en el lambda
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    void OnButtonClicked(int index)
    {
        buttonsPressed[index] = true;
        Debug.Log($"Botón {index} pulsado.");

        if (CheckAllButtonsPressed())
        {
            Debug.Log("Todos los botones han sido pulsados.");
        }
    }

    public bool CheckAllButtonsPressed()
    {
        if (buttonsPressed != null && buttonsPressed.Length > 0)
        {
            foreach (bool pressed in buttonsPressed)
            {
                if (!pressed) return false;
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

}

