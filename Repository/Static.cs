using Infrastructure_Layer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Data_Layer.Repository
{
    public class Static
    {
        public static Customer GetCustomer(HttpContext HttpContext, MaindbContext _context)
        {
            string myCookieValue = HttpContext.Request.Cookies["MyCookie"];
            var person = _context.Customers.FirstOrDefault(x => x.Cookie == myCookieValue);
            return person;
        }
    }
}
