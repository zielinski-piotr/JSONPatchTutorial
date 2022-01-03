using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using House = JSONPatchTutorial.Contract.Requests.House;

namespace JSONPatchTutorial.Service
{
    public interface IHouseService
    {
        Task UpdateHouse(JsonPatchDocument<House.Patch> patch, Guid id);
        Task UpdateHouse(House.Update update, Guid id);
        Task<Contract.Responses.House.Response> GetHouseById(Guid id);
        Task<IEnumerable<Contract.Responses.House.ListItem>> GetHouses();
        Task RemoveHouseById(Guid id);
        Task<Contract.Responses.House.Response> CreateHouse(House.Request request);
    }
}