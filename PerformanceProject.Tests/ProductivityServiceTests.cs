using NUnit.Framework;
using PeopleManagmentSystem_API.Services.Interfaces;
using Moq;
using PerformanceProject.Services;
using PerformanceProject.Shared.Models;
using System.Collections.Generic;
using Task = PerformanceProject.Shared.Models.Task;
using MongoDB.Bson;
using DnsClient;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using PeopleManagmentSystem_API.Services;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;

namespace PerformanceProject.Tests
{
    [TestFixture]
    public class ProductivityServiceTests
    {
        private ProductivityService _productivityService;
        private Mock<IProjectService> _mockProjectService;

        [SetUp]
        public void Setup()
        {
            _mockProjectService = new Mock<IProjectService>();

            _productivityService = new ProductivityService(_mockProjectService.Object);
        }

        //[Test]
        //public void CalculateIndividualProductivity_ShouldReturnCorrectValue()
        //{
        //    // Arrange
        //    double minProductivity = 0;
        //    double maxProductivity = 9;
        //    int completedTasksCount = 0;
        //    string projectId = "67441a2aaab865382f40f90f";
        //    string userId = "67441a318c542543223e6234";

        //    // Створюємо тестові дані
        //    var mockProject = new Project
        //    {
        //        Id = ObjectId.Parse(projectId),
        //        Team = new Team
        //        {
        //            Members = new List<TeamMember>
        //            {
        //                new TeamMember { UserId = userId }
        //            }
        //        },
        //        Tasks = new List<Task>
        //        {
        //            new Task
        //            {
        //                AssigneeId = userId,
        //                Difficulty = DifficultyLevel.Medium,
        //                Rating = 4,
        //                Status = Shared.Models.TaskStatus.Completed,
        //                Accepted = DateTime.Now.AddHours(-8),
        //                DueDate = DateTime.Now.AddHours(4)
        //            },
        //            new Task
        //            {
        //                AssigneeId = userId,
        //                Difficulty = DifficultyLevel.High,
        //                Rating = 5,
        //                Status = Shared.Models.TaskStatus.Completed,
        //                Accepted = DateTime.Now.AddHours(-20),
        //                DueDate = DateTime.Now.AddHours(-10)
        //            }
        //        }
        //    };

        //    _mockProjectService
        //        .Setup(service => service.GetAsync(It.Is<string>(id => id == projectId)))
        //        .ReturnsAsync(mockProject);

        //    var weights = new ProductivityWeights();

        //    // Act
        //    var productivity = _productivityService.CalculateIndividualProductivity(projectId, userId);

        //    // Assert
        //    Assert.That(productivity, Is.EqualTo(2).Within(0.5));
        //}

        [TestCase(DifficultyLevel.Medium, -8, 4, -2, 0.9, 4.0, 2.5)]
        [TestCase(DifficultyLevel.High, -20, -5, -1, 0.7, 3.5, 2.0)]
        [TestCase(DifficultyLevel.High, -15, 0, -10, 1.0, 5.0, 3.3)]
        [TestCase(DifficultyLevel.Low, -5, 0, -1, 1.0, 4.1, 2.3)]
        [TestCase(DifficultyLevel.Low, -5, 0, +5, 0.75, 3.0, 1.5)]
        public void CalculateIndividualProductivity_VariousScenarios_ShouldReturnExpectedProductivity(DifficultyLevel difficulty, int acceptedOffset, int dueDateOffset, int finishedOffset, double engagement, double rating, double expectedProductivity)
        {
            // Arrange
            string projectId = "67441a2aaab865382f40f90f";
            string userId = "67441a318c542543223e6234";

            var mockProject = new Project
            {
                Id = ObjectId.Parse(projectId),
                Team = new Team
                {
                    Members = new List<TeamMember>
            {
                new TeamMember { UserId = userId }
            }
                },
                Tasks = new List<Task>
        {
            new Task
            {
                AssigneeId = userId,
                Difficulty = difficulty,
                Engagement = engagement,
                Rating = rating,
                Status = Shared.Models.TaskStatus.Completed,
                Accepted = DateTime.Now.AddHours(acceptedOffset),
                DueDate = DateTime.Now.AddHours(dueDateOffset),
                Finished = DateTime.Now.AddHours(finishedOffset)
            }
        }
            };

            _mockProjectService
                .Setup(service => service.GetAsync(It.Is<string>(id => id == projectId)))
                .ReturnsAsync(mockProject);

            var weights = new ProductivityWeights();

            // Act
            var productivity = _productivityService.CalculateIndividualProductivity(projectId, userId);

            // Assert
            Assert.That(productivity, Is.EqualTo(expectedProductivity).Within(0.3));
        }

