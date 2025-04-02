using System;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;
    public event Action<ButtonThemeConfigure> OnButtonThemeChange = delegate { };
    
    public ButtonThemeConfigure[] buttonThemeConfigures;
    private void Awake()
    {
        Instance = this;
    }
    public void ChangeButtonTheme(int index)
    {
        if (index >= buttonThemeConfigures.Length) return;
        OnButtonThemeChange(buttonThemeConfigures[index]);
    }
}
