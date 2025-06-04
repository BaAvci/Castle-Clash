using UnityEngine;

public abstract class ElementalInteractionRules : Rule
{
    protected float CountValuesOfNeighbors(ElementalEffect[] elementalEffects, TileManager controller, Tile[,] tileGrid, int x, int y, bool dangerValues)
    {
        float count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int newX = x + i;
                int newY = y + j;
                if (!controller.IsIndexValid(newX, newY))
                {
                    continue;
                }
                foreach (var effect in elementalEffects)
                {
                    if (tileGrid[newX, newY].Current != effect)
                    {
                        return 0;
                    }
                    float modifier = 1;
                    if (newX == 0 || newY == 0)
                    {
                        modifier = 0.5f;
                    }
                    if (dangerValues)
                    {
                        count += effect.DangerValue * modifier;
                    }
                    else
                    {
                        count += effect.SaveValue * modifier;
                    }
                }
            }
        }
        return count;
    }
}
