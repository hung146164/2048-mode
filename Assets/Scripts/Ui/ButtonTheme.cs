using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ButtonTheme : MonoBehaviour
{

    private Image _imageButton;
    private Button _button;

    public bool isSelected = false;

    [SerializeField] private ButtonThemeConfigure _buttonThemeConfigure;
    private void Awake()
    {
        _imageButton = GetComponent<Image>();
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        if (ThemeManager.Instance == null)
        {
            Debug.LogError("ThemeManager instance is NULL!");
            return;
        }
        ThemeManager.Instance.OnButtonThemeChange+=ChangeTheme;
    }
    void OnDestroy()
    {
        ThemeManager.Instance.OnButtonThemeChange -= ChangeTheme;

    }
    public void AddListenerButton(UnityAction call)
    {
        _button.onClick.AddListener(call);
    }
    public void HoverButton()
    {
        if (!isSelected)
        {
            _imageButton.color = _buttonThemeConfigure.hover;
        }
    }
    public void ClickButton()
    {
        if (!isSelected)
        {
            _imageButton.color = _buttonThemeConfigure.click;
        }
        
    }
    public void ExitButton()
    {
        if (!isSelected)
        {
            _imageButton.color = _buttonThemeConfigure.exit;
        }
    }

    public void SelectedEffect()
    {
        _imageButton.color = _buttonThemeConfigure.selected;
    }
    private void ChangeTheme(ButtonThemeConfigure buttonTheme)
    {
        _buttonThemeConfigure = buttonTheme;
        _imageButton.color = _buttonThemeConfigure.exit;
    }
}
