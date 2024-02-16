using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using Infrastructure.Core.Models;
    using Models;
    using Services;

    public class DepartmentCommand
    {
        DepartmentService _departmentService;

        public DepartmentCommand(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public DataResult<int> AddDepartment(Department model)
        {
            return _departmentService.AddDepartment(model);
        }

        public List<Department> ListDepartment()
        {
            return _departmentService.ListDepartment();
        }

        public Department GetDepartment(int DeptID)
        {
            return _departmentService.GetDepartment(DeptID);
        }

        public BaseResult UpdateDepartment(int DepartmentID, Department model)
        {
            return _departmentService.UpdateDepartment(DepartmentID, model);
        }
    }
}
