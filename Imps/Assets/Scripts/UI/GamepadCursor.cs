using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField, Tooltip("")]
    private PlayerInput playerInput;
    [SerializeField, Tooltip("Reference to the cursor transform, used to move it.")]
    private RectTransform cursorTransform;
    [SerializeField, Tooltip("Reference to the canvas to get it's render mode.")]
    private Canvas canvas;
    [SerializeField, Tooltip("Reference to the canvas transform to convert screen space coordinates to rect transform coordinates.")]
    private RectTransform canvasRectTransform;
    [SerializeField, Tooltip("Multiplier for the speed of the cursor.")]
    private float cursorSpeed = 1000f;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Camera mainCamera;

    private void OnEnable() {
        mainCamera = Camera.main;

        if (virtualMouse == null)
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        // Pair the device to the user to use the PlayerInput component with the Event System & the Virtual Mouse.
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        // Set the initial cursor position.
        if (cursorTransform != null) {
            Vector2 position = cursorTransform.anchoredPosition;    
            InputState.Change(virtualMouse.position, position);
        }    

        InputSystem.onAfterUpdate += UpdateMotion;
    }

    /// <summary>
    /// Called when the script is disabled.
    /// Remove the virtual mouse from the Input System.
    /// Unsubscribe from the update events.
    /// </summary>
    private void OnDisable() {
        if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    /// <summary>
    /// Called on every frame after the Input System is updated.
    /// Gets the delta value of the joystick and adds padding and clamps it to the edges of the screen.
    /// Changes the Virtual Mouse state of the Input System because of the gamepad input.
    /// </summary>
    private void UpdateMotion() {
        if (virtualMouse == null || Gamepad.current == null) {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        // Clamp the cursor to the screen size.
        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (previousMouseState != aButtonIsPressed) {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }
        
        AnchorCursor(newPosition);
    }

    /// <summary>
    /// Converts screen space corrdinates to a local point in a RectTransform.
    /// Used to convert the cursor coordinates to match it's correct position in the canvas.
    /// </summary>
    /// <param name="position">Cursor position in screen space corrdinates.</param>
    private void AnchorCursor(Vector2 position) {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

}
