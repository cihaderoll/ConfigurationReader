using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Domain.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets related entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets related entity list by Id list
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetByIdListAsync(List<int> idList);

        /// <summary>
        /// Gets all entity list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to storage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddAsync(TEntity model);

        /// <summary>
        /// Cehcks if any record with the same name exist in storage
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        Task<bool> CheckForDuplicateConfigNameAsync(string configName);
    }
}
