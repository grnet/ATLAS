
namespace StudentPractice.BusinessModel
{
    public enum enPositionTransferResult
    {
        Success = 0,
        InvalidStatus,
        NewOfficeIsTheSame,
        NewOfficeIsNotVerified,
        NewOfficeDoesNotServeAcademic,
        NewOfficeDoesNotServeStudent
    }
}
