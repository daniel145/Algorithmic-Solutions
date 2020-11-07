using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalTetris : MonoBehaviour
{
    #region Private Variables
    private const uint WIDTH = 10, HEIGHT = 20;
    private bool[,] internalState;
    public bool[,] InternalState { get { return GetAll(); } private set {} }
    public const List<string> PIECES_LIST = ["Line", "RL", "L", "Square", "S", "RS", "T"] //R stands for reversed
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

    public void EraseRows(int y, int rowCount = 1)
    {
        // Erases rowCount rows starting from row y and moving upwards.
        // Rows above erased rows falls to rowCount rows so they rest atop other blocks
        
        if (IsIllegal(0, y))
            throw new System.Exception("Passed illegal values to EraseRow");
        else if (rowCount > HEIGHT - y)
        {
            Debug.Log("EraseRows: Number of rows to erase reaches past grid height!");
            rowCount = (int)HEIGHT - y;
        }

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = y + rowCount; j < HEIGHT; j++)
                internalState[i, j - rowCount] = internalState[i, j];
            for (int k = (int)HEIGHT - rowCount; k < HEIGHT; k++)
                internalState[i, k] = false;
        }
    }

    public int AutoPrune()
    {
        // Erases first group of contiguous rows from internalState that are complete (all true)
        // Returns the number of erased rows

        bool searching = true;
        int startRow = -1;
        int rowCount = 0;

        for (int j = 0; j < HEIGHT; j++)
        {
            bool rowFill = CheckRowFill(j);
            if (rowFill && searching)
            {
                startRow = j;
                searching = false;
            }
            else if (rowFill)
                rowCount++;
            else if (!searching && !rowFill)
                break;
        }

        EraseRows(startRow, rowCount);
        return rowCount;
    }

    public bool CheckRowFill(int y)
    {
        for (int i = 0; i < WIDTH; i++)
            if (!internalState[i, y])
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

    public void PrintState()
    {
        string row = "";
        for (int j = (int)HEIGHT - 1; j >= 0; j--)
        {
            for (int i = 0; i < WIDTH; i++)
            {
                if (internalState[i, j])
                    row += "+";
                else
                    row += "-";
            }
            row += '\n';
        }
        Debug.Log(row);
    }

    //Takes in a tetris piece (Line, RL, L, Square, RS, S)
    //returns 
    public List<bool[,]> PotentialStates(string piece) 
    {
        List<bool[,]> statesList = new List<bool[,]>();
        // have to make a deep copy of internalState b/c its passed by reference
        int startingLoc = 0; //starting y coordinate
        if (piece = "Line") 
        {
            for(int i = 0; i < WIDTH; i++)
            {
                bool[,] tempState = GetAll();
                startingLoc = checkHeight(i); //will be implemented later
                tempState[i, startingLoc + 1] = true;
                tempState[i, startingLoc + 2] = true;
                tempState[i, startingLoc + 3] = true;
                tempState[i, startingLoc + 4] = true;
                PotentialStates.add(tempState);
                if (i + 4 < WIDTH) //Handles horizontal lines, makes sure it does not go over
                {
                    tempState = GetAll(); //resets tempState
                    tempState[i, startingLoc + 1] = true;
                    tempState[i + 1, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc + 1] = true;
                    tempState[i + 3, startingLoc + 1] = true;
                    PotentialStates.add(tempState);
                }
            }
        }
        else if (piece = "RL") 
        {
            for(int i = 0; i < WIDTH - 1; i++)
            {
                bool[,] tempState = GetAll();
                startingLoc = checkHeight(i);
            }
        }
        else if (piece = "L") { }
        else if (piece = "Square") 
        {
            for(int i = 0; i < WIDTH-1; i++)
            {
                bool[,] tempState = GetAll();
                startingLoc = checkHeight(i); 
                tempState[i, startingLoc + 1] = true;
                tempState[i, startingLoc + 2] = true;
                tempState[i + 1, startingLoc + 1] = true;
                tempState[i + 1, startingLoc + 2] = true;
                PotentialStates.add(tempState);
            }
        }
        else if (piece = "RS") { }
        else { } //Normal S
        return statesList;
    }
    #endregion

    #region Private Functions
    private bool IsIllegal(int x, int y)
    {
        if (x < 0 || x > WIDTH || y < 0 || y > HEIGHT)
            return true;
        return false;
    }
    #endregion
}
