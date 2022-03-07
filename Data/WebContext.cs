using Microsoft.EntityFrameworkCore;
using RegistroLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistroLogin.Data
{
    public class WebContext: DbContext
    {
        public WebContext(DbContextOptions<WebContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}
