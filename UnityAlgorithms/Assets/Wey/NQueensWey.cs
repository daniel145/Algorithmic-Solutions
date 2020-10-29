using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NQueensWey : MonoBehaviour
{
    #region Structs
    //The Coord struct is a simple data structure for holding a pair of (row, col) values.
    private struct Coord {
        private int x, y;

        public Coord(int xCoord, int yCoord) {
            x = xCoord;
            y = yCoord;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
    }

    //The Queen struct is simply an object that contains a Coordinate value. 
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
    private List<Queen> queens;

    //The 2D array responsible for keeping track of where queens are on the board
    private bool[,] boardState;

    //The integer value corresponding to the dimension of the chess board
    private int boardLength;

    //The integer value corresponding to the column of the queen placed manually by the user.
    private int startingCol;
    #endregion

    #region Primary Functions

    // Start is called before the first frame update
    private void Start() {
        BoardSetUp(8, 2, 4);
    }

    //The main function for initiating the N Queens Algorithm
    public bool[,] BoardSetUp(int length, int xCoord, int yCoord) {
        queens = new List<Queen>();
        boardLength = length;
        boardState = new bool[8, 8];

        if (xCoord < 0 || xCoord >= length || yCoord < 0 || yCoord >= length) {
            throw new Exception("Initial queen placement is invalid. Queen is trying to be placed at " + xCoord + " " + yCoord);
        }

        startingCol = xCoord;
        Coord newCoordinate = new Coord(xCoord, yCoord);
        PlaceQueen(newCoordinate);
        return boardState;

        //if (Solver(0)) {
        //    Debug.Log("success!");
        //}
        //else {
        //    Debug.Log("boo");
        //}

        //bool[,] queenDisplay = new bool[8, 8];
        //foreach (Queen q in queens) {
        //    queenDisplay[q.Coordinate.X, q.Coordinate.Y] = true;
        //}
        //Print2DArray(queenDisplay);
    }

    //The recursive solving method for the backtracking N Queens algorithm 
    private bool Solver(int column) {
        if (column >= boardLength) return true;

        if (column == startingCol) {
            return Solver(column + 1);
        }

        for (int i = 0; i < boardLength; i++) {
            if (isLegal(column, i)) {
                Coord curr = new Coord(column, i);
                PlaceQueen(curr);
                if (Solver(column + 1) == true) {
                    return true;
                }
                else {
                    RemoveQueen();
                }
            }
        }
        return false;
    }

    //The helper function to check if a space on the chess board is not under attack from any other queen
    private bool isLegal(int row, int col) {
        for (int i = 0; i < boardLength; i++) {
            if (boardState[i, col] == true) {
                return false;
            }
        }

        foreach (Queen q in queens) {
            int deltaRow = Mathf.Abs(q.Coordinate.X - row);
            int deltaCol = Mathf.Abs(q.Coordinate.Y - col);
            if (deltaRow == deltaCol) return false;
        }

        return true;
    }

    #endregion

    #region Queen Utilities
    //The function that creates and then adds a queen onto the board.
    private void PlaceQueen(Coord coordinate) {
        Queen newQueen = new Queen(coordinate);
        queens.Add(newQueen);
        boardState[coordinate.X, coordinate.Y] = true;
    }

    //The function that removes a queen from the board.
    private void RemoveQueen() {
        Queen removedQueen = queens[queens.Count - 1];
        Coord removedCoord = removedQueen.Coordinate;
        boardState[removedCoord.X, removedCoord.Y] = false;
        queens.RemoveAt(queens.Count - 1);
    }

    #endregion

    #region Utility Functions
    //A utility function for printing out the board state 2D array in the Unity Debug Console
    public static void Print2DArray(bool[,] matrix) {
        string arrayString = "";
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                arrayString += string.Format("{0} ", matrix[i, j]);
            }
            arrayString += System.Environment.NewLine + System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
    #endregion
}
