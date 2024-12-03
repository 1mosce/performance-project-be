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

namespace PerformanceProject.Tests.Services
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


}
