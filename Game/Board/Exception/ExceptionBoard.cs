using System;

namespace ConsoleChessGame.Board.Exception
{
    class ExceptionBoard : ApplicationException
    {
        public ExceptionBoard(string msg)
            : base(msg) { }
    }
}
