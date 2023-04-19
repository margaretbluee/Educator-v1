using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface ILecturerRepository
{
    IEnumerable<Lecturer> GetAllLecturers();

    Lecturer GetLecturerById(int id);

    void UpdateLecturer(Lecturer lecturer);
    
    void AddLecturer(Lecturer lecturer);
}