using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalTetris : MonoBehaviour
{
    #region Private Variables
    private const int WIDTH = 10, HEIGHT = 20;
    private bool[,] internalState;
    public bool[,] InternalState { get { return GetAll(); } private set {} }
    public const List<string> PIECES_LIST = ["Line", "RL", "L", "Square", "S", "RS", "T"] //R stands for reversed
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        internalState = new bool[WIDTH, HEIGHT];
    }
    #endregion

    #region Public Functions
    public bool Set(int x, int y, bool value = true)
    {
        // Set the value at x, y of the internal state to value.

        if (IsIllegal(x,y))
            throw new System.Exception("Passed illegal values to Set");
        else if (internalState[x, y] == value)
            return false;

        internalState[x, y] = value;
        return true;
    }

    public void SetMultiple(List<int> coordinates, bool value = true)
    {
        // Sets multiple coordinates to value
        // Coordinates represented by list of ints of alternating x, y values
        // Last element of lists of odd length are ignored

        for (int i = 0; i < coordinates.Count; i += 2)
        {
            if (i + 1 >= coordinates.Count)
                break;
            int x = coordinates[i];
            int y = coordinates[i + 1];
            internalState[x, y] = value;
        }
    }

    public bool Get(int x, int y)
    {
        // Get the value at x, y of the internal state

        if (IsIllegal(x, y))
            throw new System.Exception("Passed illegal values to Get");
        else
            return internalState[x, y];
    }

    public bool GetMultiple(List<int> coordinates, bool value = false)
    {
        // Checks if all coordinate values are parameter value
        // Coordinates represented by list of ints of alternating x, y values
        // Last element of lists of odd length are ignored

        for (int i = 0; i < coordinates.Count; i += 2)
        {
            if (i + 1 >= coordinates.Count)
                break;
            int x = coordinates[i];
            int y = coordinates[i + 1];
            if (internalState[x, y] != value)
                return false;
        }
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
            rowCount = HEIGHT - y;
        }

        for (int i = 0; i < WIDTH; i++)
        {
            for (int j = y + rowCount; j < HEIGHT; j++)
                internalState[i, j - rowCount] = internalState[i, j];
            for (int k = HEIGHT - rowCount; k < HEIGHT; k++)
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
        // Returns true if a row is filled (all values are true)

        for (int i = 0; i < WIDTH; i++)
            if (!internalState[i, y])
                return false;
        return true;
    }

    public int CheckHeight(int x)
    {
        // Checks the tallest filled value of column x
        // Returns -1 if the column is empty

        for (int j = HEIGHT - 1; j > -1; j--)
            if (internalState[x, j])
                return j;

        return -1;
    }

    public int CheckHeightAll()
    {
        // Returns the height of the tallest filled square

        int tallest = -1;
        for (int i = 0; i < WIDTH; i++)
        {
            int height = CheckHeight(i);
            if (height > tallest)
                tallest = height;
        }
        return tallest;
    }

    public bool[,] GetAll()
    {
        // Get a deep copy of the InternalState

        bool[,] copy = new bool[WIDTH, HEIGHT];
        for (int i = 0; i < WIDTH; i++)
            for (int j = 0; j < HEIGHT; j++)
                copy[i, j] = internalState[i, j];
        return copy;
    }

    public void PrintState()
    {
        // Debug.Log the current state of InternalState

        string row = "";
        for (int j = HEIGHT - 1; j >= 0; j--)
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
    //returns list of potential states
    //might need to check if the entire shape is empty before placing the piece using GetMultiple
    public List<bool[,]> PotentialStates(string piece) 
    {
        List<bool[,]> statesList = new List<bool[,]>();
        // have to make a deep copy of internalState b/c its passed by reference
        int startingLoc = 0; //starting y coordinate
        bool[,] tempState;
        if (piece = "Line") 
        {
            for(int i = 0; i < WIDTH; i++)
            {
                tempState = GetAll();
                startingLoc = CheckHeight(i); //will be implemented later
                tempState[i, startingLoc + 1] = true;
                tempState[i, startingLoc + 2] = true;
                tempState[i, startingLoc + 3] = true;
                tempState[i, startingLoc + 4] = true;
                PotentialStates.add(tempState);
                if (i + 4 < WIDTH) //Handles horizontal lines, makes sure it does not go over
                {
                    tempState = GetAll(); //resets tempState
                    startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1), CheckHeight(i + 2), CheckHeight(i + 3));
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
                tempState = GetAll();
                startingLoc = (CheckHeight(i) < CheckHeight(i + 1)) ? CheckHeight(i) : CheckHeight(i + 1) - 2;
                tempState[i, startingLoc + 1] = true;
                tempState[i, startingLoc + 2] = true;
                tempState[i, startingLoc + 3] = true;
                tempState[i + 1, startingLoc + 3] = true;
                PotentialStates.add(tempState);

                //flipped
                tempState = GetAll();
                startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1));
                tempState[i, startingLoc + 1] = true;
                tempState[i + 1, startingLoc + 1] = true;
                tempState[i + 1, startingLoc + 2] = true;
                tempState[i + 1, startingLoc + 3] = true;
                PotentialStates.add(tempState);

                //For the two horizontal orientations
                if (i + 3 < WIDTH)
                {
                    tempState = GetAll();
                    startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1), CheckHeight(i + 2));
                    tempState[i, startingLoc + 1] = true;
                    tempState[i, startingLoc + 2] = true;
                    tempState[i + 1, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc + 1] = true;
                    PotentialStates.add(tempState);

                    tempState = GetAll();
                    startingLoc = (CheckHeight(i) > Mathf.Max(CheckHeight(i + 1), CheckHeight(i+2))) ? CheckHeight(i) : Mathf.Max(CheckHeight(i + 1), CheckHeight(i+2)) + 1;
                    tempState[i, startingLoc + 1] = true;
                    tempState[i + 1, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc] = true;
                    PotentialStates.add(tempState);
                }
            }
        }
        else if (piece = "L") 
        {
            for(int i = 0; i < WIDTH - 1; i++)
            {
                tempState = GetAll();
                startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1));
                tempState[i, startingLoc + 1] = true;
                tempState[i, startingLoc + 2] = true;
                tempState[i, startingLoc + 3] = true;
                tempState[i + 1, startingLoc + 1] = true;
                PotentialStates.add(tempState);

                tempState = GetAll();
                startingLoc = (CheckHeight(i) > CheckHeight(i + 1)) ? CheckHeight(i) : CheckHeight(i + 1) + 2;
                tempState[i, startingLoc + 1] = true;
                tempState[i + 1, startingLoc + 1] = true;
                tempState[i + 1, startingLoc] = true;
                tempState[i + 1, startingLoc - 1] = true;
                PotentialStates.add(tempState);

                if(i + 2 < WIDTH)
                {
                    tempState = GetAll();
                    startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1), CheckHeight(i + 2));
                    tempState[i, startingLoc + 1] = true;
                    tempState[i + 1, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc + 1] = true;
                    tempState[i + 2, startingLoc + 2] = true;
                    PotentialStates.add(tempState);

                    tempState = GetAll();
                    startingLoc = (CheckHeight(i) < Mathf.Max(CheckHeight(i + 1), CheckHeight(i + 2))) ? CheckHeight(i) : Mathf.Max(CheckHeight(i + 1), CheckHeight(i + 2)) - 1;
                    tempState[i, startingLoc + 1] = true;
                    tempState[i, startingLoc + 2] = true;
                    tempState[i + 1, startingLoc + 2] = true;
                    tempState[i + 2, startingLoc + 2] = true;
                    PotentialStates.add(tempState);
                }
            }
        }
        else if (piece = "Square") 
        {
            for(int i = 0; i < WIDTH-1; i++)
            {
                tempState = GetAll();
                startingLoc = Mathf.Max(CheckHeight(i), CheckHeight(i + 1)); 
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
        // Checks if given x, y coordinates falls within bounds of array

        if (x < 0 || x > WIDTH || y < 0 || y > HEIGHT)
            return true;
        return false;
    }
    #endregion
}
