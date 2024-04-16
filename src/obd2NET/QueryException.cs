using System;

namespace obd2NET;

public class QueryException : Exception
{
  public QueryException(string msg) :
    base(msg)
  {
  }
}
