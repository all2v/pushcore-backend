using System.Linq.Dynamic.Core;
using Astrum.Recruting.Application.Service;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using MassTransit;
using MassTransit.Util;
using NUnit.Framework;

namespace HRM.UnitTests
{
    [TestFixture]
    public class PracticeVacancyServiceTests
    {
        private List<PracticeRequest> _expectedCandidates;
        private List<PracticeVacancy> _expectedPracticeVacancies;
        private List<PracticeSkill> _expectedSkills;
        private Guid _expectedPracticeId;
        private Guid _expectedSkillId;
        private RecrutingDbContext _context;
        private PracticeVacancyService practiceVacancyService;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new ContextGetter().GetContext<RecrutingDbContext>();
            LoadTestData();
            practiceVacancyService = new PracticeVacancyService(_context);
        }


        [Test, Order(1)]
        public void GetPracticeCandidatesByPracticeId_ReturnsPracticeCandidates()
        {
            var actualCandidates = practiceVacancyService.GetPracticeCandidatesByPracticeId(_expectedCandidates.First().PracticeVacancyId).Result;

            Assert.That(actualCandidates, Has.Count.EqualTo(_expectedCandidates.Count));
            foreach (var expectedCandidate in _expectedCandidates)
            {
                var actualCandidate = actualCandidates.First(c => c.HrmUserId == expectedCandidate.HrmUserId);
                Assert.Multiple(() =>
                {
                    Assert.That(actualCandidate.HrmUserId, Is.EqualTo(expectedCandidate.HrmUserId));
                    Assert.That(actualCandidate.PracticeDirectionId, Is.EqualTo(expectedCandidate.PracticeVacancyId));
                    Assert.That(actualCandidate.Fio, Is.EqualTo(expectedCandidate.Fio));
                    Assert.That(actualCandidate.About, Is.EqualTo(expectedCandidate.About));
                    Assert.That(actualCandidate.TaskUrl, Is.EqualTo(expectedCandidate.TaskUrl));
                    Assert.That(actualCandidate.TgLogin, Is.EqualTo(expectedCandidate.TgLogin));
                    Assert.That(actualCandidate.VacancyName, Is.EqualTo(expectedCandidate.PracticeVacancy.Name));
                });
            }
        }

        [Test, Order(2)]
        public void GetAllCandidates_ReturnsAllExistedCandidates()
        {
            var actualCandidates = practiceVacancyService.GetAllCandidates().Result;

            Assert.That(actualCandidates, Has.Count.EqualTo(_expectedCandidates.Count));
            foreach (var expectedCandidate in _expectedCandidates)
            {
                var actualCandidate = actualCandidates.First(c => c.HrmUserId == expectedCandidate.HrmUserId);
                Assert.Multiple(() =>
                {
                    Assert.That(actualCandidate.HrmUserId, Is.EqualTo(expectedCandidate.HrmUserId));
                    Assert.That(actualCandidate.Fio, Is.EqualTo(expectedCandidate.Fio));
                    Assert.That(actualCandidate.About, Is.EqualTo(expectedCandidate.About));
                    Assert.That(actualCandidate.TgLogin, Is.EqualTo(expectedCandidate.TgLogin));
                });
            }
        }


        [Test, Order(3)]
        public void GetPracticeDirections_ReturnsPracticeDirectionsByIsActiveStatus([Values] bool isActive)
        {
            var actualDirections = practiceVacancyService.GetPracticeDirections(isActive).Result;
            var expectedDirections = _expectedPracticeVacancies.Where(direction => direction.IsActive == isActive).ToList();

            Assert.That(actualDirections, Has.Count.EqualTo(expectedDirections.Count));
            foreach (var expectedDirection in expectedDirections)
            {
                var actualDirection = actualDirections.First(c => c.Id == expectedDirection.Id);
                Assert.Multiple(() =>
                {
                    Assert.That(actualDirection.Id, Is.EqualTo(expectedDirection.Id));
                    Assert.That(actualDirection.Name, Is.EqualTo(expectedDirection.Name));
                    Assert.That(actualDirection.RequestsCount, Is.EqualTo(expectedDirection.PracticeRequests.Count));
                    Assert.That(actualDirection.MaxParticipantsCount, Is.EqualTo(expectedDirection.MaxParticipantsCount));
                    Assert.That(actualDirection.CreatedAt, Is.EqualTo(expectedDirection.CreatedAt));
                    Assert.That(actualDirection.ExpiredAt, Is.EqualTo(expectedDirection.ExpiredAt));
                });
            }
        }


