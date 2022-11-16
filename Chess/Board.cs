using System.Diagnostics;
using System.Numerics;

namespace Chess
{
	// A board state
	public partial class Board
	{
		public Tile Size { get; private set; }

		public Dictionary<Tile, PieceType> Pieces { get; private set; }

		public State state;

		/// <summary>Result of the current board. </summary>
		public enum State
		{
			/// <summary>On going</summary>
			OnGoing,
			/// <summary>3 times the same board</summary>
			ThreeFoldRepeat,
			/// <summary>50 times without moving a pawn or eating a piece</summary>
			FiftyRuleRepeat,
			/// <summary>No more move for the next player</summary>
			TieNoMove,
			/// <summary>Not enough pieces to do a check mate</summary>
			TieNoMatePossible,
			/// <summary>Check</summary>
			//Check,
			/// <summary>Checkmate</summary>
			Mate
		}

		public Board()
        {
			Size = Tile.One * 8;
			Pieces = new Dictionary<Tile, PieceType>();

			Default();
		}

		public void Default()
		{
			for (int x = 0; x < Size.X; x++)
				PlaceMirrored(new Tile(x, 1), PieceType.PAWN);

			PieceType[] order = { 
				PieceType.ROOK,
				PieceType.KNIGHT,
				PieceType.BISHOP,
				PieceType.QUEEN,
				PieceType.KING,
				PieceType.BISHOP,
				PieceType.KNIGHT,
				PieceType.ROOK
			};

			for (int x = 0; x < order.Length; x++)
				PlaceMirrored(new Tile(x, 0), order[x]);
        }

		public void Place(Tile tile, PieceType pieceType)
		{
			Debug.Assert(tile.X < Size.X, "Tile out of range (X)");
			Debug.Assert(tile.Y < Size.Y, "Tile out of range (Y)");

			Pieces.Add(tile, pieceType);
		}

		public void PlaceMirrored(Tile tile, PieceType type)
		{
			Place(tile, type | PieceType.WHITE);
			Tile mirrored = new Tile(tile.X, (Size.Y - 1) - tile.Y);;
			Place(mirrored, type | PieceType.BLACK);
		}

		//

		public List<Tile> GetPieceMoves(Tile tile, PieceType piece)
		{
			Movement movement = piece.GetMovement();

			List<Tile> movementTiles = movement.Get(tile, piece);

            // Whether the piece should reincure it's movement
            // Bishops, Rooks, Queens, Hawks ect
            if (!movement.IsRepeatable()) return movementTiles;
			
			List<Tile> tiles = new List<Tile>();

			foreach (Tile movementTile in movementTiles)
			{
				Tile targetTile = tile;
				while (targetTile.InRange(8))
                {
					tiles.Add(targetTile);

					PieceType targetPiece = GetPiece(targetTile);
					if (targetPiece.Is(Piece.))
                    {

                    }

					targetTile += movementTile;

                }
			}

			return tiles;
			
        }

		public PieceType GetPiece(Tile tile)
        {
            Pieces.TryGetValue(tile, out PieceType piece);
            return piece;
		}

		public bool Move(Tile from, Tile to, bool force = false)
        {
			PieceType piece = Pieces[from];

			List<Tile> fullMoves = piece.GetMovement().Get(from, piece);

			foreach (Tile move in fullMoves)
				Console.WriteLine(piece.Ide() + " move: " + move.X + ", " + move.Y);
            

			if (!fullMoves.Contains(to) && !force) return false;

			Pieces.Remove(from);
			Pieces[to] = piece;

			return true;
		}



		//

		public void DebugPrint()
		{
			for (int y = 0; y < Size.Y; y++)
			{
				for (int x = 0; x < Size.X; x++)
				{
					//Tile tile = new Tile(x, y);
					Tile tile = new Tile(x, Size.Y - y - 1);
					if (Pieces.ContainsKey(tile))
                    {
						PieceType piece = Pieces[tile];
						Console.Write(piece.Ide() + " ");
                    }
					else
						Console.Write("-- ");
				}
				Console.WriteLine();
			}
		}
	}

