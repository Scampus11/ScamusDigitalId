using ScampusCloud.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScampusCloud.Repository.DoorGroup
{
    public class DoorGroupRepository
    {
        private GeneralMethods objgm = new GeneralMethods();
        public List<SelectListItem> BindReaderDropDown(string CompanyId)
        {
            QueryBuilder objQueryBuilder = new QueryBuilder
            {
                TableName = "Tbl_Mstr_Reader_Master",
                StoredProcedureName = @"Sps_Load_Reader_Dropdown",
                SetQueryType = QueryBuilder.QueryType.SELECT,
            };
            objQueryBuilder.AddFieldValue("@CompanyId", CompanyId, DataTypes.Text, false);
            return objgm.GetListUsingSp<SelectListItem>(objQueryBuilder);
        }
    }
}