        [Test, Order(4)]
        public void GetPracticeSkills_ReturnsPracticeSkills()
        {
            var actualSkills = practiceVacancyService.GetPracticeSkills().Result;
            var expectedSkills = _expectedSkills.Distinct().ToList();

            Assert.That(actualSkills, Has.Count.EqualTo(expectedSkills.Count));
            foreach (var expectedSkill in expectedSkills)
            {
                var actualDirection = actualSkills.First(c => c.Id == expectedSkill.Id);
                Assert.Multiple(() =>
                {
                    Assert.That(actualDirection.Id, Is.EqualTo(expectedSkill.Id));
                    Assert.That(actualDirection.Name, Is.EqualTo(expectedSkill.Name));
                });
            }
        }


        [Test, Order(5)]
        public void CreatePractice_CheckPracticeIsSaved()
        {
            var practiceModel = new PracticeCreateModel
            {
                Name = "Java Practice",
                IsActive = true,
                ExpiredAt = new DateTime(2050, 01, 01),
                MaxParticipantsCount = 10,
                TaskUrl = "https://practice.66bit.ru/csharp",
                PracticeSkills = _expectedSkills.Take(1).Select(skill => skill.Id).ToList(),
                Responsibilities = new List<string>() { "Понимание экосистемы Java" }
            };

            var returnedPractice = practiceVacancyService.CreateOrUpdatePractice(practiceModel).Result;
            Assert.That(_context.PracticeVacancies.First(vacancy => vacancy.Id == returnedPractice.Id), Is.Not.Null);
            var savedPractice = _context.PracticeVacancies.First(vacancy => vacancy.Id.Equals(returnedPractice.Id));
            _expectedPracticeId = savedPractice.Id;
            Assert.Multiple(() =>
            {
                Assert.That(savedPractice.Name, Is.EqualTo(practiceModel.Name));
                Assert.That(savedPractice.IsActive, Is.EqualTo(practiceModel.IsActive));
                Assert.That(savedPractice.ExpiredAt, Is.EqualTo(practiceModel.ExpiredAt));
                Assert.That(savedPractice.MaxParticipantsCount, Is.EqualTo(practiceModel.MaxParticipantsCount));
                Assert.That(savedPractice.TaskUrl, Is.EqualTo(practiceModel.TaskUrl));
                Assert.That(savedPractice.PracticeSkills.Select(s => s.Id), Is.EqualTo(practiceModel.PracticeSkills));
                Assert.That(savedPractice.Responsibilities.Select(r => r.Name), Is.EqualTo(practiceModel.Responsibilities));
            });
        }


        [Test, Order(6)]
        public void UpdatePractice_CheckPracticeChagesIsSaved()
        {
            var practiceModel = new PracticeCreateModel
            {
                Id = _expectedPracticeId,
                Name = "Java",
                IsActive = true,
                ExpiredAt = new DateTime(2025, 01, 01),
                MaxParticipantsCount = 15,
                TaskUrl = "https://practice.66bit.ru/java",
                PracticeSkills = _expectedSkills.Select(skill => skill.Id).ToList(),
                Responsibilities = new List<string>() { "Понимание экосистемы Java", "Разработка и тестирование ПО" }
            };

            var returnedPractice = practiceVacancyService.CreateOrUpdatePractice(practiceModel).Result;
            Assert.That(returnedPractice.Id, Is.EqualTo(_expectedPracticeId));
            Assert.That(_context.PracticeVacancies.First(vacancy => vacancy.Id == returnedPractice.Id), Is.Not.Null);
            var updatedPractice = _context.PracticeVacancies.First(vacancy => vacancy.Id.Equals(returnedPractice.Id));
            Assert.Multiple(() =>
            {
                Assert.That(updatedPractice.Name, Is.EqualTo(practiceModel.Name));
                Assert.That(updatedPractice.IsActive, Is.EqualTo(practiceModel.IsActive));
                Assert.That(updatedPractice.ExpiredAt, Is.EqualTo(practiceModel.ExpiredAt));
                Assert.That(updatedPractice.MaxParticipantsCount, Is.EqualTo(practiceModel.MaxParticipantsCount));
                Assert.That(updatedPractice.TaskUrl, Is.EqualTo(practiceModel.TaskUrl));
                Assert.That(updatedPractice.PracticeSkills.Select(s => s.Id), Is.EqualTo(practiceModel.PracticeSkills));
                Assert.That(updatedPractice.Responsibilities.Select(r => r.Name), Is.EqualTo(practiceModel.Responsibilities));
            });
        }


