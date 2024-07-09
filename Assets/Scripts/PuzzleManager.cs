using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Transform piecePrefab;
    [SerializeField] public int id;

    private List<Transform> pieces;
    private int emptyLocation;
    private int columns;
    private int rows;
    private PuzzleConfig data;
    private Material material;
    private static AudioSource audioSource;

    private void CreateGamePieces()
    {   
        material = Resources.Load("Materials/" + data.materialName + "Material", typeof(Material)) as Material;
        if (GameConfig.mode == "easy") {
            rows = data.easyRows;
            columns = data.easyColumns;
        }
        if (GameConfig.mode == "hard")
        {
            rows = data.hardRows;
            columns = data.hardColumns;
        }

        float scale = Mathf.Min(gameObject.GetComponent<BoxCollider2D>().size.x / columns, gameObject.GetComponent<BoxCollider2D>().size.y / rows);
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameObject.transform);
                pieces.Add(piece);
                
                piece.localPosition = new Vector3((col - (columns - 1) / 2f) * scale, (-row + (rows - 1) / 2f) * scale, -0.2f);
                piece.localScale = Vector3.one * scale;
                piece.name = $"{(row * columns) + col}";

                piece.GetComponent<MeshRenderer>().material = material;

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
        if (pieces.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }


    private bool SwapIfValid(int i, int offset, int colCheck, bool animate = false)
    {
        if (((i % columns) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Swap them in game state.
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            // Swap their transforms.
            if (!animate)
            {
                (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            }
            else {
                Vector3 target = pieces[i].localPosition;
                pieces[i].localPosition = pieces[i + offset].localPosition;
                LeanTween.moveLocal(pieces[i + offset].gameObject, target, 0.2f);
            }
            // Update empty location.
            emptyLocation = i;
            return true;
        }
        return false;
    }

    private void ShowPicture()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].GetComponent<Collider2D>().enabled = false;
            pieces[i].gameObject.SetActive(true);
            pieces[i].transform.Find("Frame").gameObject.SetActive(false);
        }
    }

    private void MovePice(GameObject pice)
    {
        if (pieces.Contains(pice.transform))
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == pice.transform)
                {
                    // Check each direction to see if valid move.
                    // We break out on success so we don't carry on and swap back again.
                    if (SwapIfValid(i, -columns, columns, true)) { audioSource.Play(); break; }
                    if (SwapIfValid(i, +columns, columns, true)) { audioSource.Play(); break; }
                    if (SwapIfValid(i, -1, 0, true)) { audioSource.Play(); break; }
                    if (SwapIfValid(i, +1, columns - 1, true)) { audioSource.Play(); break; }
                    
                }
            }
            if (CheckCompletion())
            {
                GameConfig.SetAsDone(id);
                GameEvents.current.PuzzleDone();
                ShowPicture();
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
    }

    private void StartPuzzle()
    {
        if (GameConfig.activePuzzle == id)
        {
            CreateGamePieces();
            StartCoroutine(WaitShuffle(0.5f));
        }
    }

    private void ClosePuzzle()
    {
        if (CheckCompletion()) return;
        ResetPuzzle();
    }

    private void CreateAsDone()
    {
        CreateGamePieces();
        ShowPicture();
    }

    private void ResetPuzzle()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform pice = gameObject.transform.GetChild(i);
            Destroy(pice.gameObject);
        }
        pieces.Clear();
    }

    void Start()
    {
        GameEvents.current.onModeSelected += StartPuzzle;
        GameEvents.current.onBackButtonClick += ClosePuzzle;
        GameEvents.current.onGamePiceClick += MovePice;
        GameEvents.current.onResetButtonClick += ResetPuzzle;
        audioSource = GetComponent<AudioSource>();
        data = GameConfig.GetPuzzleConfig(id);
        pieces = new List<Transform>();
        if (data.done) CreateAsDone();
    }
}
