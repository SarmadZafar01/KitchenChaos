using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{


    [SerializeField] private KitchenObjectSo kitchobjectso;
   
 
   





  

    public override void Interact(Player player)
    {//there is no kitchenobject here
        if (!HasKitchenObject())
        {
            //player is carrying sonmething
            if (player.HasKitchenObject()) { 

            player.GetKitchenObject().SetKitchenObjectParent(this);

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
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                           player.GetKitchenObject().DestroySelf();
                        }
                    }
                }


            }
            //player is not carring anyting
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
      

}
