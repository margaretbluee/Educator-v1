﻿using ADOPSE.Models;

namespace ADOPSE.Repositories.IRepositories;

public interface IModuleRepository
{
    IEnumerable<Module> GetModules();

    Module GetModuleById(int id);

    bool ExistsModuleById(int id);

    bool IsGoogleCalendarIdEmpty(int moduleid);    

    IEnumerable<Module> GetModuleStacks(int limit, int offset);

    int GetModuleCount();

    int GetModuleCountByLecturerId(int id);

    int GetModuleCountFiltered(Dictionary<string, string> dic);

    IEnumerable<Module> GetFilteredModules(Dictionary<string, string> dic, int limit, int offset);

    Module GetModuleByCalendarId(string id);

    IEnumerable<Module> GetModuleStacksByLecturerId(int limit, int offset, int id);

    IEnumerable<string> GetAllCalendarIds();

    Module UpdateGoogleCalendarIdOfModuleByModuleId(int moduleId, string googleCalendarId);

    bool ClearGoogleCalendarIdOfModuleByCalendarId(string  calendarId);

    public void CreateIndex();



}