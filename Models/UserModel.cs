using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxBill.Models
{
    public class UserModel
    {
        public long tckn { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string birthDay { get; set; }
        public string password { get; set; }

        public UserModel() {
            
        }
    }
}