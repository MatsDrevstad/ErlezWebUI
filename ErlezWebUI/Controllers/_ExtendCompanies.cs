using ErlezWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErlezWebUI.Controllers
{
    public static class _ExtendCompanyB2s
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<CompanyB2> companyB2s, int selectedId)
        {
            return companyB2s.OrderBy(c => c.CompanyName)
                .Select(company => new SelectListItem
                {
                    Selected = (company.Id == selectedId),
                    Text = company.CompanyName,
                    Value = company.Id.ToString()
                });
        }

        public static string ConvertNull(this IEnumerable<CompanyB2> CompanyB2s, int id, string result = "N/A")
        {
            try
            {
                result = CompanyB2s.First(c => c.Id == id).CompanyName;
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