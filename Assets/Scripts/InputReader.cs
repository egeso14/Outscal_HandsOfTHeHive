using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions _inputSystem_Actions; // this is what actually detects the input in our abstraction



    private void OnEnable()
    {
        if (_inputSystem_Actions == null)
        {
            _inputSystem_Actions = new InputSystem_Actions();
            _inputSystem_Actions.Player.SetCallbacks(this);
            _inputSystem_Actions.UI.SetCallbacks(this);
            SetGameplay();
        }
    }

    private void OnDisable()
    {
        UnSetGameplay();
    }

    public event Action<Vector2> MoveEvent;
    public event Action<float> ZoomEvent;
    public event Action PauseEvent;
    public event Action ResumeEvent;
    public event Action<Vector2> ScreenClickEvent;

    public void SetGameplay()
    {
        _inputSystem_Actions.Player.Enable();
        _inputSystem_Actions.UI.Enable();
    }

    public void UnSetGameplay()
    {
        _inputSystem_Actions.Player.Disable();
        _inputSystem_Actions.UI.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
       
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        // later add logic here to differentiate between ui clicks and world clicks
        //ClickEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
       
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled)
        {
            MoveEvent.Invoke(Vector2.zero);
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
       
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        
    }



    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(context.ReadValue<Vector2>());
            ZoomEvent.Invoke(context.ReadValue<Vector2>().y);
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        ScreenClickEvent.Invoke(Input.mousePosition);
    }
}
