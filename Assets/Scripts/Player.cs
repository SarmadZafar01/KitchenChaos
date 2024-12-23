
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnselectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnselectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private Transform KitchenObjectHoldPoint;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateEvent += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGamemanger.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGamemanger.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        
    }

    private void Update()
    {
        HandleMovemnt();
        HandelInteraction();
       
    }

    public bool IsWalking()
    {
        return isWalking;
    }



    private void HandelInteraction()
    {
        Vector2 inputVector = gameInput.GetMomvemntVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }


        float interactDistance = 2f;

    if(    Physics.Raycast(transform.position, lastInteractDir,out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
         if(  raycastHit.transform.TryGetComponent(out BaseCounter BaseCounter))
            {
                if (BaseCounter != selectedCounter)
                {
                    setSelectedCounter(BaseCounter);
                   
                }


            } else
            {
                setSelectedCounter( null);
                
            }
            
     

        }
        else
        {
            setSelectedCounter( null);
           
        }
   

    }


   private void HandleMovemnt()
    {
        //Getting input from GameInput
        Vector2 inputVector = gameInput.GetMomvemntVectorNormalized();


        //Character Move Direaction
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float PlayerRadius = .7f;
        float PlayerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //  //Can not move towards move Dir

            //    //attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDirX, moveDistance);

            if (canMove)
            {

                //      //can move on x direaction
                moveDir = moveDirX;
            }
            else
            {
                //        //can not move on x

                //        //attempt only z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //can move only to z
                    moveDir = moveDirZ;
                }
                else
                {
                    //can not move any direction
                }



            }

        }




        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }



        isWalking = moveDir != Vector3.zero;


        float rotationSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

    }


    private void setSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnselectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
