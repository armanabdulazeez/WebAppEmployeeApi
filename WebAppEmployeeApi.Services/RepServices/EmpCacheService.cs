using WebAppEmployeeApi.Domain.Models;

namespace WebAppEmployeeApi.Services.RepServices
{
    public class EmpCacheService
    {
        private readonly Dictionary<string, (List<EmployeeModel> Data, DateTime Expiry)> _cache = new();
        private readonly TimeSpan _timeToLive = TimeSpan.FromSeconds(300);

        public void Set(string key, List<EmployeeModel> employees)
        {
            var expiry = DateTime.Now.Add(_timeToLive);
            _cache[key] = (employees, expiry);
        }

        public bool TryGet(string key, out List<EmployeeModel>? employees)
        {
            employees = null;

            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.Expiry > DateTime.Now)
                {
                    employees = entry.Data;
                    return true;
                }

                _cache.Remove(key);
            }
            return false;
        }
        public void ClearAll()
        {
            _cache.Clear();
        }
    }
}