        [Test, Order(7)]
        public void CreateSkill_CheckSkillIsSaved()
        {
            var skillModel = new SkillModel
            {
                Name = "WebSockets"
            };

            var returnedSkill = practiceVacancyService.CreateOrUpdateSkill(skillModel).Result;
            Assert.That(_context.PracticeSkills.First(skill => skill.Id == returnedSkill.Id), Is.Not.Null);
            var savedSkill = _context.PracticeSkills.First(skill => skill.Id.Equals(returnedSkill.Id));
            _expectedSkillId = savedSkill.Id;
            Assert.That(savedSkill.Name, Is.EqualTo(skillModel.Name));
        }


        [Test, Order(8)]
        public void UpdateSkill_CheckSkillChagesIsSaved()
        {
            var skillModel = new SkillModel
            {
                Id = _expectedSkillId,
                Name = "Реализация клиента и сервера WebSocket на ASP.NET"
            };

            var returnedSkill = practiceVacancyService.CreateOrUpdateSkill(skillModel).Result;
            Assert.That(returnedSkill.Id, Is.EqualTo(_expectedSkillId));
            Assert.That(_context.PracticeSkills.First(skill => skill.Id == returnedSkill.Id), Is.Not.Null);
            var savedSkill = _context.PracticeSkills.First(skill => skill.Id.Equals(returnedSkill.Id));
            Assert.That(savedSkill.Name, Is.EqualTo(skillModel.Name));
        }


        [Test, Order(9)]
        public void RemovePracticeSkills_CheckSkillIsRemoved()
        {
            var skillId = _context.PracticeSkills.First().Id;
            practiceVacancyService.RemovePracticeSkills(skillId).Wait();
            Assert.That(_context.PracticeSkills.Where(skill => skill.Id == skillId), Is.Empty);
        }


        [Test, Order(10)]
        public void MovePracticeDirectionToArchive_CheckStatusIsNotActive()
        {
            var practiceToArchive = _context.PracticeVacancies.First();
            practiceVacancyService.MovePracticeDirectionToArchive(practiceToArchive.PracticeId).Wait();
            Assert.That(_context.PracticeVacancies.Where(vacancy => vacancy.Id == practiceToArchive.Id), Is.Not.Null);
            var savedPracticeVacancy = _context.PracticeVacancies.First(vacancy => vacancy.Id.Equals(practiceToArchive.Id));
            Assert.Multiple(() =>
            {
                Assert.That(savedPracticeVacancy.IsActive, Is.EqualTo(false));
                Assert.That(savedPracticeVacancy.Name, Is.EqualTo(practiceToArchive.Name));
                Assert.That(savedPracticeVacancy.PracticeId, Is.EqualTo(practiceToArchive.PracticeId));
                Assert.That(savedPracticeVacancy.CreatedAt, Is.EqualTo(practiceToArchive.CreatedAt));
                Assert.That(savedPracticeVacancy.ExpiredAt, Is.EqualTo(practiceToArchive.ExpiredAt));
                Assert.That(savedPracticeVacancy.MaxParticipantsCount, Is.EqualTo(practiceToArchive.MaxParticipantsCount));
                Assert.That(savedPracticeVacancy.PracticeRequests, Is.EqualTo(practiceToArchive.PracticeRequests));
                Assert.That(savedPracticeVacancy.PracticeSkills, Is.EqualTo(practiceToArchive.PracticeSkills));
            });
        }


