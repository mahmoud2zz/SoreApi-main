using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;

namespace CoffeeStoreApi.Helpers
{
    public class ResponseBuilder
    {
        public static Response<T> Success<T>(string message, T? data = default)
        {
            return new Response<T>(true, message, data);
        }

        public static Response<T> Failure<T>(string message, T? data = default)
        {
            return new Response<T>(false, message, data);
        }

        
    }
}

