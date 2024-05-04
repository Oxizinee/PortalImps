using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class VirtualMouse : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public RectTransform CursorTransform;
    public RectTransform CanvasRectTransform;
    public Canvas Canvas;
    public bool APressed;

    public float CursorSpeed = 1000;
    public Mouse VirtualMousee;

    private Mouse _currentMouse;

    private bool _previousMouseState;
    private const string _mouseScheme = "Keyboard&Mouse";
    private string _previousControlScheme = "";
    private const string _gamepadScheme = "Gamepad";

    private void OnEnable()
    {
        if(VirtualMousee == null) 
        {
            VirtualMousee = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!VirtualMousee.added)
        {
            InputSystem.AddDevice(VirtualMousee);
        }

        InputUser.PerformPairingWithDevice(VirtualMousee, PlayerInput.user);

        if (CursorTransform != null)
        {
            Vector2 position = CursorTransform.anchoredPosition;
            InputState.Change(VirtualMousee.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMethod;
        PlayerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(VirtualMousee);
        InputSystem.onAfterUpdate -= UpdateMethod;
        PlayerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMethod()
    {
        if (VirtualMousee == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= CursorSpeed * Time.deltaTime;

        Vector2 currentPosition = VirtualMousee.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(VirtualMousee.position, newPosition);
        InputState.Change(VirtualMousee.delta, deltaValue);

        APressed = Gamepad.current.aButton.IsPressed();

        if (_previousMouseState != APressed) 
        {
            VirtualMousee.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.IsPressed());
            InputState.Change(VirtualMousee, mouseState);
            _previousMouseState = APressed;
        }

        AnchorCursor(newPosition);
        
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRectTransform, position, 
            Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out anchoredPosition);
        CursorTransform.anchoredPosition = anchoredPosition;
    }
    private void OnControlsChanged(PlayerInput input)
    {
        if (PlayerInput.currentControlScheme == _mouseScheme && _previousControlScheme != _mouseScheme)
        {
            CursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            _currentMouse.WarpCursorPosition(VirtualMousee.position.ReadValue());
            _previousControlScheme = _mouseScheme;
        }
        else if (PlayerInput.currentControlScheme == _gamepadScheme && _previousControlScheme != _gamepadScheme)
        {
            CursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(VirtualMousee.position, _currentMouse.position.ReadValue());
            AnchorCursor(_currentMouse.position.ReadValue());
            _previousControlScheme = _gamepadScheme;
        }
    }
}
