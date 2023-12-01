using System.Net.Http.Json;
using FluentResults;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Services.Abstract;
using System.Net.Http.Headers;
using System.Net;

namespace W.O.Web.Services.Concrete
{
    public class WorkOrdersService : IWorkOrdersService
    {
        private readonly HttpClient _httpClient;

        private readonly static string endpoint = "api/v1/workorders";
        public WorkOrdersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            try
            { 
            var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok(),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<WorkOrderDetailsDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{endpoint}/{id}");

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<WorkOrderDetailsDTO>())!),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
            }
            catch(Exception)
            {

                throw;
            }
        }

        public async Task<Result<IEnumerable<WorkOrderDTO>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok(((await response.Content.ReadFromJsonAsync<IEnumerable<WorkOrderDTO>>())) ?? new List<WorkOrderDTO>()),
                    _ => Result.Fail(await response.Content.ReadAsStringAsync())
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<WorkOrderDTO>> AddAsync(CreateWorkOrderRequest entity)
        {
			try
			{
                var response = await _httpClient.PostAsJsonAsync(endpoint,entity);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<WorkOrderDTO>())!),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
			}
			catch (Exception)
			{
				throw;
			}
		}

        public async Task<Result> UpdateAsync(WorkOrderDTO entity)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{endpoint}/{entity.Id}", entity);

                return response.IsSuccessStatusCode switch
                {
                     true => Result.Ok(),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
