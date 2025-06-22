using UnityEngine;

public abstract class GameOfLifeRule : RuleCopy
{
    protected int CountLivingNeighbors(AutomataController automataController, Cell[,] cellGrid, int x, int y)
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0 & i == 0)
                {

                }

                int newX = x + i;
                int newY = y + j;
                if (automataController.IsIndexValid(newX, newY) && cellGrid[newX, newY].Current == CellTypeOld.Alive)
                {
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
}
