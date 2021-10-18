using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;

namespace SKPLager.API.Services
{
    public interface IRepo<Entity, IdType> where Entity : BaseModel<IdType>
    {
        /// <summary>
        /// Gets all of that entity
        /// </summary>
        /// <returns>Return All entity</returns>
        IQueryable<Entity> GetAll();

        /// <summary>
        /// Returns all entity with a spefic predicate
        /// </summary>
        /// <param name="predicate">A function that sort entity</param>
        /// <returns>Return all entity sorted with predicate</returns>
        IEnumerable<Entity> Find(Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// Gets a Entity with a spefic ID
        /// </summary>
        /// <param name="OId">The ID of the entity</param>
        /// <returns>Return the entity with the spefic ID</returns>
        Entity Get(IdType OId);

        /// <summary>
        /// Gets a Entity with a spefic ID in async
        /// </summary>
        /// <param name="OId">The ID of the entity</param>
        /// <returns>Return the entity with the spefic ID</returns>
        Task<Entity> GetAsync(IdType OId);

        /// <summary>
        /// Return true or false if the predicate is true
        /// </summary>
        /// <param name="predicate">The Function that determines if it should return true or false</param>
        /// <returns>Return true or false</returns>
        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// Return true or false if the predicate is true
        /// </summary>
        /// <param name="predicate">The Function that determines if it should return true or false</param>
        /// <returns>Return true or false</returns>
        bool Any(Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// Adds the entity to the database
        /// </summary>
        /// <param name="entity">The Entity that should be in the database</param>
        void Add(Entity entity);

        /// <summary>
        /// Adds the entity to the database
        /// </summary>
        /// <param name="entity">The Entity that should be in the database</param>
        Task AddAsync(Entity entity);

        /// <summary>
        /// Adds entitis to the database
        /// </summary>
        /// <param name="entitis">The Entity that should be in the database</param>
        void AddRange(IEnumerable<Entity> entitis);

        /// <summary>
        /// Adds entitis to the database
        /// </summary>
        /// <param name="entitis">The entitis that should be in the database</param>
        Task AddRangeAsync(IEnumerable<Entity> entitis);


        /// <summary>
        /// removes the entity from the database
        /// </summary>
        /// <param name="entity">The Entity that should be removed from the database</param>
        void Remove(Entity entity);

        /// <summary>
        /// removes entitis from the database
        /// </summary>
        /// <param name="entitis">The entitis that should be removes from the database</param>
        void RemoveRange(IEnumerable<Entity> entitis);

        /// <summary>
        /// Update the entity in the database
        /// </summary>
        /// <param name="entity">The Entity that should be in the database</param>
        void Update(Entity entity);

        /// <summary>
        /// Saves the changes to the database
        /// </summary>
        /// <returns>Returns the status</returns>
        int Save();

        /// <summary>
        /// Saves the changes to the database
        /// </summary>
        /// <returns>Returns the status</returns>
        Task<int> SaveAsync();

        /// <summary>
        /// Count number of entries
        /// </summary>
        /// <returns>Returns the status</returns>
        Task<int> CountAsync();
    }
}
