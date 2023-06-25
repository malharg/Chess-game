using System;
using UnityEngine;

namespace Chess.Scripts.Core {
    public class ChessPlayerPlacementHandler : MonoBehaviour {
        [SerializeField] private int row, column;
        [SerializeField] private bool isEnemyPiece;
         private Material defaultMaterial;
        private Material selectedMaterial;
        private void Start() {
            transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, column).transform.position;

            // Highlight possible moves based on the piece type
            HighlightPossibleMoves();
            defaultMaterial = GetComponent<Renderer>().material;
            selectedMaterial = Resources.Load<Material>("SelectedMaterial");
        }
        public void Select() {
            GetComponent<Renderer>().material = selectedMaterial;
            Debug.Log("Piece selected: " + name);
            HighlightPossibleMoves();
        }

        public void Deselect() {
            GetComponent<Renderer>().material = defaultMaterial;
        }
        private void HighlightPossibleMoves() {
            ChessBoardPlacementHandler.Instance.ClearHighlights();

            // Call the corresponding highlighting methods for each piece
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForPawn(row, column, isEnemyPiece);
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForRook(row, column, isEnemyPiece);
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForKnight(row, column, isEnemyPiece);
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForBishop(row, column, isEnemyPiece);
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForQueen(row, column, isEnemyPiece);
            ChessBoardPlacementHandler.Instance.HighlightPossibleMovesForKing(row, column, isEnemyPiece);
        }
    }
}
