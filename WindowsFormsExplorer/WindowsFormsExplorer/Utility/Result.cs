using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsExplorer.Utility
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public Error Error { get; }

        private Result(bool isSuccess, T value, Error error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }


        public static Result<T> Success(T value) =>
        new Result<T>(true, value, Error.None);

        public static Result<T> Failure(Error error) =>
            new Result<T>(false, default, error);

        public static implicit operator Result<T>(T value) =>
            Success(value);

        public static implicit operator Result<T>(Error error) =>
            Failure(error);


        public Result<TResult> Map<TResult>(Func<T, TResult> func)
        {
            if (IsFailure)
                return Result<TResult>.Failure(Error);

            return Result<TResult>.Success(func(Value));
        }


        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func)
        {
            if (IsFailure)
                return Result<TResult>.Failure(Error);

            return func(Value);
        }


    }



}
