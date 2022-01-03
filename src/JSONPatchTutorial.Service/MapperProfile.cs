using AutoMapper;
using JSONPatchTutorial.Contract.Responses;
using DataModelHouse = JSONPatchTutorial.Domain.House;
using DataModelAddress = JSONPatchTutorial.Domain.Address;
using DataModelRoom = JSONPatchTutorial.Domain.Room;

namespace JSONPatchTutorial.Service
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<DataModelHouse, House.Response>();
            CreateMap<DataModelHouse, House.ListItem>();
            CreateMap<DataModelHouse, Contract.Requests.House.Patch>().ReverseMap();
            CreateMap<DataModelAddress, Address.Response>();
            CreateMap<DataModelAddress, Contract.Requests.Address.Patch>().ReverseMap();
            CreateMap<Contract.Requests.House.Update, DataModelHouse>().ReverseMap();
            CreateMap<Contract.Requests.House.Request, DataModelHouse>();
            CreateMap<DataModelRoom, Room.ListItem>();
            CreateMap<DataModelRoom, Contract.Requests.Room.Patch>().ReverseMap();
        }
    }
}