using System;

namespace Xadrez_Console.Board.Exception
{
    class ExceptionBoard : ApplicationException
    {
        public ExceptionBoard(string msg)
            : base(msg) { }
    }
}
