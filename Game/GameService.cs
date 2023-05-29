using System.Collections.Generic;
using System.Linq;
using tabuleiro;
using ConsoleChessGame.Board.Exception;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game
{
    static class GameService
    {
        public static BoardService Board { get; private set; }
        public static Color CurrentPlayerColor { get; private set; }
        public static int Turn { get; private set; }
        public static bool FinishedGame { get; set; }
        public static bool IsCheck { get; set; }
        public static Piece VulnerablePieceForEnPassant;

        private static HashSet<Piece> Pieces;
        private static HashSet<Piece> CapturedPieces;

        static GameService()
        {
            Board = new BoardService(8, 8);
            CurrentPlayerColor = Color.White;
            Turn = 1;
            FinishedGame = false;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            VulnerablePieceForEnPassant = null;

            InitializePieces();
        }

        /// <summary>
        /// Realiza incrementação na propriedade <see langword="Turn"/>
        /// </summary>
        private static void PassTurn()
        {
            Turn++;
        }

        /// <summary>
        /// Verifica se a posição de origem informada pelo jogador é válida
        /// </summary>
        /// <param name="position">Posição no tabuleiro informada pelo jogador</param>
        public static void ValidateOriginPosition(PositionOnBoard position)
        {
            var piece = Board.GetPiece(position);

            if (piece is null)
                throw new ExceptionBoard("A posição escolhida não corresponde a nenhuma peça!");

            if (piece.Color != CurrentPlayerColor)
                throw new ExceptionBoard("A peça de origem escolhida não é sua!");

            if (!piece.ExistsPossibleMoves()) 
                throw new ExceptionBoard("Não existe movimentos possíveis para a peça de origem escolhida!");
        }

        /// <summary>
        /// Retorna a cor adversária com base na cor enviada por parâmetro
        /// </summary>
        /// <param name="color">Cor que deseja saber a adversária</param>
        private static Color GetOpponentColor(Color color)
        {
            var colorReturn = Color.White;

            if (color == Color.White)
                colorReturn = Color.Red;

            return colorReturn;
        }

        /// <summary>
        /// Verifica se a posição de destino escolhida para uma peça é válida
        /// </summary>
        /// <param name="originPosition">Posição de origem da peça</param>
        /// <param name="destinyPosition">Posição de destino da peça</param>
        public static void ValidateDestinationPosition(PositionOnBoard originPosition, PositionOnBoard destinyPosition)
        {
            var piece = Board.GetPiece(originPosition);

            if (!piece.CanMoveTo(destinyPosition))
            {
                throw new ExceptionBoard("Não é possível mover a peça para a posição informada.");
            }
        }

        /// <summary>
        /// Obtem as peças capturadas na partida de uma determinada cor
        /// </summary>
        /// <param name="color">Cor das peças a serem obtidas</param>
        public static HashSet<Piece> GetCapturedPieces(Color color) =>
            CapturedPieces
                .Where(p => p.Color.Equals(color))
                .ToHashSet();
            
        /// <summary>
        /// Obtem as peças que ainda estão no jogo de uma determinada cor
        /// </summary>
        /// <param name="color">Cor referente as peças que deseja obter</param>
        public static HashSet<Piece> GetPiecesInGame(Color color)
        {
            // Consultando as peças da cor informada por parâmetro
            var piecesInGame = Pieces
                    .Where(p => p.Color.Equals(color))
                    .ToHashSet();

            // Removendo peças que já tenham sido capturadas
            piecesInGame.ExceptWith(
                    GetCapturedPieces(color)
                );

            return piecesInGame;
        }

        /// <summary>
        /// Realiza um movimento com base nas posições informadas
        /// </summary>
        /// <param name="initialPosition">Posição inicial da peça que deseja realizar o movimento</param>
        /// <param name="finalPosition">Posição final da peça escolhida</param>
        /// <returns>Uma instância de <see langword="Piece"/> da a peça movimentada</returns>
        public static Piece MakeAMove(PositionOnBoard initialPosition, PositionOnBoard finalPosition)
        {
            Piece movimentedPiece = Board.RemovePiece(initialPosition);
            movimentedPiece.IncreaseMovement();

            Piece capturedPiece = Board.RemovePiece(finalPosition);
            Board.PutPiece(movimentedPiece, finalPosition);

            if (capturedPiece is not null)
                CapturedPieces.Add(capturedPiece);

            if (IsASmallCastling(movimentedPiece, initialPosition, finalPosition))
                MakeASmallCastling(initialPosition);

            if (IsABigCastling(movimentedPiece, initialPosition, finalPosition))
                MakeABigCastling(initialPosition);

            if (IsAEnPassant(movimentedPiece, capturedPiece, initialPosition, finalPosition))
                MakeAEnPassant(movimentedPiece, capturedPiece, finalPosition);

            return capturedPiece;
        }

        /// <summary>
        /// Desfaz um movimento com base nas posições e peça capturada
        /// </summary>
        /// <param name="initialPosition">Posição inicial do movimento realizado</param>
        /// <param name="finalPosition">Posição final do movimento realizado</param>
        /// <param name="capturedPiece">Peça capturada no movimento</param>
        public static void UndoAMove(PositionOnBoard initialPosition, PositionOnBoard finalPosition, Piece capturedPiece)
        {
            Piece movimentedPiece = Board.RemovePiece(finalPosition);
            movimentedPiece.DecrementMovement();

            if (capturedPiece is not null)
            {
                Board.PutPiece(capturedPiece, finalPosition);
                CapturedPieces.Remove(movimentedPiece);
            }
            Board.PutPiece(movimentedPiece, initialPosition);

            if (IsASmallCastling(movimentedPiece, initialPosition, finalPosition))
                UndoASmallCastling(initialPosition); // todo: Realizar teste para verificar que realmente está funcionando

            if (IsABigCastling(movimentedPiece, initialPosition, finalPosition))
                UndoABigCastling(initialPosition); // todo: Realizar teste para verificar se realmente está funcionando

            if (IsAEnPassant(movimentedPiece, capturedPiece, initialPosition, finalPosition))
                UndoAEnPassant(finalPosition, movimentedPiece); // todo: Realizar teste para verificar se realmente está funcionando

        }

        /// <summary>
        /// Realiza uma jogada
        /// </summary>
        /// <param name="initialPosition">Posição inicial da peça movimentada</param>
        /// <param name="finalPosition">Posição final da peça movimentada</param>
        public static void MakeAPlay(PositionOnBoard initialPosition, PositionOnBoard finalPosition)
        {
            Piece capturedPiece = MakeAMove(initialPosition, finalPosition);

            if (IsInCheck(CurrentPlayerColor))
            {
                UndoAMove(initialPosition, finalPosition, capturedPiece);
                throw new ExceptionBoard("Você não pode se colocar em xeque!");
            }

            Piece movimentedPiece = Board.GetPiece(finalPosition);

            IsCheck = IsInCheck(GetOpponentColor(CurrentPlayerColor));
            FinishedGame = IsInCheckmate(GetOpponentColor(CurrentPlayerColor));
            
            if (!FinishedGame)
            {
                PassTurn();
                ChangePlayer();
            }

            // Jogada Especial: Promoção
            if (movimentedPiece is Pawn)
            {
                if (movimentedPiece.Color == Color.White && finalPosition.Row == 0 ||
                    movimentedPiece.Color == Color.Red && finalPosition.Row == 7)
                {
                    movimentedPiece = Board.RemovePiece(finalPosition);
                    Pieces.Remove(movimentedPiece);

                    Piece queen = new Queen(movimentedPiece.Color, Board);
                    Board.PutPiece(queen, finalPosition);
                    Pieces.Add(queen);
                }
            }

            // Jogada Especial: En Passant
            if (movimentedPiece is Pawn &&
                (finalPosition.Row - 2 == initialPosition.Row ||
                 finalPosition.Row + 2 == initialPosition.Row))
            {
                VulnerablePieceForEnPassant = movimentedPiece;
            }
            else
            {
                VulnerablePieceForEnPassant = null;
            }
        }

        /// <summary>
        /// Verifica se o jogador de uma determinada cor está em cheque
        /// </summary>
        /// <param name="color">Cor do jogador</param>
        public static bool IsInCheck(Color color)
        {
            PositionOnBoard kingPosition = FindRei(color).Position;
            IEnumerable<Piece> opponentPieces = GetPiecesInGame(GetOpponentColor(color));

            return opponentPieces.Any(p => 
                    p.GetValidMoves()[kingPosition.Row, kingPosition.Column]
                );
        }

        /// <summary>
        /// Verifica se o jogador de uma determinada cor está em cheque mate
        /// </summary>
        /// <param name="color">Cor do jogador a verificar se está em cheque mate</param>
        public static bool IsInCheckmate(Color color)
        {
            if (!IsInCheck(color))
                return false;

            foreach (Piece piece in GetPiecesInGame(color))
            {
                bool[,] possibleMoves = piece.GetValidMoves();

                for (int row = 0; row < Board.Linhas; row++)
                {
                    for (int column = 0; column < Board.Colunas; column++)
                    {
                        if (possibleMoves[row, column])
                        {
                            PositionOnBoard finalPosition = new PositionOnBoard(row, column);
                            PositionOnBoard initialPosition = piece.Position;

                            Piece pecaCapturada = MakeAMove(initialPosition, finalPosition);
                            bool isCheck = IsInCheck(color);

                            UndoAMove(initialPosition, finalPosition, pecaCapturada);
                            if (!isCheck)
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Obtém o Rei de uma determiada cor
        /// </summary>
        /// <param name="color">Cor do Rei a ser procurado</param>
        /// <returns>Rei encontrado</returns>
        private static Piece FindRei(Color color) =>
            GetPiecesInGame(color)
                .FirstOrDefault(p => p is King);

        /// <summary>
        /// Altera a vez do jogador
        /// </summary>
        public static void ChangePlayer()
        {
            if (CurrentPlayerColor == Color.White)
                CurrentPlayerColor = Color.Red;
            else
                CurrentPlayerColor = Color.White;
        }

        /// <summary>
        /// Insere uma nova peça no tabuleiro
        /// </summary>
        /// <param name="column">Coluna da nova peça</param>
        /// <param name="row">Linha da nova peça</param>
        /// <param name="piece">Peça a ser inserida</param>
        public static void PutNewPieceOnBoard(char column, int row, Piece piece)
        {
            Board.PutPiece(piece, new PositionOnGame(column, row).ConvertToPositionOnBoard());
            Pieces.Add(piece);
        }

        /// <summary>
        /// Inicializa as peças do jogo, colocando cada peça em sua respectiva posição
        /// e respectiva cor
        /// </summary>
        private static void InitializePieces()
        {
            // todo: Refatorar cada peça
            PutNewPieceOnBoard('a', 1, new Rook(Color.White, Board));
            PutNewPieceOnBoard('h', 1, new Rook(Color.White, Board));
            PutNewPieceOnBoard('e', 1, new King(Color.White, Board));
            PutNewPieceOnBoard('d', 1, new Queen(Color.White, Board));
            PutNewPieceOnBoard('b', 1, new Horse(Color.White, Board));
            PutNewPieceOnBoard('g', 1, new Horse(Color.White, Board));
            PutNewPieceOnBoard('f', 1, new Bishop(Color.White, Board));
            PutNewPieceOnBoard('c', 1, new Bishop(Color.White, Board));
            PutNewPieceOnBoard('a', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('h', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('e', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('d', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('b', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('g', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('f', 2, new Pawn(Color.White, Board));
            PutNewPieceOnBoard('c', 2, new Pawn(Color.White, Board));

            PutNewPieceOnBoard('a', 8, new Rook(Color.Red, Board));
            PutNewPieceOnBoard('h', 8, new Rook(Color.Red, Board));
            PutNewPieceOnBoard('e', 8, new King(Color.Red, Board));
            PutNewPieceOnBoard('d', 8, new Queen(Color.Red, Board));
            PutNewPieceOnBoard('b', 8, new Horse(Color.Red, Board));
            PutNewPieceOnBoard('g', 8, new Horse(Color.Red, Board));
            PutNewPieceOnBoard('f', 8, new Bishop(Color.Red, Board));
            PutNewPieceOnBoard('c', 8, new Bishop(Color.Red, Board));
            PutNewPieceOnBoard('a', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('h', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('e', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('d', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('b', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('g', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('f', 7, new Pawn(Color.Red, Board));
            PutNewPieceOnBoard('c', 7, new Pawn(Color.Red, Board));

        }

        /// <summary>
        /// Verifica se a jogada é um Roque Pequeno
        /// </summary>
        /// <param name="movimentedPiece">Peça movimentada na jogada</param>
        /// <param name="initialPosition">Posição inicial da peça</param>
        /// <param name="finalPosition">Posição final da peça</param>
        /// <returns></returns>
        private static bool IsASmallCastling(Piece movimentedPiece, PositionOnBoard initialPosition, PositionOnBoard finalPosition) =>
            movimentedPiece is King &&
            finalPosition.Column == initialPosition.Column + 2;

        /// <summary>
        /// Realiza a jogada Roque pequeno
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void MakeASmallCastling(PositionOnBoard initialPosition)
        {
            // Posição da torre com base na posição do rei
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Row, initialPosition.Column + 3);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Row, initialPosition.Column + 1);

            Piece rook = Board.RemovePiece(rookPosition);
            rook.IncreaseMovement();

            Board.PutPiece(rook, rookDestiny);
        }

        /// <summary>
        /// Desfaz a jogada de Roque Pequeno
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void UndoASmallCastling(PositionOnBoard initialPosition)
        {
            // Posição da torre com base na posição do rei
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Row, initialPosition.Column + 1);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Row, initialPosition.Column + 3);

            Piece rook = Board.RemovePiece(rookPosition);
            rook.DecrementMovement();

            Board.PutPiece(rook, rookDestiny);
        }

        /// <summary>
        /// Verifica se a jogada é um Roque Grande
        /// </summary>
        /// <param name="movimentedPiece">Peça movimentada na jogada</param>
        /// <param name="initialPosition">Posição inicial da peça</param>
        /// <param name="finalPosition">Posição final da peça</param>
        /// <returns></returns>
        private static bool IsABigCastling(Piece movimentedPiece, PositionOnBoard initialPosition, PositionOnBoard finalPosition) =>
            movimentedPiece is King && 
            finalPosition.Column == initialPosition.Column - 2;


        /// <summary>
        /// Realiza a jogada Roque grande
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void MakeABigCastling(PositionOnBoard initialPosition)
        {
            // Posição da torre com base na posição do rei
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Row, initialPosition.Column - 4);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Row, initialPosition.Column - 1);

            Piece rook = Board.RemovePiece(rookPosition);
            rook.IncreaseMovement();

            Board.PutPiece(rook, rookDestiny);
        }

        /// <summary>
        /// Desfaz a jogada de Roque Grande
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void UndoABigCastling(PositionOnBoard initialPosition)
        {
            // Posição da torre com base na posição do rei
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Row, initialPosition.Column - 1);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Row, initialPosition.Column - 4);

            Piece rook = Board.RemovePiece(rookPosition);
            rook.IncreaseMovement();
            
            Board.PutPiece(rook, rookDestiny);
        }

        /// <summary>
        /// Verifica se a jogada realizada é um En Passant
        /// </summary>
        /// <param name="movimentedPiece">Peça movimentada na jogada</param>
        /// <param name="capturedPiece">Peça capturada na jogada</param>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        /// <param name="finalPosition">Posição final da jogada</param>
        /// <returns></returns>
        private static bool IsAEnPassant(
                Piece movimentedPiece, 
                Piece capturedPiece, 
                PositionOnBoard initialPosition, 
                PositionOnBoard finalPosition
        ) =>
            movimentedPiece is Pawn &&
            finalPosition.Column != initialPosition.Column &&
            capturedPiece == VulnerablePieceForEnPassant;

        /// <summary>
        /// Realiza a jogada En Passant
        /// </summary>
        /// <param name="movimentedPiece">Instância <see langword="Piece"/> da Peça movimentada</param>
        /// <param name="capturedPiece">Instância <see langword="Piece"/> da Peça capturada</param>
        /// <param name="finalPosition">Posição final da jogada</param>
        private static void MakeAEnPassant(Piece movimentedPiece, Piece capturedPiece, PositionOnBoard finalPosition)
        {
            PositionOnBoard positionCapturedPiece;

            if (movimentedPiece.Color == Color.White)
                positionCapturedPiece = new PositionOnBoard(finalPosition.Row + 1, finalPosition.Column);
            else
                positionCapturedPiece = new PositionOnBoard(finalPosition.Row - 1, finalPosition.Column);

            capturedPiece = Board.RemovePiece(positionCapturedPiece);
            CapturedPieces.Add(capturedPiece);
        }

        /// <summary>
        /// Desfaz a jogada En Passant
        /// </summary>
        /// <param name="finalPosition">Posição final da jogada realizada</param>
        /// <param name="movimentedPiece">Peça movimentada</param>
        private static void UndoAEnPassant(PositionOnBoard finalPosition, Piece movimentedPiece)
        {
            Piece pawn = Board.RemovePiece(finalPosition);

            PositionOnBoard pawnFinalPosition;
            if (movimentedPiece.Color == Color.White)
                pawnFinalPosition = new PositionOnBoard(3, finalPosition.Column);
            else
                pawnFinalPosition = new PositionOnBoard(4, finalPosition.Column);

            Board.PutPiece(pawn, pawnFinalPosition);
        }

    }
}
