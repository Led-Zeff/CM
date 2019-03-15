using CM.Context.Entities;
using CM.Context.Repositories.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CM.Context.Repositories
{
    class UnitOfWork : IDisposable
    {
        private CasContext context = new CasContext();
        private BaseRepository<Cas> casRepository;
        private BaseRepository<Project> projectRepository;
        private BaseRepository<CasFile> casFileRepository;

        public BaseRepository<Cas> CasRepository
        {
            get
            {
                if (casRepository == null)
                    casRepository = new BaseRepository<Cas>(context);
                return casRepository;
            }
        }

        public BaseRepository<Project> ProjectRepository
        {
            get
            {
                if (projectRepository == null)
                    projectRepository = new BaseRepository<Project>(context);
                return projectRepository;
            }
        }

        public BaseRepository<CasFile> CasFileRepository
        {
            get
            {
                if (casFileRepository == null)
                    casFileRepository = new BaseRepository<CasFile>(context);
                return casFileRepository;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
