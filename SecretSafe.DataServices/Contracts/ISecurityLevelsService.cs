﻿using SecretSafe.Models;
using System.Linq;

namespace SecretSafe.DataServices
{
    public interface ISecurityLevelsService
    {
        IQueryable<SecurityLevel> GetAll();

        IQueryable<SecurityLevel> Get(int SecurityLevelId);

        SecurityLevel GetByName(string SecurityLevelName);
    }
}