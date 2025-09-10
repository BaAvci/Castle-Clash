using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public event Action<TileType, Vector3> UnitTileEffectChange;
    public event Func<Unit, Vector3, bool> UnitMovedTile;
    private TileType walkingElementalEffect;
    private Unit owner;
    private UnitAttack unitAttack;
    private bool? constantMovementDataNeeded;
    private int lastPosition = -1; // x position

    private void Update()
    {
        if (unitAttack.Target == null)
        {
            Movement();
        }
    }

    private void Movement()
    {
        var calculatedSpeed = new Vector3(owner.UnitStats.Speed, 0, 0) * Time.deltaTime;
        if (!owner.PlayerOwned)
        {
            calculatedSpeed *= -1;
        }
        transform.parent.Translate(calculatedSpeed, Space.World);
        int xPos = Mathf.FloorToInt(transform.position.x);
        if (xPos != lastPosition || constantMovementDataNeeded == true)
        {
            lastPosition = xPos;
            UpdatePosition();
        }
    }

    public void Initialize(Unit owner, UnitAttack unitAttack, TileType tileType)
    {
        this.owner = owner;
        this.unitAttack = unitAttack;
        walkingElementalEffect = tileType;
    }

    private void UpdatePosition()
    {
        if (walkingElementalEffect != null)
        {
            UnitTileEffectChange?.Invoke(walkingElementalEffect, transform.position);
        }
        constantMovementDataNeeded = UnitMovedTile?.Invoke(owner, transform.position);
    }
}