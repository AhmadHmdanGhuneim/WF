﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WF.Config;
using WF.Functions;
using WF.Helpers;
using WF.Models.BaseResult;

namespace WF.ApiFactory
{
    public class BaseFactory
    {
        protected async Task<OperationResult<TModel>> Request<TModel>(string path, HttpMethod method, CancellationToken token, Dictionary<string, string> options = null)
        {
            OperationResult<TModel> result;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = method
                };

                if (method == HttpMethod.Post)
                {
                    if (options != null)
                        request.Content = new FormUrlEncodedContent(options);
                    request.RequestUri = new Uri(Configuration.HOST_URI + path);
                }
                else if (method == HttpMethod.Get)
                {
                    request.RequestUri = new Uri(Configuration.HOST_URI + path + "?" + OptionsToStr(options));
                }

                var response = await client.SendAsync(request, token);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OperationResult<TModel>>(json);
                }
                else
                {
                    result = OperationResult<TModel>.Result(ResultCode.UnknownError);
                }
                await ApiResultExecutor.Execute(result);
                return result;
            }
            catch (TaskCanceledException)
            {
                result = OperationResult<TModel>.Result(ResultCode.Cancelled);
                return result;
            }
            catch (Exception exception)
            {
                result = OperationResult<TModel>.Result(ResultCode.UnknownError);
                GeneralFunctions.HandelException(exception, "Request");
                return result;
            }
         
        }

        private static string OptionsToStr(Dictionary<string, string> data)
        {
            try
            {


                var result = String.Empty;
                if (data != null)
                {
                    foreach (var key in data.Keys)
                        if (data[key] != null)
                            result = result + (key + "=" + Uri.EscapeUriString(data[key]) + "&");
                }
                return result.TrimEnd('&');
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "OptionStr");
                return null;
            }
        }




        protected async Task<OperationResult<TModel>> CompanyRequest<TModel>(string path, HttpMethod method, CancellationToken token, Dictionary<string, string> options = null)
        {
            OperationResult<TModel> result;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = method
                };

                if (method == HttpMethod.Post)
                {
                    if (options != null)
                        request.Content = new FormUrlEncodedContent(options);
                    request.RequestUri = new Uri(Configuration.HOST_URI + path);
                }
                else if (method == HttpMethod.Get)
                {
                    request.RequestUri = new Uri(Configuration.HOST_URI + path + "?" + OptionsToStr(options));
                }

                var response = await client.SendAsync(request, token);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<OperationResult<TModel>>(json);
                }
                else
                {
                    result = OperationResult<TModel>.Result(ResultCode.UnknownError);
                }
            }
            catch (TaskCanceledException)
            {
                result = OperationResult<TModel>.Result(ResultCode.Cancelled);
            }
            catch (Exception exception)
            {
                result = OperationResult<TModel>.Result(ResultCode.UnknownError);
                GeneralFunctions.HandelException(exception, "Request");
            }
            await ApiResultExecutor.Execute(result);
            return result;
        }

    }
}
