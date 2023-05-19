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

    public class FunctionCategoryService
    {
        FunctionCatagoryRepository _functionCatagoryRepository;

        public FunctionCategoryService(FunctionCatagoryRepository functionCatagoryRepository)
        {
            _functionCatagoryRepository = functionCatagoryRepository;
        }

        public List<FunctionCatagory> ListFunction()
        {
            return _functionCatagoryRepository.ListFunction();
        }
    }
}
