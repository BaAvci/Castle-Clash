using System;
using System.Collections.Generic;
using UnityEngine;

public enum ExecutionOrder
{
    Up,
    UpLineByLine,
}

[CreateAssetMenu(fileName = "TileData", menuName = "Cellular Automata/Tile Data")]
public class TileData : ScriptableObject
{
    public Material TileMaterial;
    public int Priority = 0;
    public Rule[] Rules;
    public ExecutionOrder ExecutionOrder;
    public ElementalEffect ElementalEffect;
    public ElementalEffect[] AppliedEffects;

    public void ExecuteRules(TileManager controller, Tile[,] tileGrid, ElementalEffect[] elementalEffects)
    {
        List<Rule> elementEffectRules = new();
        foreach (var element in elementalEffects)
        {
            elementEffectRules.AddRange(element.ApplyRulesToTile);
        }
        Rule[] elemtRules = elementEffectRules.ToArray();
        switch (ExecutionOrder)
        {
            case ExecutionOrder.Up:
                UpExecution(controller, tileGrid, elemtRules);
                break;
            case ExecutionOrder.UpLineByLine:
                UpLineByLineExecution(controller, tileGrid, elemtRules);
                break;
        }
    }
    public void ExecuteRules(TileManager controller, Tile[,] tileGrid, ElementalEffect[] elementalEffects, int x, int y)
    {
        List<Rule> elementEffectRules = new();
        foreach (var element in elementalEffects)
        {
            elementEffectRules.AddRange(element.ApplyRulesToTile);
        }
        Rule[] elemtRules = elementEffectRules.ToArray();

        DirectExecution(controller, tileGrid, elemtRules, x, y);
    }

    private void DirectExecution(TileManager controller, Tile[,] tileGrid, Rule[] rules, int x, int y)
    {
        for (int i = 0; i < rules.Length; i++)
        {
            rules[i].ExecuteRule(controller, tileGrid, x, y, ElementalEffect);
        }
    }

    private void UpLineByLineExecution(TileManager controller, Tile[,] tileGrid, Rule[] rules)
    {

        for (int y = 0; y < tileGrid.GetLength(1); y++)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                for (int x = 0; x < tileGrid.GetLength(0); x++)
                {
                    if (tileGrid[x, y].Current != ElementalEffect)
                    {
                        continue;
                    }
                    rules[i].ExecuteRule(controller, tileGrid, x, y, ElementalEffect);
                }
            }
        }
    }

    private void UpExecution(TileManager controller, Tile[,] tileGrid, Rule[] rules)
    {
        for (int i = 0; i < rules.Length; i++)
        {
            for (int y = 0; y < tileGrid.GetLength(1); y++)
            {
                for (int x = 0; x < tileGrid.GetLength(0); x++)
                {
                    if (tileGrid[x, y].Current != ElementalEffect)
                    {
                        continue;
                    }
                    rules[i].ExecuteRule(controller, tileGrid, x, y, ElementalEffect);
                }
            }
        }
    }
}
