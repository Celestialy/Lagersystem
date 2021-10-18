using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SKPLager.API.Database;
using SKPLager.Shared.Models;

namespace SKPLager.API.Services
{
    public class Repo<Entity, IdType> : IRepo<Entity, IdType> where Entity : BaseModel<IdType>
    {
        protected readonly DBContext _context;
        protected readonly ICurrentUserDepartmentService _CurrentDepartment;

        public Repo(DBContext context, ICurrentUserDepartmentService currentDepartment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _CurrentDepartment = currentDepartment ?? throw new ArgumentNullException(nameof(currentDepartment));
        }

        public virtual IQueryable<Entity> GetAll()
        {
            return _context.Set<Entity>().Where(x => _CurrentDepartment.Ids.Contains(x.DepartmentId));
        }

        public virtual Entity Get(IdType OId)
        {
            return _context.Set<Entity>().Find(OId);
        }

        public virtual async Task<Entity> GetAsync(IdType OId)
        {
            return await _context.Set<Entity>().FindAsync(OId);
        }

        public IEnumerable<Entity> Find(Expression<Func<Entity, bool>> predicate)
        {
            return _context.Set<Entity>().Where(x => _CurrentDepartment.Ids.Contains(x.DepartmentId)).Where(predicate);
        }

        public void Add(Entity entity)
        {

            _context.Set<Entity>().Add(entity);
        }

        public virtual async Task AddAsync(Entity entity)
        {
            await _context.Set<Entity>().AddAsync(entity);
        }

        public void AddRange(IEnumerable<Entity> entitis)
        {
            _context.Set<Entity>().AddRange(entitis);
        }

        public async Task AddRangeAsync(IEnumerable<Entity> entitis)
        {
            await _context.Set<Entity>().AddRangeAsync(entitis);
        }

        public virtual void Remove(Entity entity)
        {
            _context.Remove<Entity>(entity);
        }

        public void RemoveRange(IEnumerable<Entity> entitis)
        {
            //Go to every entity and remove them
            foreach (var entity in entitis)
                Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual void Update(Entity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Set<Entity>().Update(entity);
        }



        public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await _context.Set<Entity>().AnyAsync(predicate);
        }

        public bool Any(Expression<Func<Entity, bool>> predicate)
        {
            return _context.Set<Entity>().Any(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<Entity>().CountAsync();
        }
    }
}
