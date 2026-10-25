using System;
using System.Threading.Tasks;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> UpdateAppliedPromocodesAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
                return false;

            employee.AppliedPromocodesCount++;

            await _employeeRepository.UpdateAsync(employee);

            return true;
        }
    }
}
