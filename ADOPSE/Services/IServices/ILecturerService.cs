using ADOPSE.Models;

namespace ADOPSE.Services.IServices;

public interface ILecturerService
{
    IEnumerable<Lecturer> GetLecturers();
    Lecturer GetLecturer(int lecturerId);
    void AddLecturer(Lecturer lecturerToAdd);
    void UpdateLecturer(Lecturer lecturerToUpdate);
    void DeleteLecturer(int lecturerId);
}