using System.Collections.Generic;
using System.Linq;
using tabuleiro;
using ConsoleChessGame.Board.Exception;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces;
using ConsoleChessGame.Game.Pieces.Abstract;
using Xadrez_Console.Game.Pieces;

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
            // STOPPED HERE

            if (IsABigCastling(movimentedPiece, initialPosition, finalPosition))
            {
                PositionOnBoard posTorre = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna - 4);
                PositionOnBoard destinoTorre = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna - 1);
                Piece torre = Board.RemovePiece(destinoTorre);
                torre.IncreaseMovement();
                Board.PutPiece(torre, posTorre);
            }

            if (IsAEnPassant(movimentedPiece, capturedPiece, initialPosition, finalPosition))
            {
                Piece peao = Board.RemovePiece(finalPosition);
                PositionOnBoard posP;
                if (movimentedPiece.Color == Color.White)
                {
                    posP = new PositionOnBoard(3, finalPosition.Coluna);
                }
                else
                {
                    posP = new PositionOnBoard(4, finalPosition.Coluna);
                }
                Board.PutPiece(peao, posP);
            }

        }

        public void RealizarJogada(PositionOnBoard posInicial, PositionOnBoard posFinal)
        {
            Piece pecaMorta = MakeAMove(posInicial, posFinal);
            if (EstaEmXeque(CurrentPlayerColor))
            {
                UndoAMove(posInicial, posFinal, pecaMorta);
                throw new ExceptionBoard("Você não pode se colocar em xeque!");
            }
            Piece possivelEnPassant = Board.GetPiece(posFinal);
            // Jogada Especial: Promoção
            if (possivelEnPassant is Pawn)
            {
                if (possivelEnPassant.Color == Color.White && posFinal.Linha == 0 ||
                    possivelEnPassant.Color == Color.Red && posFinal.Linha == 7)
                {
                    possivelEnPassant = Board.RemovePiece(posFinal);
                    Pieces.Remove(possivelEnPassant);
                    Piece dama = new Queen(possivelEnPassant.Color, Board);
                    Board.PutPiece(dama, posFinal);
                    Pieces.Add(dama);
                }
            }



            if (EstaEmXeque(GetOpponentColor(CurrentPlayerColor)))
            {
                IsCheck = true;
            }
            else
            {
                IsCheck = false;
            }

            if (TesteXequeMate(GetOpponentColor(CurrentPlayerColor)))
            {
                FinishedGame = true;
            }
            else
            {
                PassTurn();
                MudarJogador();
            }

            // Jogada Especial: En Passant
            if (possivelEnPassant is Pawn &&
                (posFinal.Linha - 2 == posInicial.Linha ||
                 posFinal.Linha + 2 == posInicial.Linha))
            {
                VulnerablePieceForEnPassant = possivelEnPassant;
            }
            else
            {
                VulnerablePieceForEnPassant = null;
            }
        }

        public bool EstaEmXeque(Color cor)
        {

            PositionOnBoard posicaoRei = FindRei(cor).Position;
            foreach (Piece p in GetPiecesInGame(GetOpponentColor(cor)))
            {
                if (p.GetValidMoves()[posicaoRei.Linha, posicaoRei.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Color cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach (Piece p in GetPiecesInGame(cor))
            {
                bool[,] movPossiveis = p.GetValidMoves();
                for (int i = 0; i < Board.Linhas; i++)
                {
                    for (int j = 0; j < Board.Colunas; j++)
                    {
                        if (movPossiveis[i, j])
                        {
                            PositionOnBoard destino = new PositionOnBoard(i, j);
                            PositionOnBoard origem = p.Position;

                            Piece pecaCapturada = MakeAMove(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            UndoAMove(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        private Piece FindRei(Color cor)
        {
            foreach (Piece p in GetPiecesInGame(cor))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public void MudarJogador()
        {
            if (CurrentPlayerColor == Color.White)
            {
                CurrentPlayerColor = Color.Red;
            }
            else
            {
                CurrentPlayerColor = Color.White;
            }
        }

        public static void ColocarNovaPeca(char coluna, int linha, Piece peca)
        {
            Board.PutPiece(peca, new PositionOnGame(coluna, linha).ConvertToPositionOnBoard());
            Pieces.Add(peca);
        }

        /// <summary>
        /// Inicializa as peças do jogo, colocando cada peça em sua respectiva posição
        /// e respectiva cor
        /// </summary>
        private static void InitializePieces()
        {
            // todo: Refatorar cada peça
            ColocarNovaPeca('a', 1, new Rook(Color.White, Board));
            ColocarNovaPeca('h', 1, new Rook(Color.White, Board));
            ColocarNovaPeca('e', 1, new King(Color.White, Board, this));
            ColocarNovaPeca('d', 1, new Queen(Color.White, Board));
            ColocarNovaPeca('b', 1, new Horse(Color.White, Board));
            ColocarNovaPeca('g', 1, new Horse(Color.White, Board));
            ColocarNovaPeca('f', 1, new Bishop(Color.White, Board));
            ColocarNovaPeca('c', 1, new Bishop(Color.White, Board));
            ColocarNovaPeca('a', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('h', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('e', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('d', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('b', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('g', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('f', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('c', 2, new Pawn(Color.White, Board, this));


            ColocarNovaPeca('a', 8, new Rook(Color.Red, Board));
            ColocarNovaPeca('h', 8, new Rook(Color.Red, Board));
            ColocarNovaPeca('e', 8, new King(Color.Red, Board, this));
            ColocarNovaPeca('d', 8, new Queen(Color.Red, Board));
            ColocarNovaPeca('b', 8, new Horse(Color.Red, Board));
            ColocarNovaPeca('g', 8, new Horse(Color.Red, Board));
            ColocarNovaPeca('f', 8, new Bishop(Color.Red, Board));
            ColocarNovaPeca('c', 8, new Bishop(Color.Red, Board));
            ColocarNovaPeca('a', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('h', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('e', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('d', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('b', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('g', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('f', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('c', 7, new Pawn(Color.Red, Board, this));

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
            finalPosition.Coluna == initialPosition.Coluna + 2;

        /// <summary>
        /// Realiza a jogada Roque pequeno
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void MakeASmallCastling(PositionOnBoard initialPosition)
        {
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna + 3);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna + 1);

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
            PositionOnBoard rookPosition = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna + 1);
            PositionOnBoard rookDestiny = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna + 3);

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
            finalPosition.Coluna == initialPosition.Coluna - 2;

        /// <summary>
        /// Realiza a jogada Roque grande
        /// </summary>
        /// <param name="initialPosition">Posição inicial da jogada</param>
        private static void MakeABigCastling(PositionOnBoard initialPosition)
        {
            PositionOnBoard posTorre = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna - 4);
            PositionOnBoard destinoTorre = new PositionOnBoard(initialPosition.Linha, initialPosition.Coluna - 1);

            Piece torre = Board.RemovePiece(posTorre);
            torre.IncreaseMovement();

            Board.PutPiece(torre, destinoTorre);
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
            finalPosition.Coluna != initialPosition.Coluna &&
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
                positionCapturedPiece = new PositionOnBoard(finalPosition.Linha + 1, finalPosition.Coluna);
            else
                positionCapturedPiece = new PositionOnBoard(finalPosition.Linha - 1, finalPosition.Coluna);

            capturedPiece = Board.RemovePiece(positionCapturedPiece);
            CapturedPieces.Add(capturedPiece);
        }
    }
}
