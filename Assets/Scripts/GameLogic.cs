using Assets.Scripts.Eventing;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Inject]
    private EventBroker _eventBroker;

    private const int _rows = 6;
    private const int _columns = 7;

    private int[,] _board = new int[_rows, _columns];

    // Player IDs (1 for Player 1, 2 for Player 2)
    private int _currentPlayer = 1;

    private void Start()
    {
        ResetBoard();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for player input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var column = Random.Range(0, 6);
            DropPiece(column);
        }
    }

    // Resets the board for a new game
    private void ResetBoard()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                _board[row, col] = 0;
            }
        }
    }

    // Drops a piece into the specified column
    private bool DropPiece(int column)
    {
        // Check if the column is full
        if (_board[0, column] != 0)
        {
            Debug.Log("Column is full!");
            return false;
        }

        // Find the lowest empty row in the column
        for (int row = _rows - 1; row >= 0; row--)
        {
            if (_board[row, column] == 0)
            {
                _board[row, column] = _currentPlayer;
                Debug.Log($"Player {_currentPlayer} dropped piece in column: {column}");
                // Check if this move resulted in a win
                if (CheckWin(row, column))
                {
                    Debug.Log("Player " + _currentPlayer + " wins!");
                }
                else
                {
                    // Switch to the other player
                    _currentPlayer = (_currentPlayer == 1) ? 2 : 1;
                }
                return true;
            }
        }

        return false;
    }

    private bool CheckWin(int row, int col)
    {
        return CheckDirection(row, col, 1, 0) ||  // Horizontal
               CheckDirection(row, col, 0, 1) ||  // Vertical
               CheckDirection(row, col, 1, 1) ||  // Diagonal /
               CheckDirection(row, col, 1, -1);   // Diagonal \
    }

    // Check for four in a row in a specific direction
    private bool CheckDirection(int startRow, int startCol, int rowDir, int colDir)
    {
        int count = 1;
        count += CountPieces(startRow, startCol, rowDir, colDir);  // Check forward
        count += CountPieces(startRow, startCol, -rowDir, -colDir); // Check backward

        return count >= 4;
    }

    // Count the number of consecutive pieces for the current player in one direction
    private int CountPieces(int startRow, int startCol, int rowDir, int colDir)
    {
        int count = 0;
        int player = _board[startRow, startCol];

        int row = startRow + rowDir;
        int col = startCol + colDir;

        while (row >= 0 && row < _rows && col >= 0 && col < _columns && _board[row, col] == player)
        {
            count++;
            row += rowDir;
            col += colDir;
        }

        return count;
    }
}