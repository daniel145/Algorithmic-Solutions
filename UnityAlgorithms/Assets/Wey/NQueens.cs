using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NQueens : MonoBehaviour
{
    #region Structs
    private struct Coord {
        private int x, y;

        public Coord(int xCoord, int yCoord) {
            x = xCoord;
            y = yCoord;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
    }

    private struct Queen {
        private Coord coordinate;

        public Queen(Coord coord) {
            coordinate = coord;
        }

        public Coord Coordinate { get { return coordinate; } set { coordinate = value; } }
    }
    #endregion

    #region Variables
    //The data structure responsible for keeping track of queen placement
    Stack queens;

    int[,] illegalSpaces;

    int boardLength;
    int startingCol;


    #endregion

    private void Start() {
        BoardSetUp(8, 0, 7);
    }

    public void BoardSetUp(int length, int xCoord, int yCoord) {
        queens = new Stack((int)length);
        illegalSpaces = new int[length, length];
        boardLength = length;

        if (xCoord < 0 || xCoord >= length || yCoord < 0 || yCoord >= length) {
            throw new Exception("Initial queen placement is invalid. Queen is trying to be placed at " + xCoord + " " + yCoord);
        }

        startingCol = xCoord;
        Coord newCoordinate = new Coord(xCoord, yCoord);
        PlaceQueen(newCoordinate);

        QueenAdder();

        bool[,] queenDisplay = new bool[8, 8];
        while (queens.Count > 0) {
            Queen q = (Queen)queens.Pop();
            Coord coord = q.Coordinate;
            queenDisplay[coord.X, coord.Y] = true;
        }
        Print2DArray(queenDisplay);

    }

    private void QueenAdder() {
        bool placedQueen = false;
        //curr = 6, 4
        Coord curr = new Coord(0, 0);
        int counter = 0;
        for (int i = 0; i < boardLength; i++) {
            if (startingCol == i) continue;
            if (counter >= 5000) {
                Debug.Log("ya dun fked up");
                break;
            }
     
            placedQueen = false;
            for (int j = 0; j < boardLength; j++) {
                if (illegalSpaces[i, j] == 0) {
                    curr = new Coord(i, j);
                    PlaceQueen(curr);
                    placedQueen = true;
                    break;
                }
            }
            if (!placedQueen) {
                i -= Backtracker(curr);
            }
            counter++;
        }
    }

    private int Backtracker(Coord c) {
        int backsteps = 1;
        RemoveQueen();
        bool placedQueen = false;
        for (int i = c.Y + 1; i < boardLength; i++) {
            placedQueen = false;
            //remove queen
            //place in next spot
            //if no next spot, go 1 further back
            if (illegalSpaces[c.X, i] == 0) {
                PlaceQueen(new Coord(c.X, i));
                placedQueen = true;
                break;
            }
        }
        if (!placedQueen) {
            //backsteps++;
            Queen temp = (Queen)queens.Peek();
            return backsteps + Backtracker(temp.Coordinate);
        }
        return backsteps;
    }


    #region Queen Logistics
    private void PlaceQueen(Coord coordinate) {
        Queen newQueen = new Queen(coordinate);
        queens.Push(newQueen);

        AddRowsCols(coordinate);
        AddDiagonals(coordinate);
    }
    private void AddRowsCols(Coord coordinate) {
        for (int i = 0; i < boardLength; i++) {
            illegalSpaces[coordinate.X, i]++;
            illegalSpaces[i, coordinate.Y]++;
        }
    }
    private void AddDiagonals(Coord coordinate) {
        int indexLeft = Mathf.Min(coordinate.X, coordinate.Y);
        int indexRight = Mathf.Min(boardLength - coordinate.X, boardLength - coordinate.Y);

        Coord botLeft = new Coord(coordinate.X - indexLeft, coordinate.Y - indexLeft);
        Coord topRight = new Coord(coordinate.X + indexRight, coordinate.Y + indexRight);
        Coord topLeft = new Coord(coordinate.X - indexLeft, coordinate.Y + indexLeft);
        Coord botRight = new Coord(coordinate.X + indexRight, coordinate.Y - indexRight);

        int iterationsUp = topRight.X - botLeft.X;
        for (int i = 0; i < iterationsUp; i++) {
            illegalSpaces[botLeft.X + i, botLeft.Y + i]++;
        }

        int iterationsDown = topLeft.X - botRight.X;
        for (int i = 0; i < iterationsDown; i++) {
            illegalSpaces[topLeft.X + i, topLeft.Y - i]++;
        }
    }
    private Queen RemoveQueen() {
        Queen removedQueen = (Queen)queens.Pop();
        Coord removedCoord = removedQueen.Coordinate;
        RemoveRowsCols(removedCoord);
        RemoveDiagonals(removedCoord);
        return removedQueen;
    }
    private void RemoveRowsCols(Coord coordinate) {
        for (int i = 0; i < boardLength; i++) {
            illegalSpaces[coordinate.X, i]--;
            illegalSpaces[i, coordinate.Y]--;
        }
    }
    private void RemoveDiagonals(Coord coordinate) {
        int indexLeft = Mathf.Min(coordinate.X, coordinate.Y);
        int indexRight = Mathf.Min(boardLength - coordinate.X, boardLength - coordinate.Y);

        Coord botLeft = new Coord(coordinate.X - indexLeft, coordinate.Y - indexLeft);
        Coord topRight = new Coord(coordinate.X + indexRight, coordinate.Y + indexRight);
        Coord topLeft = new Coord(coordinate.X - indexLeft, coordinate.Y + indexLeft);
        Coord botRight = new Coord(coordinate.X + indexRight, coordinate.Y - indexRight);

        int iterationsUp = topRight.X - botLeft.X;
        for (int i = 0; i < iterationsUp; i++) {
            illegalSpaces[botLeft.X + i, botLeft.Y + i]--;
        }

        int iterationsDown = topLeft.X - botRight.X;
        for (int i = 0; i < iterationsDown; i++) {
            illegalSpaces[topLeft.X + i, topLeft.Y - i]--;
        }
    }
    #endregion


    public static void Print2DArray(bool[,] matrix) {
        Debug.Log("reached");
        string arrayString = "";
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                arrayString += string.Format("{0} ", matrix[i, j]);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
}
