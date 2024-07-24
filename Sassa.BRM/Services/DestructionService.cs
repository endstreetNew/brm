using Sassa.Brm.Common.Helpers;
using Sassa.BRM.Models;
using System.Threading.Tasks;

namespace Sassa.BRM.Services
{
    public class DestructionService
    {

        ModelContext _context;
        RawSqlService _raw;
        public DestructionService(ModelContext context, RawSqlService raw)
        {
            //if (StaticD.Users == null) StaticD.Users = new List<string>();
            _context = context;
            _raw = raw;
        }

        public async Task DestroyXlsxFile(string fileName, string columnName = "ID")
        {
            var DestroyList = XlsxHelper.ReadDestroyList(fileName, columnName);
            foreach (var item in DestroyList)
            {
                //await _raw.ExecuteNonQuery($"Update DC_File set Application_Status = 'DESTROY' where Applicant_no = '{item}'");
                //await _raw.ExecuteNonQuery($"Update dc_socpen set status_code = 'DESTROY' where Beneficiary_id = '{item}'");

                var first = _raw.ExecuteNonQuery($"Update DC_File set Application_Status = 'DESTROY' where Applicant_no = '{item}'");
                var second = _raw.ExecuteNonQuery($"Update dc_socpen set status_code = 'DESTROY' where Beneficiary_id = '{item}'");

                await Task.WhenAll(first, second);
            }
        }
    }
}
