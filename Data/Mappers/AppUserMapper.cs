using ExamApi.Data.Entity;
using ExamApi.DTO.Auth;

namespace ExamApi.Data.Mappers
{
    public static class AppUserMapper
    {
        public static AppUser ToAppUser(this RegisterRequest request)
        {
            return new AppUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName
            };
        }
    }
}
