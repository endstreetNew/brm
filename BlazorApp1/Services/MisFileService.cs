using Microsoft.EntityFrameworkCore;
using Sassa.BRM.Models;

namespace Sassa.BRM.Services
{
    public class MisFileService
    {
        ModelContext _context;
        public MisFileService(ModelContext context)
        {
            //if (StaticD.Users == null) StaticD.Users = new List<string>();
            _context = context;

        }

        public async Task<List<MisLivelinkTbl>> GetMisFiles(string idNumber)
        {
            return await _context.MisLivelinkTbls.Where(x => x.IdNumber == idNumber).ToListAsync();
        }
    }
}
