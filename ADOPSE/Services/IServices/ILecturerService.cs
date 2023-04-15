using ADOPSE.DTOs;

namespace ADOPSE.Services.IServices;

public interface ILecturerService
{
    Task<List<LecturerDTO>> GetLecturers();
    Task<LecturerDTO> GetLecturer(int lecturerId);
    Task<LecturerDTO> AddLecturer(LecturerToAddDTO lecturerToAddDTO);
    Task<LecturerDTO> UpdateLecturer(LecturerDTO lecturerToUpdateDTO);
    Task DeleteLecturer(int lecturerId);
}