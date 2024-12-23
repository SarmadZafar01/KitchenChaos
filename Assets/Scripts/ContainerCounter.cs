using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter


{



    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSo kitchobjectso;
   



 


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {

            KitchenObject.SpawnkitchenObject(kitchobjectso, player);
            
            OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
        

    }


   
}
