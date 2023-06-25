using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour {
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _takeHighlightPrefab;
    private GameObject[,] _chessBoard;

    internal static ChessBoardPlacementHandler Instance;

    private void Awake() {
        Instance = this;
        GenerateArray();
    }

    private void GenerateArray() {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    internal GameObject GetTile(int i, int j) {
        try {
            return _chessBoard[i, j];
        } catch (Exception) {
            Debug.LogError("Invalid row or column.");
            return null;
        }
    }

    internal void Highlight(int row, int col) {
        var tile = GetTile(row, col).transform;
        if (tile == null) {
            Debug.LogError("Invalid row or column.");
            return;
        }

        Instantiate(_highlightPrefab, tile.transform.position, Quaternion.identity, tile.transform);
        Debug.Log("Highlighting cell: " + row + ", " + col);
    }

    internal void HighlightTake(int row, int col) {
        var tile = GetTile(row, col).transform;
        if (tile == null) {
            Debug.LogError("Invalid row or column.");
            return;
        }

        Instantiate(_takeHighlightPrefab, tile.transform.position, Quaternion.identity, tile.transform);
    }

    internal void ClearHighlights() {
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++) {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;
                foreach (Transform childTransform in tile.transform) {
                    Destroy(childTransform.gameObject);
                }
            }
        }
    }

    internal void HighlightPossibleMovesForPawn(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        // Calculate the forward move
        int forwardRow = isEnemyPiece ? row - 1 : row + 1;
        if (IsTileInBounds(forwardRow, col) && !IsOccupiedByPiece(forwardRow, col)) {
            Highlight(forwardRow, col);
        }

        // Calculate the diagonal captures
        int leftDiagonalRow = isEnemyPiece ? row - 1 : row + 1;
        int leftDiagonalCol = col - 1;
        if (IsTileInBounds(leftDiagonalRow, leftDiagonalCol) && IsEnemyPiece(leftDiagonalRow, leftDiagonalCol)) {
            HighlightTake(leftDiagonalRow, leftDiagonalCol);
        }

        int rightDiagonalRow = isEnemyPiece ? row - 1 : row + 1;
        int rightDiagonalCol = col + 1;
        if (IsTileInBounds(rightDiagonalRow, rightDiagonalCol) && IsEnemyPiece(rightDiagonalRow, rightDiagonalCol)) {
            HighlightTake(rightDiagonalRow, rightDiagonalCol);
        }
    }

    internal void HighlightPossibleMovesForRook(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        // Highlight all possible moves horizontally
        for (int i = row + 1; i < 8; i++) {
            if (IsOccupiedByPiece(i, col)) {
                if (IsEnemyPiece(i, col) != isEnemyPiece) {
                    HighlightTake(i, col);
                }
                break;
            }
            Highlight(i, col);
        }
        for (int i = row - 1; i >= 0; i--) {
            if (IsOccupiedByPiece(i, col)) {
                if (IsEnemyPiece(i, col) != isEnemyPiece) {
                    HighlightTake(i, col);
                }
                break;
            }
            Highlight(i, col);
        }

        // Highlight all possible moves vertically
        for (int j = col + 1; j < 8; j++) {
            if (IsOccupiedByPiece(row, j)) {
                if (IsEnemyPiece(row, j) != isEnemyPiece) {
                    HighlightTake(row, j);
                }
                break;
            }
            Highlight(row, j);
        }
        for (int j = col - 1; j >= 0; j--) {
            if (IsOccupiedByPiece(row, j)) {
                if (IsEnemyPiece(row, j) != isEnemyPiece) {
                    HighlightTake(row, j);
                }
                break;
            }
            Highlight(row, j);
        }
    }

    internal void HighlightPossibleMovesForKnight(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        int[] knightRowOffsets = { 2, 2, 1, 1, -1, -1, -2, -2 };
        int[] knightColOffsets = { 1, -1, 2, -2, 2, -2, 1, -1 };

        for (int i = 0; i < knightRowOffsets.Length; i++) {
            int newRow = row + knightRowOffsets[i];
            int newCol = col + knightColOffsets[i];

            if (IsTileInBounds(newRow, newCol)) {
                if (IsOccupiedByPiece(newRow, newCol)) {
                    if (IsEnemyPiece(newRow, newCol) != isEnemyPiece) {
                        HighlightTake(newRow, newCol);
                    }
                }
                else {
                    Highlight(newRow, newCol);
                }
            }
        }
    }

    internal void HighlightPossibleMovesForBishop(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        // Highlight all possible moves in diagonal directions
        for (int i = row + 1, j = col + 1; i < 8 && j < 8; i++, j++) {
            if (IsOccupiedByPiece(i, j)) {
                if (IsEnemyPiece(i, j) != isEnemyPiece) {
                    HighlightTake(i, j);
                }
                break;
            }
            Highlight(i, j);
        }
        for (int i = row + 1, j = col - 1; i < 8 && j >= 0; i++, j--) {
            if (IsOccupiedByPiece(i, j)) {
                if (IsEnemyPiece(i, j) != isEnemyPiece) {
                    HighlightTake(i, j);
                }
                break;
            }
            Highlight(i, j);
        }
        for (int i = row - 1, j = col + 1; i >= 0 && j < 8; i--, j++) {
            if (IsOccupiedByPiece(i, j)) {
                if (IsEnemyPiece(i, j) != isEnemyPiece) {
                    HighlightTake(i, j);
                }
                break;
            }
            Highlight(i, j);
        }
        for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--) {
            if (IsOccupiedByPiece(i, j)) {
                if (IsEnemyPiece(i, j) != isEnemyPiece) {
                    HighlightTake(i, j);
                }
                break;
            }
            Highlight(i, j);
        }
    }

    internal void HighlightPossibleMovesForQueen(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        // Highlight possible moves for a queen as a combination of rook and bishop moves
        HighlightPossibleMovesForRook(row, col, isEnemyPiece);
        HighlightPossibleMovesForBishop(row, col, isEnemyPiece);
    }

    internal void HighlightPossibleMovesForKing(int row, int col, bool isEnemyPiece) {
        // Clear previous highlights
        ClearHighlights();

        int[] kingRowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] kingColOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < kingRowOffsets.Length; i++) {
            int newRow = row + kingRowOffsets[i];
            int newCol = col + kingColOffsets[i];

            if (IsTileInBounds(newRow, newCol)) {
                if (IsOccupiedByPiece(newRow, newCol)) {
                    if (IsEnemyPiece(newRow, newCol) != isEnemyPiece) {
                        HighlightTake(newRow, newCol);
                    }
                }
                else {
                    Highlight(newRow, newCol);
                }
            }
        }
    }

    private bool IsTileInBounds(int row, int col) {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    private bool IsOccupiedByPiece(int row, int col) {
        return GetTile(row, col).transform.childCount > 0;
    }

    private bool IsEnemyPiece(int row, int col) {
        var tile = GetTile(row, col).transform;
        if (tile.childCount <= 0) return false;

        var childSpriteRenderer = tile.GetChild(0).GetComponent<SpriteRenderer>();
        if (childSpriteRenderer != null && childSpriteRenderer.color == Color.red) {
            return true;
        }

        return false;
    }
}
