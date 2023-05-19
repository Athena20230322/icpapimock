using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models;

    public class DepartmentService
    {
        DepartmentRepository _departmentRepository;

        public DepartmentService(DepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public DataResult<int> AddDepartment(Department model)
        {
            return _departmentRepository.AddDepartment(model);
        }

        public List<Department> ListDepartment()
        {
            return _departmentRepository.ListDepartment();
        }

        public Department GetDepartment(int DeptID)
        {
            return _departmentRepository.GetDepartment(DeptID);
        }

        public BaseResult UpdateDepartment(int DepartmentID, Department model)
        {
            return _departmentRepository.UpdateDepartment(DepartmentID, model);
        }
    }
}
