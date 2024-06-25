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
    private bool shuffling = false;

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

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];
                float columnsPropotion = 1f / columns;
                float rowPropotion = 1f / rows;
                uv[0] = new Vector2((columnsPropotion * col), 1 - ((rowPropotion * (row + 1))));
                uv[1] = new Vector2((columnsPropotion * (col + 1)), 1 - ((rowPropotion * (row + 1))));
                uv[2] = new Vector2((columnsPropotion * col), 1 - ((rowPropotion * row)));
                uv[3] = new Vector2((columnsPropotion * (col + 1)), 1 - ((rowPropotion * row)));
                mesh.uv = uv;

                if ((row == 0) && (col == 0))
                {
                    emptyLocation = 0;
                    piece.gameObject.SetActive(false);
                }
                   
 
            }
        }
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }


    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % columns) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Swap them in game state.
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            // Swap their transforms.
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            // Update empty location.
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private void MovePice(GameObject pice)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == pice.transform)
            {
                // Check each direction to see if valid move.
                // We break out on success so we don't carry on and swap back again.
                if (SwapIfValid(i, -columns, columns)) { break; }
                if (SwapIfValid(i, +columns, columns)) { break; }
                if (SwapIfValid(i, -1, 0)) { break; }
                if (SwapIfValid(i, +1, columns - 1)) { break; }
            }
        }
        if (CheckCompletion())
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                pieces[i].GetComponent<Collider2D>().enabled = false;
                pieces[i].gameObject.SetActive(true);
                pieces[i].transform.Find("Frame").gameObject.SetActive(false);
            }
        }
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (columns * columns * rows))
        {
            // Pick a random location.
            int rnd = Random.Range(0, columns * rows);
            // Only thing we forbid is undoing the last move.
            if (rnd == last) { continue; }
            last = emptyLocation;
            // Try surrounding spaces looking for valid move.
            if (SwapIfValid(rnd, -columns, columns))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +columns, columns))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, columns - 1))
            {
                count++;
            }
        }
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    void Start()
    {
        pieces = new List<Transform>();
        columns = 2;
        rows = 3;
        CreateGamePieces();
        shuffling = true;
        StartCoroutine(WaitShuffle(0.5f));
        GameEvents.current.onGamePiceClick += MovePice;
    }
}
