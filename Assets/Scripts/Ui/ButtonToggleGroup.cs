using System.Collections.Generic;
using UnityEngine;

public class ButtonToggleGroup : MonoBehaviour
{
    public List<ButtonTheme> buttons;
    private ButtonTheme selectedButton;

    private void Start()
    {
        foreach (ButtonTheme btn in buttons)
        {
            btn.AddListenerButton(() => OnButtonClick(btn));
        }

        if (buttons.Count != 0)
        {
            selectedButton = buttons[0];
            selectedButton.SelectedEffect();
            selectedButton.isSelected = true;
        }
    }
    private void OnButtonClick(ButtonTheme button)
    {
        if (selectedButton == button) return;
        if (selectedButton != null)
        {
            selectedButton.isSelected = false;
            selectedButton.ExitButton();
        }
        selectedButton = button;
        selectedButton.SelectedEffect();
        selectedButton.isSelected = true;
    }
}