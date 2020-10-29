using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlowTiles : MonoBehaviour
{
    #region Public Variables
    public uint size = 8;
    public Tile[] tiles;
    public NQueensWey solver;
    public GameObject queenPrefab;
    #endregion

    #region Private
    private bool glow = true;
    private Tilemap tilemap;
    private Vector3Int pastTile;
    private GameObject queensFolder;
    private List<GameObject> queenList;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        queensFolder = new GameObject("QueensFolder");
        queenList = new List<GameObject>();
    }

    // If glow is enabled, then give the tile being moused over a yellow background.
    void Update()
    {
        if (glow)
        {
            // Point ray to board to find selection
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit);
            if (hit.collider == null)
            {
                ConvertBack();
                pastTile = Vector3Int.one;
                return;
            }

            // Avoid tile positions outside of chess board
            Vector3Int tilePosition = tilemap.WorldToCell(hit.point);
            if (!CheckBounds(tilePosition))
                return;
            else if (Input.GetMouseButtonDown(0))
            {
                glow = false;
                ConvertBack();
                bool[,] queenArray = solver.BoardSetUp((int)size, tilePosition.x + 1, tilePosition.y + 1);
                PlaceQueens(queenArray);
            }
            else if (tilePosition != pastTile)
            {
                ConvertBack();
                tilemap.SetTile(tilePosition, tiles[Mathf.Abs(tilePosition.x + tilePosition.y) % 2 + 2]);
                pastTile = tilePosition;
            }
        }
    }

    #region Public Functions
    // Adds a queen to board.
    public void AddQueen(int x, int y)
    {
        GameObject obj = Instantiate(queenPrefab);
        obj.transform.position = new Vector3(x * 2, 0, y * 2);
        obj.transform.parent = queensFolder.transform;
        queenList.Add(obj);
    }

    public void RemoveQueen()
    {
        int index = queenList.Count - 1;
        GameObject queen = queenList[index];
        queenList.Remove(queen);
        Destroy(queen);
    }
    #endregion

    #region Private Functions
    private void ConvertBack()
    {
        if (pastTile != Vector3Int.one)
            tilemap.SetTile(pastTile, tiles[Mathf.Abs(pastTile.x + pastTile.y) % 2]);
    }

    private bool CheckBounds(Vector3Int position)
    {
        if (position.x < -1 || position.x > 6 || position.y < -1 || position.y > 6)
        {
            Debug.Log("Tile Position out of range: " + position);
            return false;
        }
        return true;
    }

    private void PlaceQueens(bool[,] array)
    {
        // Check if an answer was returned
        if (array == null)
        {
            Debug.Log("Array was null");
            return;
        }

        // Remove any queens currently on the board
        while (queenList.Count > 0)
        {
            int index = queenList.Count - 1;
            queenList.Remove(queenList[index]);
            Destroy(queenList[index]);
        }

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                if (array[i, j])
                {
                    GameObject queen = Instantiate(queenPrefab);
                    queen.transform.parent = queensFolder.transform;
                    queen.transform.position = new Vector3(i * 2, 0, j * 2);
                }
    }
    #endregion
}
