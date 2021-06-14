using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///  This acts like our register for the different data tables...
        ///  All the operations in GenericRepository are essentially staged changes, not actually saved... That's what this is for. 
        /// </summary>

        IGenericRepository<Country> Countries { get; }

        IGenericRepository<Hotel> Hotels { get; }

        Task Save();
    }
}
