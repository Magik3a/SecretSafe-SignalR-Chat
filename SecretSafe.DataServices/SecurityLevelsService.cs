using SecretSafe.Models;
using System.Linq;

namespace SecretSafe.DataServices
{
    using SecretSafe.Models;
    using System.Linq;
    using System;
    using Data;

    public class SecurityLevelsService : ISecurityLevelsService
    {
        private readonly IRepository<SecurityLevel> securityLevel;
        public SecurityLevelsService(IRepository<SecurityLevel> securityLevel)
        {
            this.securityLevel = securityLevel;
        }
        public IQueryable<SecurityLevel> Get(int SecurityLevelId)
        {
            return securityLevel.All().Where(s => s.SecurityLevelId == SecurityLevelId);
        }

        public IQueryable<SecurityLevel> GetAll()
        {
            return securityLevel.All();
        }
    }
}
