using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.ResultPattern
{
    public class Result<T>
    {
        private Result(T value)
        {
            Value = value;
            Error = null;
        }
        private Result(Error error)
        {
            Error = error;
            Value = default;
        }
        public T? Value { get;}
        public Error? Error { get;}
        public bool IsSuccess => Error == null;

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(Error error) => new Result<T>(error);
    }
}