	partial class Board
	{
		public static readonly Tile a1 = new(0, 0);
		public static readonly Tile a2 = new(0, 1);
		public static readonly Tile a3 = new(0, 2);
		public static readonly Tile a4 = new(0, 3);
		public static readonly Tile a5 = new(0, 4);
		public static readonly Tile a6 = new(0, 5);
		public static readonly Tile a7 = new(0, 6);
		public static readonly Tile a8 = new(0, 7);

		public static readonly Tile b1 = new(1, 0);
		public static readonly Tile b2 = new(1, 1);
		public static readonly Tile b3 = new(1, 2);
		public static readonly Tile b4 = new(1, 3);
		public static readonly Tile b5 = new(1, 4);
		public static readonly Tile b6 = new(1, 5);
		public static readonly Tile b7 = new(1, 6);
		public static readonly Tile b8 = new(1, 7);

		public static readonly Tile c1 = new(2, 0);
		public static readonly Tile c2 = new(2, 1);
		public static readonly Tile c3 = new(2, 2);
		public static readonly Tile c4 = new(2, 3);
		public static readonly Tile c5 = new(2, 4);
		public static readonly Tile c6 = new(2, 5);
		public static readonly Tile c7 = new(2, 6);
		public static readonly Tile c8 = new(2, 7);

		public static readonly Tile d1 = new(3, 0);
		public static readonly Tile d2 = new(3, 1);
		public static readonly Tile d3 = new(3, 2);
		public static readonly Tile d4 = new(3, 3);
		public static readonly Tile d5 = new(3, 4);
		public static readonly Tile d6 = new(3, 5);
		public static readonly Tile d7 = new(3, 6);
		public static readonly Tile d8 = new(3, 7);
		
		public static readonly Tile e1 = new(4, 0);
		public static readonly Tile e2 = new(4, 1);
		public static readonly Tile e3 = new(4, 2);
		public static readonly Tile e4 = new(4, 3);
		public static readonly Tile e5 = new(4, 4);
		public static readonly Tile e6 = new(4, 5);
		public static readonly Tile e7 = new(4, 6);
		public static readonly Tile e8 = new(4, 7);

		public static readonly Tile f1 = new(5, 0);
		public static readonly Tile f2 = new(5, 1);
		public static readonly Tile f3 = new(5, 2);
		public static readonly Tile f4 = new(5, 3);
		public static readonly Tile f5 = new(5, 4);
		public static readonly Tile f6 = new(5, 5);
		public static readonly Tile f7 = new(5, 6);
		public static readonly Tile f8 = new(5, 7);

		public static readonly Tile g1 = new(6, 0);
		public static readonly Tile g2 = new(6, 1);
		public static readonly Tile g3 = new(6, 2);
		public static readonly Tile g4 = new(6, 3);
		public static readonly Tile g5 = new(6, 4);
		public static readonly Tile g6 = new(6, 5);
		public static readonly Tile g7 = new(6, 6);
		public static readonly Tile g8 = new(6, 7);

		public static readonly Tile h1 = new(7, 0);
		public static readonly Tile h2 = new(7, 1);
		public static readonly Tile h3 = new(7, 2);
		public static readonly Tile h4 = new(7, 3);
		public static readonly Tile h5 = new(7, 4);
		public static readonly Tile h6 = new(7, 5);
		public static readonly Tile h7 = new(7, 6);
		public static readonly Tile h8 = new(7, 7);
	}







	//Castling

	// Once per game, each king can make a move known as castling. Castling consists of moving the king two squares toward a rook of the same color on the same rank, and then placing the rook on the square that the king crossed.
	// - Neither the king nor the rook has previously moved during the game.
	// - There are no pieces between the king and the rook.
	// - The king is not in check and does not pass through or land on any square attacked by an enemy piece.
	// Castling is still permitted if the rook is under attack, or if the rook crosses an attacked square.

}