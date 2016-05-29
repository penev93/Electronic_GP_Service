using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Hospitology.Models
{
    public class AddministratorViewModel
    {
        public static Expression<Func<Addministrator, AddministratorViewModel>> FromAddministrator
        {
            get
            {
                return admin => new AddministratorViewModel
                {

                    username = admin.AdminName,
                    password = admin.Password,
                    
                };
            }

        }
        public string username { get; set; }
        public string password { get; set; }
    }
}