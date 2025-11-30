using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] InputActionReference _Menu;
    [SerializeField] UnityEvent _OnMenuEnable;
    [SerializeField] UnityEvent _OnMenuDisable;

    bool _isMenuEnable = false;

    private void OnEnable()
    {
        _Menu.action.Enable();
        _Menu.action.performed += CallMenu;
    }

    private void OnDisable()
    {
        _Menu.action.performed -= CallMenu;
    }

    private void CallMenu(InputAction.CallbackContext ctx)
    {
        if (_isMenuEnable)
        {
            _OnMenuDisable?.Invoke();
            _isMenuEnable = false;
        }
        else
        {
            _OnMenuEnable?.Invoke();
            _isMenuEnable = true;
        }
    }

    public void CallMenu()
    {
        if (_isMenuEnable)
        {
            _OnMenuDisable?.Invoke();
            _isMenuEnable = false;
        }
        else
        {
            _OnMenuEnable?.Invoke();
            _isMenuEnable = true;
        }
    }

    public bool GetIsMenuEnable() => _isMenuEnable;
}
