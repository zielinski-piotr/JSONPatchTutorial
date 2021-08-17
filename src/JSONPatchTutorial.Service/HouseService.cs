using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using JSONPatchTutorial.Contract.Requests;
using JSONPatchTutorial.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using DataModelHouse = JSONPatchTutorial.Domain.House;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace JSONPatchTutorial.Service
{
    public class HouseService : IHouseService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public HouseService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task UpdateHouse(JsonPatchDocument<House.Patch> patch, Guid id)
        {
            _ = id == Guid.Empty ? throw new ArgumentException(nameof(id)) : id;
            _ = patch ?? throw new ArgumentNullException(nameof(patch));

            var dataModelHouse = await _repository.Get<DataModelHouse>()
                                     .Include(x => x.Address)
                                     .FirstOrDefaultAsync(x => x.Id == id) ??
                                 throw new KeyNotFoundException($"There is no {nameof(House)} with Id: {id}");

            var apiModelHouse = _mapper.Map<House.Patch>(dataModelHouse);

            patch.ApplyTo(apiModelHouse);
            
            _mapper.Map(apiModelHouse, dataModelHouse);

            _repository.Update(dataModelHouse);

            await _repository.SaveChangesAsync();
        }

        public async Task<Contract.Responses.House.Response> GetHouseById(Guid id)
        {
            var dataModelHouse = await _repository.Get<DataModelHouse>()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<Contract.Responses.House.Response>(dataModelHouse);
        }

        public async Task<IEnumerable<Contract.Responses.House.ListItem>> GetHouses()
        {
            var dataModelHouse = await _repository.Get<DataModelHouse>().ToListAsync();

            return _mapper.Map<IEnumerable<Contract.Responses.House.ListItem>>(dataModelHouse);
        }
    }
}