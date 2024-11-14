using LabAllianceTest.API.Abstractions;
using LabAllianceTest.API.Entities;
using LabAllianceTest.API.Exceptions;
using LabAllianceTest.API.Helpers;
using LabAllianceTest.API.Models;

namespace LabAlliance.MockData.Repositories
{
    public class UserRepositoryTest
    {
        private readonly IPasswordHasher _passwordHasher;

        List<UserEntity> Users = new List<UserEntity>
{
    new UserEntity{ Id = Guid.NewGuid(), Login = "TestUnitLogin", Password = "$2a$11$YdwWDRtGXMF75npnzWyYUeYxeBpA842XkNA19eAHo7DQvJxJJjv/6"},
    new UserEntity{ Id = Guid.NewGuid(), Login = "TestUnitLogin1", Password = "$2a$11$YdwWDRtGXMF75npnzWyYUeYxeBpA842XkNA19eAHo7DQvJxJJjv/6"},
    new UserEntity{ Id = Guid.NewGuid(), Login = "TestUnitLogin2", Password = "$2a$11$YdwWDRtGXMF75npnzWyYUeYxeBpA842XkNA19eAHo7DQvJxJJjv/6"},
};

        public UserRepositoryTest()
        {
            _passwordHasher = new PasswordHasher();
        }

        public async Task<Guid> CreateUserAsync(UserModel user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Login.Equals(user.Login));

            if (existingUser != null) { throw new RepositoryException("Пользователь с таким логином уже существует."); }
            else
            {
                UserEntity userEntity = new UserEntity
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = _passwordHasher.Generate(user.Password)
                };

                await Task.Delay(100);

                return userEntity.Id;
            }
        }

        public async Task<UserModel> LoginUserAsync(string login, string password)
        {
            await Task.Delay(100);

            var existingUser = Users.FirstOrDefault(u => u.Login.Equals(login))
                ?? throw new AuthenticationFailedException("Неверный логин или пароль."); ;

            var user = UserModel.Create(
                existingUser.Id,
                existingUser.Login,
                existingUser.Password,
                false).user;

            return user;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            await Task.Delay(100);

            var userEntities = Users.ToList();

            var users = userEntities.Select(u => UserModel.Create(u.Id, u.Login, u.Password, false).user).ToList();

            return users;
        }
    }
}
