﻿using Microsoft.EntityFrameworkCore;
using Tweet.API.Model;

namespace Tweet.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}