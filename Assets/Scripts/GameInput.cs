using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateEvent;
    private PlayerInputAction playerInputActions;
    private void Awake()
    {
          playerInputActions= new PlayerInputAction();
        playerInputActions.Player.Enable();
        playerInputActions.Player.interact.performed += Interact_performed;
        playerInputActions.Player.InterractAlternate.performed += InterractAlternate_performed;
    }

    private void InterractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateEvent?.Invoke(this, EventArgs.Empty);   
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMomvemntVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

       

        inputVector = inputVector.normalized;


    
        return inputVector;
    }

}
