using Nop.Core;
using Nop.Core.Domain.Students;
using Nop.Data;

namespace Nop.Services.Students;

public partial class StudentService : IStudentService
{
    #region Fields

    protected readonly IRepository<Student> _studentRepository;

    #endregion

    #region Ctor

    public StudentService(
        IRepository<Student> studentRepository
        )
    {
        _studentRepository = studentRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Student>> GetAllStudentsAsync(
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



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _studentRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (customerId > 0)
                query = query.Where(x => customerId == x.CustomerId);

            if (customerIds != null && customerIds.Any())
                query = query.Where(x => customerIds.Contains(x.CustomerId));


            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(x => firstName == x.FirstName);

            if (firstNames != null && firstNames.Any())
                query = query.Where(x => firstNames.Contains(x.FirstName));

            if (!string.IsNullOrWhiteSpace(middleName))
                query = query.Where(x => middleName == x.MiddleName);

            if (middleNames != null && middleNames.Any())
                query = query.Where(x => middleNames.Contains(x.MiddleName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(x => lastName == x.LastName);

            if (lastNames != null && lastNames.Any())
                query = query.Where(x => lastNames.Contains(x.LastName));

            if (previousSchoolId > 0)
                query = query.Where(x => previousSchoolId == x.PreviousSchoolId);

            if (previousSchoolIds != null && previousSchoolIds.Any())
                query = query.Where(x => previousSchoolIds.Contains(x.PreviousSchoolId));

            if (!string.IsNullOrWhiteSpace(identificationMark))
                query = query.Where(x => identificationMark == x.IdentificationMark);

            if (identificationMarks != null && identificationMarks.Any())
                query = query.Where(x => identificationMarks.Contains(x.IdentificationMark));


            if (!string.IsNullOrWhiteSpace(allergies))
                query = query.Where(x => allergies == x.Allergies);

            if (allergieses != null && allergieses.Any())
                query = query.Where(x => allergieses.Contains(x.Allergies));

            if (!string.IsNullOrWhiteSpace(medicalNotes))
                query = query.Where(x => medicalNotes == x.MedicalNotes);

            if (medicalNoteses != null && medicalNoteses.Any())
                query = query.Where(x => medicalNoteses.Contains(x.MedicalNotes));

            if (guardianTypeId > 0)
                query = query.Where(x => guardianTypeId == x.GuardianTypeId);

            if (guardianTypeIds != null && guardianTypeIds.Any())
                query = query.Where(x => guardianTypeIds.Contains(x.GuardianTypeId));

            query = query.WhereBoolean(x => x.IsActive, isActive);

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Student> GetStudentByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _studentRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Student>> GetStudentsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _studentRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertStudentAsync(Student student)
    {
        if (student == null)
            return;

        await _studentRepository.InsertAsync(student);
    }
    
    public virtual async Task InsertStudentAsync(IEnumerable<Student> students)
    {
        if (students == null || !students.Any())
            return;

        await _studentRepository.InsertAsync(students.ToList());
    }

    public virtual async Task UpdateStudentAsync(Student student)
    {
        if (student == null)
            return;

        await _studentRepository.UpdateAsync(student);
    }

    public virtual async Task UpdateStudentAsync(IEnumerable<Student> students)
    {
        if (students == null || !students.Any())
            return;

        await _studentRepository.UpdateAsync(students.ToList());
    }

    public virtual async Task DeleteStudentAsync(Student student)
    {
        if (student == null)
            return;

        await _studentRepository.DeleteAsync(student);
    }

    public virtual async Task DeleteStudentAsync(IEnumerable<Student> students)
    {
        if (students == null || !students.Any())
            return;

        await _studentRepository.DeleteAsync(students.ToList());
    }

    #endregion
}