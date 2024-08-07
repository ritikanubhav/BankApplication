using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class InvalidPinException : ApplicationException
{
    public InvalidPinException(string message) : base(message)
    {
    }
}

public class InsufficientBalanceException : ApplicationException
{
    public InsufficientBalanceException(string message) : base(message)
    {
    }
}

public class InactiveAccountException : ApplicationException
{
    public InactiveAccountException(string message) : base(message)
    {
    }
}

public class AccountDoesNotExistException : ApplicationException
{
    public AccountDoesNotExistException(string message) : base(message)
    {
    }
}
public class InvalidAccountTypeException : ApplicationException
{
    public InvalidAccountTypeException(string message) : base(message) { }
}
public class DailyLimitExceededException : Exception
{
    public DailyLimitExceededException() : base("Daily limit exceeded.") { }
}

public class InvalidPrivilegeTypeException : Exception
{
    public InvalidPrivilegeTypeException() : base("Invalid privilege type provided.") { }
}
public class TransactionNotFoundException : Exception
{
    public TransactionNotFoundException() : base("Transaction not found.") { }
}

public class InvalidTransactionTypeException : Exception
{
    public InvalidTransactionTypeException() : base("Invalid transaction type provided.") { }
}
public class InvalidPolicyTypeException : Exception
{
    public InvalidPolicyTypeException() : base("Invalid policy type.") { }
}

public class UnableToOpenAccountException : Exception
{
    public UnableToOpenAccountException(string message) : base(message) { }
}

public class MinBalanceNeedsToBeMaintainedException : Exception
{
    public MinBalanceNeedsToBeMaintainedException(string message) : base(message) { }
}