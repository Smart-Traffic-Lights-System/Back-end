using System;

namespace UserStore.Business;

public class MyException : Exception
{
    public MyException(string msg) 
        : base(msg)
    {             
    }
}