        [Test]
        public void CalculateTeamProductivity_ShouldReturnExpectedProductivity()
        {
            // Arrange
            var projectId = "67441a2aaab865382f40f90f";
            var mockProject = new Project
            {
                Id = ObjectId.Parse(projectId),
                Team = new Team
                {
                    Members = new List<TeamMember>
            {
                new TeamMember { UserId = "user1" },
                new TeamMember { UserId = "user2" }
            }
                },
                Tasks = new List<Task>
        {
            new Task
            {
                AssigneeId = "user1", Difficulty = DifficultyLevel.Medium, Status = Shared.Models.TaskStatus.Completed, Engagement = 0.8, Rating = 4, Accepted = DateTime.Now.AddHours(-10), DueDate = DateTime.Now.AddHours(2), Finished = DateTime.Now.AddHours(-2),
            },
            new Task
            { AssigneeId = "user2", Difficulty = DifficultyLevel.High, Status = Shared.Models.TaskStatus.Completed, Rating = 5, Engagement = 0.7, Accepted = DateTime.Now.AddHours(-20), DueDate = DateTime.Now.AddHours(-5), Finished = DateTime.Now.AddHours(-2)
            }
        }
            };
            _mockProjectService
                .Setup(service => service.GetAsync(It.Is<string>(id => id == projectId)))
                .ReturnsAsync(mockProject);

            // Act
            var result = _productivityService.CalculateTeamProductivity(projectId);

            // Assert
            Assert.That(result, Is.EqualTo(2.5).Within(0.3));
        }


    }

    [TestFixture]
    public class ProjectServiceTests
    {
        private Mock<IProjectService> _mockProjectService;

        [SetUp]
        public void SetUp()
        {
            _mockProjectService = new Mock<IProjectService>();
        }

        [Test]
        public void GetProjects_ShouldReturnProjects_WhenDataExists()
        {
            // Arrange
            var expectedProjects = new List<Project>
        {
            new Project
            {
                Id = ObjectId.GenerateNewId(),
                Name = "E-commerce Platform",
                Description = "Development of an online store",
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                Status = ProjectStatus.InProgress
            },
            new Project
            {
                Id = ObjectId.GenerateNewId(),
                Name = "CRM System",
                Description = "Custom CRM solution for a client",
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(40)),
                Status = ProjectStatus.InProgress
            }
        };

            _mockProjectService
                .Setup(service => service.GetAsync())
                .ReturnsAsync(expectedProjects);

            var projectService = _mockProjectService.Object;

            // Act
            var result = projectService.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("E-commerce Platform"));
            Assert.That(result[1].Name, Is.EqualTo("CRM System"));

