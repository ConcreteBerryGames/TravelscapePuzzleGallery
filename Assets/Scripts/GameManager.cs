using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private List<Transform> pieces;
    private int emptyLocation;
    private int columns;
    private int rows;

    private void CreateGamePieces()
    {
        // This is the width of each tile.
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                piece.localPosition = new Vector3(col, -row, 0);
                piece.name = $"{(row * columns) + col}";
                // We want an empty space in the bottom right.
                if ((row == 0) && (col == 0))
                {
                    emptyLocation = 0;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                   
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    float columnsPropotion = 1f / columns;
                    float rowPropotion = 1f / rows;
                    uv[0] = new Vector2((columnsPropotion * col), 1 - ((rowPropotion * (row + 1))));
                    uv[1] = new Vector2((columnsPropotion * (col + 1)), 1 - ((rowPropotion * (row + 1))));
                    uv[2] = new Vector2((columnsPropotion * col), 1 - ((rowPropotion * row)));
                    uv[3] = new Vector2((columnsPropotion * (col + 1)), 1 - ((rowPropotion * row)));
                    mesh.uv = uv;
                }
            }
        }
    }


    void Start()
    {
        pieces = new List<Transform>();
        columns = 3;
        rows = 4;
        CreateGamePieces();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                Debug.Log(hit.transform.position);
            }
        }
    }
}
