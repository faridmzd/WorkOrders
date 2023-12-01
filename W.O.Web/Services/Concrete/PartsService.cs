using FluentResults;
using System.Net.Http.Json;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Services.Abstract;

namespace W.O.Web.Services.Concrete
{
    public class PartsService : IPartsService
    {
        private readonly HttpClient _httpClient;

        private readonly static string endpoint = "api/v1/parts";
        public PartsService(HttpClient httpClient)
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

        public async Task<Result<PartDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{endpoint}/{id}");

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<PartDTO>())!),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<IEnumerable<PartDTO>>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok(((await response.Content.ReadFromJsonAsync<IEnumerable<PartDTO>>())) ?? new List<PartDTO>()),
                    _ => Result.Fail(await response.Content.ReadAsStringAsync())
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<PartDTO>> AddAsync(CreatePartRequest entity)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, entity);

                return response.IsSuccessStatusCode switch
                {
                    true => Result.Ok((await response.Content.ReadFromJsonAsync<PartDTO>())!),
                    _ => Result.Fail(new Error(await response.Content.ReadAsStringAsync()))
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> UpdateAsync(PartDTO entity)
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
