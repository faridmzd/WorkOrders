using FluentResults;
using System.Net.Http.Json;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Services.Abstract;

namespace W.O.Web.Services.Concrete
{
    public class VisitsService : IVisitsService
    {
        private readonly HttpClient _httpClient;

        private readonly static string endpoint = "api/v1/visits";
        public VisitsService(HttpClient httpClient)
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

        public async Task<Result<VisitDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{endpoint}/{id}");

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<VisitDTO>())!),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<IEnumerable<VisitDTO>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok(((await response.Content.ReadFromJsonAsync<IEnumerable<VisitDTO>>())) ?? new List<VisitDTO>()),
                    _ => Result.Fail(await response.Content.ReadAsStringAsync())
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<VisitDTO>> AddAsync(CreateVisitRequest entity)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, entity);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<VisitDTO>())!),
                    _ => Result.Fail(new Error("sala"))
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> UpdateAsync(VisitDTO entity)
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
