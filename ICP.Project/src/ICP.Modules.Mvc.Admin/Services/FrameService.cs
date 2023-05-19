using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Models;
    using Models.ViewModels;

    public class FrameService
    {
        /// <summary>
        /// 建立最多有三層關連的Tree
        /// </summary>
        /// <param name="userFunctions"></param>
        /// <returns></returns>
        public List<FrameMenuItem> ListMenuItem(List<FunctionCatagory> userFunctions)
        {
            var items = Mapper.Map<List<FrameMenuItem>>(userFunctions);  

            var rootTree = items.Where(f => f.FunctionLevel == 1).ToList();
            foreach (var rootFuc in rootTree)
            {
                var sencondTree = items.Where(t => t.FunctionGroupID == rootFuc.FunctionID && t.FunctionGroupID != t.FunctionID).ToList();
                foreach (var secondFuc in sencondTree)
                {
                    var thirdTree = items.Where(t => t.FunctionGroupID == secondFuc.FunctionID && t.FunctionGroupID != t.FunctionID).ToList();
                    secondFuc.ChildrenFunction = thirdTree;
                }

                rootFuc.ChildrenFunction = sencondTree.Where(t => t.ChildrenFunction.Count > 0).ToList();
            }

            return rootTree.Where(t => t.ChildrenFunction.Count > 0).ToList();
        }
    }
}
