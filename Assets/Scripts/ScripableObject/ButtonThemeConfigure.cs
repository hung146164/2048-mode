using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonTheme",menuName = "Data/ButtonTheme")]
public class ButtonThemeConfigure : ScriptableObject
{
    public Color hover;
    public Color click;
    public Color exit;
    public Color selected;
}
