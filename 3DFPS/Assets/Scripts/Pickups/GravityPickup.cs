using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pickup-derived class which grants ammo to the player that picks it up.
/// </summary>
public class GravityPickup : Pickup
{

    /// <summary>
    /// Description:
    /// When picked up, Cuts gravity in hald
    /// Inputs: Collider collision
    /// Outputs: N/A
    /// </summary>
    /// <param name="collision">The collider that is attempting to pick up this pickup</param>
    public override void DoOnPickup(Collider collision)
    {
        if (collision.tag == "Player")
        {
           PlayerController.gravity = PlayerController.gravity * 0.2f;
        }
        base.DoOnPickup(collision);
    }
}
