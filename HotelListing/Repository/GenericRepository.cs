using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        /// <summary>
        /// whatever we load up in the startup, it's now available applciation wide. All we do is make
        /// reference to the existing object in the startup.
        /// Our bridge to the Database we just get a copy of it, instead of getting a new instance. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }


        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            // get all records in table
            IQueryable<T> query = _db;
            // did the calling code, would they like to include additional details | we can save some additional queries here.
            if (includes != null) {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            // the expression is a lambda expression that lets you put in something like
            // query => query.Name etc.
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expresssion = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
           
            IQueryable<T> query = _db;
            // this basically brings our record set down from 500, to 20 if there is an expression. 
            if (expresssion != null)
            {
                query = query.Where(expresssion);
            }
            // Then put the includes on 
        
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            // then do the order by if needed.
            if (orderBy != null)
            {
                query = orderBy(query);
            }
           
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            // this takes two lines, basically what is in memory, could be different than what's actually in the db
            // so we're doing two steps here to check that we have the correct one.
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
