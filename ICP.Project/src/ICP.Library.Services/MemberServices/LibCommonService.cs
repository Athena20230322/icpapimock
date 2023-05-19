using ICP.Library.Models.MemberModels;
using ICP.Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    public class LibCommonService
    {
        CommonRepository _commonRepository;

        public LibCommonService(
            CommonRepository commonRepository
            )
        {
            _commonRepository = commonRepository;
        }

        public CountryTownIDModel GetArea(string ZipCode)
        {
            if (string.IsNullOrWhiteSpace(ZipCode)) return null;

            return _commonRepository.GetArea(ZipCode);
        }

        public string GetAreaID(string ZipCode)
        {
            var area = GetArea(ZipCode);

            if (area == null) return null;

            return area.CountyID;
        }
    }
}
