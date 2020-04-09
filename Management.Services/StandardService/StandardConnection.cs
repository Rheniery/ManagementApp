using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Management.Services.StandardService
{
    public class StandardConnection : DbContext
    {
        public string GetConnectionString()
        {
            return @"Server=localhost\MSSQLSERVER01;Database=Management;Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }

    
}
