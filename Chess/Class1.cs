using System.Numerics;

namespace Chess
{
	public enum Set
	{
		WHITE, BLACK
	}

	public enum Piece
	{
		PAWN = 1,
		KNIGHT = 3,
		BISHOP = 3,
		ROOK = 5,
		QUEEN = 9,
		KING = 0
	}

	// Movement
	public struct Movement
	{
		public const int
			Forward = 1, // Pawn
			L = 2, // Knight
			RankFile = 4, // Rook, Queen
			Diagonal = 8, // Bishop, Queen
			Surround = 16; // King

		public Movement()
		{

		}

		// Return all the possible movement, unblocked and unhindered by check
		public List<Vector2> GetMovement(Vector2 position)
		{
			List<Vector2> movement = new List<Vector2>();
			return movement;
		}

		public int PieceMovement(Piece piece)
		{
			int value = 0;



			return value;
		}
	}

	// A board state
	public struct Board
	{

	}







	//Castling

	// Once per game, each king can make a move known as castling. Castling consists of moving the king two squares toward a rook of the same color on the same rank, and then placing the rook on the square that the king crossed.
	// - Neither the king nor the rook has previously moved during the game.
	// - There are no pieces between the king and the rook.
	// - The king is not in check and does not pass through or land on any square attacked by an enemy piece.
	// Castling is still permitted if the rook is under attack, or if the rook crosses an attacked square.

}