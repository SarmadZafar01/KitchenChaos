using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemove;

    [SerializeField] private KitchenObjectSo platekitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax =8f;
    private int spawnPlateCount = 0;
    private int spawnPlateCountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.time;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (spawnPlateCount < spawnPlateCountMax)
            {
                spawnPlateCount++;

                OnPlateSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (spawnPlateCount > 0)
            {
                spawnPlateCount--;

                KitchenObject.SpawnkitchenObject(platekitchenObjectSO, player);
                OnPlateRemove?.Invoke(this,EventArgs.Empty);
            }
        }
    }

}
