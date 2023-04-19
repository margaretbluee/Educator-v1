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


    public IEnumerable<Lecturer> GetLecturers()
    {
        return _lecturerRepository.GetAllLecturers();
    }

    public Lecturer GetLecturer(int lecturerId)
    {
        throw new NotImplementedException();
    }

    public void AddLecturer(Lecturer lecturerToAdd)
    {
        throw new NotImplementedException();
    }

    public void UpdateLecturer(Lecturer lecturerToUpdate)
    {
        throw new NotImplementedException();
    }

    public void DeleteLecturer(int lecturerId)
    {
        throw new NotImplementedException();
    }
}