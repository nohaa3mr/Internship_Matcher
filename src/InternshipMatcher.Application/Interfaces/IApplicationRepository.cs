using InternshipMatcher.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.Interfaces
{
    public interface IApplicationRepository
    {
        public Task AddAsync(ApplicationDTO application);
         public Task<ApplicationDTO> GetByIdAsync(Guid id , CancellationToken cancellationToken);
         public Task<IQueryable<ApplicationDTO>> GetAllAsync();
         public Task UpdateAsync(ApplicationDTO application);
         public Task DeleteAsync(Guid id);
    }
}
