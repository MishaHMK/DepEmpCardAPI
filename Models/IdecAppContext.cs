using System;
using System.Data.Entity;

namespace DepEmpCardAPI.Models
{
    public interface IdecAppContext : IDisposable
    {
        DbSet<Department> Departments { get; set; }
        int SaveChanges();
        void MarkAsModified(Department item);
    }
}
