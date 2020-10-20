using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    #region Variables
    public uint size = 8;
    public uint scale = 1;
    public Material mat;

 
    private Color[] colorScheme;
    private GameObject board;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CreateDesign();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateDesign()
    {
        // Initialize chess board color scheme
        colorScheme = new Color[6];
        colorScheme[0] = Color.black;
        colorScheme[1] = new Color(88, 67, 67, 1);
        colorScheme[2] = new Color(157, 127, 119);
        colorScheme[3] = new Color(212, 201, 197);
        colorScheme[4] = new Color(229, 219, 216);
        colorScheme[5] = Color.white;

        var texture = new Texture2D(2, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, colorScheme[2]);
        texture.SetPixel(0, 1, colorScheme[3]);
        texture.Apply();

        board = new GameObject("Chessboard");

        for (int i = 0; i < size; i++)
        {
            GameObject row = GameObject.CreatePrimitive(PrimitiveType.Plane);
            row.name = "Row " + i.ToString();
            row.transform.localScale = new Vector3(0.25f * scale, 1, 0.25f * (size + 1) * scale);

            MeshRenderer render = row.GetComponent<MeshRenderer>();
            render.material = mat;
            render.material.mainTextureScale = new Vector2(1, 0.5f * (size + 1));

            row.transform.position = new Vector3(2.5f * i * scale, 0, scale * ((size + 1) - 2.5f * (i % 2)));
            row.transform.parent = board.transform;
        }
    }
}
