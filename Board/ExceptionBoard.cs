using System;

namespace tabuleiro
{
    class ExceptionBoard : ApplicationException
    {
        public ExceptionBoard(string msg)
            : base(msg) { }
    }
}
