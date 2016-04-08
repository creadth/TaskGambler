
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using InoGambling.Data.Models;

namespace InoGambling.Data
{
    class InoGamblingDbInitializer : DropCreateDatabaseIfModelChanges<InoGamblingDbContext>
    {
        protected override void Seed(InoGamblingDbContext context)
        {
            try
            {
                base.Seed(context);

                var project = new Project()
                {
                    ShortId = "TST",
                    ProjectName = "Test Project",

                };
                var user1 = new User()
                {
                    Login = "test1@test.com",
                    Password = "123",
                    Points = 1000,
                };
                var user2 = new User()
                {
                    Login = "test2@test.com",
                    Password = "123",
                    Points = 10000
                };

                context.Set<Project>().AddOrUpdate(x => x.ShortId, project);
                context.Set<User>().AddOrUpdate(x => x.Login, user1, user2);
                context.SaveChanges();

                project = context.Set<Project>().FirstOrDefault(x => x.ShortId == "TST");
                user1 = context.Set<User>().FirstOrDefault(x => x.Login == "test1@test.com");
                user2 = context.Set<User>().FirstOrDefault(x => x.Login == "test2@test.com");

                var integrationUser1Slack = new IntegrationUser()
                {
                    Name = "test1@test.com",
                    Type = IntegrationType.Slack,
                    UserId = user1.Id
                };
                var integrationUser1Youtrack = new IntegrationUser()
                {
                    Name = "test1@test.com",
                    Type = IntegrationType.Youtrack,
                    UserId = user1.Id
                };
                var integrationUser2Slack = new IntegrationUser()
                {
                    Name = "test2@test.com",
                    Type = IntegrationType.Slack,
                    UserId = user1.Id
                };
                var integrationUser2Youtrack = new IntegrationUser()
                {
                    Name = "test2@test.com",
                    Type = IntegrationType.Youtrack,
                    UserId = user1.Id
                };

                context.Set<IntegrationUser>().AddOrUpdate(
                    x => new {x.Name, x.Type, x.UserId},
                    integrationUser1Slack,
                    integrationUser1Youtrack,
                    integrationUser2Slack,
                    integrationUser2Youtrack);

                var task = new Task()
                {
                    AssigneeUserId = user1.Id,
                    ProjectId = project.Id,
                    ShortId = "TST-1",
                    State = TaskState.Created,
                    Estimate = 5
                };

                context.Set<Task>().AddOrUpdate(x => x.ShortId, task);

                context.SaveChanges();

                task = context.Set<Task>().FirstOrDefault(x => x.ShortId == "TST-1");

                var bet = new Bet()
                {
                    UserId = user2.Id,
                    TaskId = task.Id,
                    Estimate = 4,
                    Points = 100
                };
                context.Set<Bet>().AddOrUpdate(x => new {x.UserId, x.TaskId}, bet);

                context.SaveChanges();
            }
            catch (Exception e)
            {
                var tmp = e;
            }
        }
    }
}
