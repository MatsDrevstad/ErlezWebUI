using ErlezWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErlezWebUI.Controllers
{
    public static class _ExtendCompanies
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

        public static string ConvertNull(this IEnumerable<Company> companies, int id, string result = "N/A")
        {
            try
            {
                result = companies.First(c => c.Id == id).CompanyName;
            }
            catch (Exception) { }

            return result;
        }

        public static string ToggleClass(this string CompanyName, string compare)
        {
            string str;
            str = (CompanyName == compare) ? "alert-danger" : "default";
            return str;
        }
    }
}