            // Verify the interaction
            _mockProjectService.Verify(service => service.GetAsync(), Times.Once);
        }

        [Test]
        public void GetProjects_ShouldReturnEmptyList_WhenNoProjectsExist()
        {
            // Arrange
            _mockProjectService
                .Setup(service => service.GetAsync())
                .ReturnsAsync(new List<Project>());

            var projectService = _mockProjectService.Object;

            // Act
            var result = projectService.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));

            // Verify the interaction
            _mockProjectService.Verify(service => service.GetAsync(), Times.Once);
        }

        [Test]
        public void UpdateWeights_ShouldUpdateProjectWeights_WhenValidProjectIdAndWeightsProvided()
        {
            // Arrange
            string projectId = ObjectId.GenerateNewId().ToString();
            var existingProject = new Project
            {
                Id = ObjectId.Parse(projectId),
                Name = "E-commerce Platform",
                Weights = new ProductivityWeights
                {
                    TaskWeight = 1.0,
                    TimeWeight = 0.8,
                    QualityWeight = 1.2,
                    EngagementWeight = 0.6
                }
            };

            var updatedWeights = new ProductivityWeights
            {
                TaskWeight = 1.2,
                TimeWeight = 1.0,
                QualityWeight = 1.5,
                EngagementWeight = 0.8
            };

            _mockProjectService
                .Setup(service => service.GetAsync(It.Is<string>(id => id == projectId)))
                .ReturnsAsync(existingProject);

            _mockProjectService
                .Setup(service => service.UpdateAsync(projectId, It.Is<Project>(p => p.Id == existingProject.Id)));

            var projectService = _mockProjectService.Object;

            // Act
            var result = projectService.GetAsync(projectId).Result;
            result.Weights = updatedWeights;

            projectService.UpdateAsync(projectId, result);

            // Assert
            Assert.That(result.Weights.TaskWeight, Is.EqualTo(1.2));
            Assert.That(result.Weights.TimeWeight, Is.EqualTo(1.0));
            Assert.That(result.Weights.QualityWeight, Is.EqualTo(1.5));
            Assert.That(result.Weights.EngagementWeight, Is.EqualTo(0.8));

            // Verify interactions
            _mockProjectService.Verify(service => service.GetAsync(It.Is<string>(id => id == projectId)), Times.Once);
            _mockProjectService.Verify(service => service.UpdateAsync(projectId, It.Is<Project>(p => p.Id == existingProject.Id)), Times.Once);
        }

        [Test]
        public void GetTaskDetails_ShouldReturnTasks_WhenValidProjectIdProvided()
        {
            // Arrange
            string projectId = ObjectId.GenerateNewId().ToString();
            var mockTasks = new List<Task>
    {
        new Task
        {
            Id = ObjectId.GenerateNewId(),
            Title = "Task 1",
            Description = "Description for Task 1",
            AssigneeId = "user1",
            Difficulty = DifficultyLevel.Medium,
            Status = Shared.Models.TaskStatus.Completed,
            Engagement = 0.8,
            Rating = 4,
            Accepted = DateTime.Now.AddHours(-10),
            DueDate = DateTime.Now.AddHours(2),
            Finished = DateTime.Now.AddHours(-2)
        },
        new Task
        {
            Id = ObjectId.GenerateNewId(),
            Title = "Task 2",
            Description = "Description for Task 2",
            AssigneeId = "user2",
            Difficulty = DifficultyLevel.High,
            Status = Shared.Models.TaskStatus.InProgress,
            Engagement = 0.9,
            Rating = 5,
            Accepted = DateTime.Now.AddHours(-5),
            DueDate = DateTime.Now.AddHours(5),
        }
    };

            var mockProject = new Project
            {
                Id = ObjectId.Parse(projectId),
                Name = "Sample Project",
                Tasks = mockTasks
            };

            _mockProjectService
                .Setup(service => service.GetAsync(It.Is<string>(id => id == projectId)))
                .ReturnsAsync(mockProject);

            var projectService = _mockProjectService.Object;

            // Act
            var result = projectService.GetAsync(projectId).Result;

            // Assert
            Assert.That(result.Tasks, Is.Not.Null);
            Assert.That(result.Tasks.Count, Is.EqualTo(2));
            Assert.That(result.Tasks[0].Title, Is.EqualTo("Task 1"));
            Assert.That(result.Tasks[1].Status, Is.EqualTo(Shared.Models.TaskStatus.InProgress));

            // Verify interactions
            _mockProjectService.Verify(service => service.GetAsync(It.Is<string>(id => id == projectId)), Times.Once);
        }




    }


}
