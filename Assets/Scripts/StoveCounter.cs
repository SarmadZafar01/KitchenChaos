using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress
{

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }


    [SerializeField] private FryingRecipeSO[] cuttingRecipeSoArray;
    [SerializeField] private BurningRecipeSo[] burningRecipeSOArray;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSo burningRecipeSO;


    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;


                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.FryingTimerMax
                    });


                    if (fryingTimer > fryingRecipeSO.FryingTimerMax)
                    {


                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnkitchenObject(fryingRecipeSO.output, this);

                        Debug.Log("Object Firted");

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state

                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;


                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.BurningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.BurningTimerMax)
                    {

                     
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnkitchenObject(burningRecipeSO.output, this);

                       
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state

                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                    case State.Burned:
                    break;

            }
        
        }


           
         
        
    }


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //player is carrying sonmething
            if (player.HasKitchenObject())
            {
                if (HasRecipWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state

                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized= fryingTimer/ fryingRecipeSO.FryingTimerMax
                    });


                }

            }
            //player is not carrying someting
            else
            {

            }
        }
        //there is kitchenobject
        else
        {
            //player carring something
            if (player.HasKitchenObject())
            {

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {

                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state

                        });


                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }

            }
            //player is not carring anyting
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state

                });


                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }



    }


    private bool HasRecipWithInput(KitchenObjectSo inputkitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputkitchenObjectSO);
        return fryingRecipeSO != null;

    }


    private KitchenObjectSo getoutputforInput(KitchenObjectSo inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }

    }


    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in cuttingRecipeSoArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSo GetBurningRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (BurningRecipeSo burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
