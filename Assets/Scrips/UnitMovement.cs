using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public event Action<Vector2Int, TileType> ChangedTileCoordinates;
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] TileType walkingElementalEffect;
    private bool newTilePositionDelivered = false;
    private void Start()
    {
        transform.position = new Vector3(0, 1, 3);
    }
    private void Update()
    {
        Movement();
    }

#if UNITY_EDITOR
    // TODO: Create Unit Stat class and move this to there
    public void ApplyEffect(float value)
    {
        Debug.Log(value);
    }
    public void TESTApplyEffect(InstanceEffect instanceEffect)
    {
        Debug.Log(instanceEffect.GetDiscription());
        ApplyEffect(instanceEffect.Value);
    }
    public void TESTApplyEffect(StatusEffects statusEffect)
    {
        ApplyEffect(statusEffect.Duration);
        WaitForSeconds wait = new WaitForSeconds(statusEffect.Duration);
        StartCoroutine(Co_StatusEffectApplication(wait, statusEffect.GetDiscription()));
    }

    private IEnumerator Co_StatusEffectApplication(WaitForSeconds wait, string description)
    {
        Debug.Log(description);
        Debug.Log("Status effect has been applied!");
        yield return wait;
        Debug.Log("Status effect has been removed!");
    }
#endif

    private void Movement()
    {
        var calculatedSpeed = new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
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
                ChangedTileCoordinates?.Invoke(new Vector2Int(objxPos, objyPos), walkingElementalEffect);
                newTilePositionDelivered = true;
            }
        }
        else
        {
            newTilePositionDelivered = false;
        }
    }
}