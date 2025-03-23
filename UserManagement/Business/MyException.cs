using System;

namespace UserManagement.Business;

public class MyException : Exception
{
    public MyException(string msg) 
        : base(msg)
    {             
    }
}
