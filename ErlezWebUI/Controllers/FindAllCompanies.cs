using ErlezWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErlezWebUI.Controllers
{
    public static class FindAllCompanies
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<Company> companies, int selectedId)
        {
            return companies.OrderBy(c => c.CompanyName)
                .Select(company => new SelectListItem
                {
                    Selected = (company.Id == selectedId),
                    Text = company.CompanyName,
                    Value = company.Id.ToString()
                });
        }
    }
}