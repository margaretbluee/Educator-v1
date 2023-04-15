using ADOPSE.DTOs;
using ADOPSE.Models;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services.IServices;

namespace ADOPSE.Services;

public class LecturerService : ILecturerService
{
    private readonly ILecturerRepository _lecturerRepository;

    public LecturerService(ILecturerRepository lecturerRepository)
    {
        _lecturerRepository = lecturerRepository;
    }

    public async Task<List<LecturerDTO>> GetLecturers()
    {
        // var usersToReturn = await _lecturerRepository.GetList();
        //
        // return usersToReturn;
        throw new NotImplementedException();
    }

    public Task<LecturerDTO> GetLecturer(int lecturerId)
    {
        throw new NotImplementedException();
    }

    public Task<LecturerDTO> AddLecturer(LecturerToAddDTO lecturerToAddDTO)
    {
        throw new NotImplementedException();
    }

    public Task<LecturerDTO> UpdateLecturer(LecturerDTO lecturerToUpdateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLecturer(int lecturerId)
    {
        throw new NotImplementedException();
    }
}