        [Test, Order(11)]
        public void SetPracticesWithDeadlineNotActive_CheckDeadlinePracticesIsNotActive()
        {
            var vacancies = _context.PracticeVacancies.ToList();
            practiceVacancyService.SetPracticesWithDeadlineNotActive(vacancies).Wait();
            Assert.That(vacancies.Any(vacancy => vacancy.ExpiredAt >= DateTimeOffset.UtcNow.Date && vacancy.IsActive == true), Is.False);
        }


        [Test, Order(12)]
        public void AddPracticeVacancy_CheckPracticeVacancyChangesIsSaved()
        {
            var vacancy = _context.PracticeVacancies.First();
            var oldId = vacancy.Id;
            var oldCreatedAt = vacancy.CreatedAt;
            var oldExpiredAt = vacancy.ExpiredAt;
            practiceVacancyService.AddPracticeVacancy(vacancy).Wait();

            Assert.Multiple(() =>
            {
                Assert.That(oldId, Is.Not.EqualTo(vacancy.Id));
                Assert.That(oldCreatedAt, Is.Not.EqualTo(vacancy.CreatedAt));
                Assert.That(oldExpiredAt, Is.Not.EqualTo(vacancy.ExpiredAt));
            });
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        private void LoadTestData()
        {
            var testHrmUser1 = new HrmUser
            {
                Name = "Иван",
                Srurname = "Петрович",
                LastName = "Дулин",
                Gender = Gender.Male,
                IsActive = true,
                ProfileId = null,
                Email = "i.p.dulin@mail.ru",
                TgLogin = "@dulin",
                CandidateResumes = new(),
                UserComments = new()
            };
            var testHrmUser2 = new HrmUser
            {
                Name = "Иван",
                Srurname = "Иванович",
                LastName = "Иванов",
                Gender = Gender.Male,
                IsActive = true,
                ProfileId = null,
                Email = "i.i.ivanov@mail.ru",
                TgLogin = "@ivanov",
                CandidateResumes = new(),
                UserComments = new()
            };
            _context.Add(testHrmUser1);
            _context.Add(testHrmUser2);
            _context.SaveChanges();

            var testPracticeSkills = new List<PracticeSkill>
            {
                new PracticeSkill { Name = "LINQ" },
                new PracticeSkill { Name = "SQL" }
            };
            var testPracticeVacancy = new PracticeVacancy
            {
                Name = "C#",
                PracticeId = Guid.Empty,
                IsActive = true,
                CreatedAt = DateTimeOffset.Now,
                ExpiredAt = new DateTime(2050, 01, 01),

                MaxParticipantsCount = 15,
                TaskUrl = "https://practice.66bit.ru/csharp",
                PracticeSkills = testPracticeSkills,
                Responsibilities = new(),
                PracticeRequests = new()
            };
            var testPracticeRequest1 = new PracticeRequest
            {
                Fio = "Дулин Иван Петрович",
                TgLogin = "@dulin",
                TaskUrl = "https://github.com/dulin/csharptest",
                IsApproved = false,
                About = "Middle .NET Разработчик",
                HrmUserId = testHrmUser1.Id,
                PracticeVacancy = testPracticeVacancy,
                //PracticeVacancyId
                CreatedAt = DateTimeOffset.Now
            };
            var testPracticeRequest2 = new PracticeRequest
            {
                Fio = "Иванов Иван Иванович",
                TgLogin = "@ivanov",
                TaskUrl = "https://github.com/dulin/csharptest",
                IsApproved = false,
                About = "Зовут Санёк, немного о себе...",
                HrmUserId = testHrmUser2.Id,
                PracticeVacancy = testPracticeVacancy,
                //PracticeVacancyId
                CreatedAt = DateTimeOffset.Now
            };
            _context.Add(testPracticeRequest1);
            _context.Add(testPracticeRequest2);
            _context.SaveChanges();

            _expectedCandidates = _context.PracticeRequests.ToList();
            _expectedPracticeVacancies = _context.PracticeVacancies.ToList();
            _expectedSkills = _context.PracticeSkills.ToList();
        }
    }
}