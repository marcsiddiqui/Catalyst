using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Students;

namespace Nop.Services.Students;

public partial interface IStudentService
{
    Task<IPagedList<Student>> GetAllStudentsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int customerId = 0, IEnumerable<int> customerIds = null,

        string firstName = null, IEnumerable<string> firstNames = null,
        string middleName = null, IEnumerable<string> middleNames = null,
        string lastName = null, IEnumerable<string> lastNames = null,
        int previousSchoolId = 0, IEnumerable<int> previousSchoolIds = null,
        string identificationMark = null, IEnumerable<string> identificationMarks = null,

        string allergies = null, IEnumerable<string> allergieses = null,
        string medicalNotes = null, IEnumerable<string> medicalNoteses = null,
        int guardianTypeId = 0, IEnumerable<int> guardianTypeIds = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Student> GetStudentByIdAsync(int id);

    Task<IList<Student>> GetStudentsByIdsAsync(IEnumerable<int> ids);

    Task InsertStudentAsync(Student student);
    
    Task InsertStudentAsync(IEnumerable<Student> students);

    Task UpdateStudentAsync(Student student);

    Task UpdateStudentAsync(IEnumerable<Student> students);

    Task DeleteStudentAsync(Student student);

    Task DeleteStudentAsync(IEnumerable<Student> students);
}