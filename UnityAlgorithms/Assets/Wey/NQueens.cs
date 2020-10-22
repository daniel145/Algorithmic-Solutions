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
    #endregion


    public void BoardSetUp(int length, int xCoord, int yCoord) {
        queens = new Stack((int)length);
        illegalSpaces = new int[length, length];
        boardLength = length;

        if (xCoord < 0 || xCoord >= length || yCoord < 0 || yCoord >= length) {
            throw new Exception("Initial queen placement is invalid. Queen is trying to be placed at " + xCoord + " " + yCoord);
        }

        Coord newCoordinate = new Coord(xCoord, yCoord);
        PlaceQueen(newCoordinate);

        BeginBranchBound();
    }

    private void BeginBranchBound() { 
        //while (queens.Count )
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
    private void RemoveQueen() {
        Queen removedQueen = (Queen)queens.Pop();
        Coord removedCoord = removedQueen.Coordinate;
        RemoveRowsCols(removedCoord);
        RemoveDiagonals(removedCoord);
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

}
