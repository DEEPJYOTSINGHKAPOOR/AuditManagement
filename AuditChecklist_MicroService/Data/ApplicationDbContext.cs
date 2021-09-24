using Global_MicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditChecklist_MicroService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<AuditQuestionModel> AuditQuestions { get; set; }
    }
}
