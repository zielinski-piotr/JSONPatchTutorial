using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JSONPatchTutorial.Contract.Requests;
using JSONPatchTutorial.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using DataModelHouse = JSONPatchTutorial.Domain.House;

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
        
        public async Task UpdateHouse(House.Update update, Guid id)
        {
            _ = id == Guid.Empty ? throw new ArgumentException(nameof(id)) : id;
            _ = update ?? throw new ArgumentNullException(nameof(update));

            var dataModelHouse = await _repository.Get<DataModelHouse>()
                                     .Include(x => x.Address)
                                     .FirstOrDefaultAsync(x => x.Id == id) ??
                                 throw new KeyNotFoundException($"There is no {nameof(House)} with Id: {id}");

            _mapper.Map(update, dataModelHouse);

            _repository.Update(dataModelHouse);

            await _repository.SaveChangesAsync();
        }

        public async Task<Contract.Responses.House.Response> GetHouseById(Guid id)
        {
            _ = id == Guid.Empty ? throw new ArgumentException(nameof(id)) : id;
            
            var dataModelHouse = await _repository.Get<DataModelHouse>()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            return dataModelHouse is null
                ? throw new KeyNotFoundException($"{nameof(DataModelHouse)} with Id: {id} was not found")
                : _mapper.Map<Contract.Responses.House.Response>(dataModelHouse);
        }
        
        public async Task RemoveHouseById(Guid id)
        {
            _ = id == Guid.Empty ? throw new ArgumentException(nameof(id)) : id;
            
            var dataModelHouse = await _repository.Get<DataModelHouse>()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dataModelHouse is null)
                throw new KeyNotFoundException($"{nameof(DataModelHouse)} with Id: {id} was not found");

            _repository.Remove(dataModelHouse);

            await _repository.SaveChangesAsync();
        }

        public async Task<Contract.Responses.House.Response> CreateHouse(House.Request request)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var house = _mapper.Map<DataModelHouse>(request);

            await _repository.Add(house);

            await _repository.SaveChangesAsync();
            
            return _mapper.Map<Contract.Responses.House.Response>(house);
        }

        public async Task<IEnumerable<Contract.Responses.House.ListItem>> GetHouses()
        {
            var dataModelHouse = await _repository.Get<DataModelHouse>().ToListAsync();

            return _mapper.Map<IEnumerable<Contract.Responses.House.ListItem>>(dataModelHouse);
        }
    }
}