using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.ApiModels
{
    public class ApiModel { }

    #region Request Models

    public class ApiListRequestModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }

    #endregion

    #region Response Models

    public class ApiResponseModel
    {
        public string MainMessage { get => Message != null && Message.Any() ? Message.FirstOrDefault() : null; }

        public IList<string> Message { get; set; }
    }

    public class ApiStaticListResponseModel : ApiResponseModel
    {
        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string NextUrl { get; set; }
    }

    public class ApiListResponseModel<T> : ApiStaticListResponseModel where T : ApiModel
    {
        public IList<T> Items { get; set; }
    }

    #region Success Responses

    public class SuccessApiResponseModel : ApiResponseModel
    {
        public SuccessApiResponseModel()
        {
            Message = new List<string>();
        }

        public SuccessApiResponseModel(string SuccessMessage)
        {
            Message = new List<string>() { SuccessMessage };
        }

        public SuccessApiResponseModel(IEnumerable<string> SuccessMessages)
        {
            Message = SuccessMessages.ToList();
        }

        public bool Success = true;

        public int Id { get; set; }

    }

    public class SuccessAuthenticatedApiResponseModel : SuccessApiResponseModel
    {
        public SuccessAuthenticatedApiResponseModel(string SuccessMessage)
        {
            Message = new List<string>() { SuccessMessage };
        }

        public string Token { get; set; }

        public string TokenType { get; set; }
    }

    public class SuccessApiListResponseModel<T> : ApiListResponseModel<T> where T : ApiModel
    {
        public SuccessApiListResponseModel()
        {
            Message = new List<string>();
        }
    }

    #endregion

    #region Error Responses

    public class ErrorApiResponseModel : ApiResponseModel
    {
        public bool Success = false;

        public bool IsNotAuthenticated { get; set; }

        public ErrorApiResponseModel()
        {
            Message = new List<string>();
        }

        public ErrorApiResponseModel(string ErrorMessage)
        {
            Message = new List<string>() { ErrorMessage };
        }

        public ErrorApiResponseModel(IEnumerable<string> ErrorMessages)
        {
            Message = ErrorMessages.ToList();
        }
    }

    #endregion

    #endregion

    #region Common Methods

    public static class ApiExtensionMethods
    {
        public static void ReadyResponse(this ApiStaticListResponseModel response, HttpRequest httpRequest, ApiListRequestModel request)
        {
            response.Page = request.Page;
            response.PageSize = request.PageSize;

            try
            {
                response.TotalPages = response.TotalCount / response.PageSize;

                if (response.TotalCount % response.PageSize > 0)
                    response.TotalPages++;
            }
            catch { }

            try
            {
                if ((request.Page * request.PageSize) <= response.TotalCount)
                {
                    var UriBuilder = new UriBuilder();
                    string host = httpRequest.Host.Value.ToString();
                    UriBuilder.Host = host;
                    UriBuilder.Path = httpRequest.Path;
                    UriBuilder.Query = string.Format("page={0}&pageSize={1}", request.Page + 1, request.PageSize);
                    response.NextUrl = UriBuilder.ToString().Replace($"[{host}]", host);
                }
                else
                {
                    response.NextUrl = null;
                }
            }
            catch
            {
                response.NextUrl = null;
            }
        }
    }

    #endregion
}
