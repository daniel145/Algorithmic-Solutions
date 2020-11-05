using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalTetris : MonoBehaviour
{
    #region Private Variables
    private const uint WIDTH = 10, HEIGHT = 20;
    private bool[,] internalState;
    public bool[,] InternalState { get { return GetAll(); } private set { }; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        internalState = new bool[WIDTH, HEIGHT];
    }

    #region Public Functions
    public bool Set(int x, int y)
    {
        if (IsIllegal(x,y))
            throw new System.Exception("Passed illegal values to FillSquare");
        else if (internalState[x, y])
            return false;

        internalState[x, y] = true;
        return true;
    }

    public bool Get(int x, int y)
    {
        if (IsIllegal(x, y))
            throw new System.Exception("Passed illegal values to GetSquare");
        else
            return internalState[x, y];
    }

    public bool Erase(int x, int y)
    {
        if (IsIllegal(x, y))
            throw new System.Exception("Passed illegal values to EraseSquare");
        else if (!internalState[x, y])
            return false;

        internalState[x, y] = false;
        return true;
    }

    public bool EraseRow(int x)
    {
        if (IsIllegal(x, 0))
            throw new System.Exception("Passed illegal values to EraseRow");
        bool rowFilled = true;
        for (int j = 0; j < HEIGHT; j++)
        {
            rowFilled = rowFilled && internalState[x, j];
            internalState[x, j] = false;
        }

        if (!rowFilled)
            return false;
        return true;
    }

    public bool[,] GetAll()
    {
        bool[,] copy = new bool[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++)
            for (int j = 0; j < HEIGHT; j++)
                copy[i, j] = internalState[i, j];
        return copy;
    }
    #endregion

    #region Private Functions
    private bool IsIllegal(int x, int y)
    {
        if (x < 0 || x > 20 || y < 0 || y > 20)
            return true;
        return false;
    }
    #endregion
}
