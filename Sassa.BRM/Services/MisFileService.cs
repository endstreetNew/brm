using Microsoft.AspNetCore.Http;
using Sassa.BRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
