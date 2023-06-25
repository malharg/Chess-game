using UnityEngine;

namespace Chess.Scripts.Core {
    public class ChessInputHandler : MonoBehaviour {
        private Camera mainCamera;
        private ChessPlayerPlacementHandler selectedPiece;

        private void Awake() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                HandleInput();
            }
        }

        private void HandleInput() {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                ChessPlayerPlacementHandler piece = hit.transform.GetComponent<ChessPlayerPlacementHandler>();
                if (piece != null) {
                    Debug.Log("Chess piece clicked: " + piece.name);
                    SelectPiece(piece);
                }
            }
        }

        private void SelectPiece(ChessPlayerPlacementHandler piece) {
            if (selectedPiece != null) {
                selectedPiece.Deselect();
            }

            selectedPiece = piece;
            selectedPiece.Select();
        }
    }
}
