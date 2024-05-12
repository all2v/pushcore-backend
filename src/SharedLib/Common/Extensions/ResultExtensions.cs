using Astrum.Core.Common.Results;
using Astrum.Core.Common.Results;
using IResult = Astrum.Core.Common.Results.IResult;
using Result = Astrum.Core.Common.Results.Result;

namespace Astrum.Core.Common.Extensions;

public static class ResultExtensions
{
    public static Result WithMessage(this Result result, string message)
    {
        result.Message = message;
        return result;
    }

    public static Result WithError(this Result result, string error)
    {
        result.AddError(error);
        return result;
    }

    public static Result WithError(this Result result, string error, string code)
    {
        result.AddError(error, code);
        return result;
    }

    public static Result WithErrors(this Result result, List<IResultError> errors)
    {
        result.AddErrors(errors);
        return result;
    }

    public static Result WithErrors(this Result result, List<ResultError> errors)
    {
        result.AddErrors(errors);
        return result;
    }

    public static Result Failed(this Result result)
    {
        result.Succeeded = false;
        return result;
    }

    public static Result Successful(this Result result)
    {
        result.Succeeded = true;
        return result;
    }

    public static Result WithException(this Result result, Exception exception)
    {
        result.Exception = exception;
        return result;
    }

    public static Results.Result<T> WithMessage<T>(this Results.Result<T> result, string message)
    {
        result.Message = message;
        return result;
    }

    public static Results.Result<T> WithData<T>(this Results.Result<T> result, T data)
    {
        result.Data = data;
        return result;
    }

    public static Results.Result<T> WithError<T>(this Results.Result<T> result, string error)
    {
        result.AddError(error);
        return result;
    }

    public static Results.Result<T> WithErrors<T>(this Results.Result<T> result, List<ResultError> errors)
    {
        result.AddErrors(errors);
        return result;
    }

    public static Results.Result<T> WithError<T>(this Results.Result<T> result, string error, string code)
    {
        result.AddError(error, code);
        return result;
    }

    public static Results.Result<T> Failed<T>(this Results.Result<T> result)
    {
        result.Succeeded = false;
        return result;
    }

    public static Results.Result<T> Successful<T>(this Results.Result<T> result)
    {
        result.Succeeded = true;
        return result;
    }

    public static Results.Result<T> WithException<T>(this Results.Result<T> result, Exception exception)
    {
        result.Exception = exception;
        return result;
    }

    public static T GetValue<T>(this IResult result)
    {
        if (result is not Result<T> genericResult)
            throw new NotSupportedException("Result not contains additional value");

        return genericResult.Data;
    }
}