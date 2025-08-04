using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour
{
    public event Action<Vector2, TileType> ChangedTileCoordinates;
    [SerializeField] private TileType walkingElementalEffect;
    private bool newTilePositionDelivered = false;
    private Unit owner;
    private void Start()
    {
        //transform.position = new Vector3(0, 1, 3);
        owner = GetComponent<Unit>();
    }
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var calculatedSpeed = new Vector3(owner.Stats.UnitStats.Speed, 0, 0) * Time.deltaTime;
        if (!owner.PlayerOwned)
        {
            calculatedSpeed *= -1;
        }
        transform.Translate(calculatedSpeed);
        var xPos = Math.Round(transform.position.x);
        var yPos = Math.Round(transform.position.z);
        var objxPos = (int)transform.position.x;
        var objyPos = (int)transform.position.z;
        if (walkingElementalEffect != null)
        {
            ApplyEffectOnTile(xPos, yPos, objxPos, objyPos);
        }
    }

    private void ApplyEffectOnTile(double xPos, double yPos, int objxPos, int objyPos)
    {
        if (xPos > objxPos || yPos > objyPos)
        {
            if (!newTilePositionDelivered)
            {
                ChangedTileCoordinates?.Invoke(new Vector2(objxPos, objyPos), walkingElementalEffect);
                newTilePositionDelivered = true;
            }
        }
        else
        {
            newTilePositionDelivered = false;
        }
    }
}