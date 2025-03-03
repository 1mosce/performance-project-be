﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PerformanceProject.Shared.Models;
using PeopleManagmentSystem_API.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

using Task = PerformanceProject.Shared.Models.Task;

namespace PeopleManagmentSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all Projects")]
        public async Task<ActionResult<List<Project>>> Get()
        {
            var projects = await _projectService.GetAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Project by Id")]
        public async Task<ActionResult<Project>> GetById(string id)
        {
            var project = await _projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            return Ok(project);
        }


        [HttpGet("{id}/tasks")]
        [SwaggerOperation(Summary = "Get Project's Tasks")]
        public async Task<ActionResult<List<Task>>> GetTasks(string id)
        {
            var project = await _projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            var tasks = await _projectService.GetTasksAsync(id);
            return Ok(tasks);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a New Project")]
        public async Task<ActionResult<Project>> Post([FromBody] Project project)
        {
            await _projectService.CreateAsync(project);

            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Modify a Project")]
        public async Task<ActionResult> Put(string id, [FromBody] Project project)
        {
            var existingProject = await _projectService.GetAsync(id);

            if (existingProject == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            await _projectService.UpdateAsync(id, project);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Project")]
        public async Task<ActionResult> Delete(string id)
        {
            var project = await _projectService.GetAsync(id);

            if (project == null)
            {
                return NotFound($"Project with Id = {id} not found");
            }

            await _projectService.RemoveAsync(project.SerializedId);

            return NoContent();
        }

        // Methodology

        [HttpPut("{projectId}/methodology")]
        [SwaggerOperation(Summary = "Set or Update the Methodology for a Project")]
        public async Task<IActionResult> SetMethodology(string projectId, [FromBody] Methodology methodology)
        {
            await _projectService.SetMethodologyAsync(projectId, methodology);
            return NoContent();
        }

        [HttpDelete("{projectId}/methodology")]
        [SwaggerOperation(Summary = "Remove the Methodology from a Project")]
        public async Task<IActionResult> RemoveMethodology(string projectId)
        {
            await _projectService.RemoveMethodologyAsync(projectId);
            return NoContent();
        }

        [HttpGet("{projectId}/methodology")]
        [SwaggerOperation(Summary = "Get the Methodology of a Project")]
        public async Task<ActionResult<Methodology?>> GetMethodology(string projectId)
        {
            try
            {
                var methodology = await _projectService.GetMethodologyAsync(projectId);
                return Ok(methodology);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Team
        [HttpGet("{id}/team")]
        [SwaggerOperation(Summary = "Get Project's Team")]
        public async Task<IActionResult> GetTeam(string id)
        {
            var team = await _projectService.GetTeamAsync(id);
            if (team == null)
            {
                return NotFound($"Team for project with Id '{id}' not found.");
            }

            return Ok(team);
        }

        [HttpPut("{id}/team")]
        [SwaggerOperation(Summary = "Set Team for Project")]
        public async Task<IActionResult> SetTeam(string id, [FromBody] Team team)
        {
            await _projectService.SetTeamAsync(id, team);
            return NoContent();
        }

        [HttpDelete("{id}/team")]
        [SwaggerOperation(Summary = "Remove Team from Project")]
        public async Task<IActionResult> RemoveTeam(string id)
        {
            await _projectService.RemoveTeamAsync(id);
            return NoContent();
        }

        // Tasks
        [HttpPost("{id}/tasks")]
        [SwaggerOperation(Summary = "Add Task to Project")]
        public async Task<IActionResult> AddTask(string id, [FromBody] Task task)
        {
            await _projectService.AddTaskAsync(id, task);
            return NoContent();
        }

        [HttpDelete("{id}/tasks/{taskId}")]
        [SwaggerOperation(Summary = "Remove Task from Project")]
        public async Task<IActionResult> RemoveTask(string id, string taskId)
        {
            await _projectService.RemoveTaskAsync(id, taskId);
            return NoContent();
        }

        [HttpPut("{id}/tasks")]
        [SwaggerOperation(Summary = "Update Task in Project")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] Task task)
        {
            await _projectService.UpdateTaskAsync(id, task);
            return NoContent();
        }

        // Status
        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Update Project Status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] ProjectStatus status)
        {
            await _projectService.UpdateStatusAsync(id, status);
            return NoContent();
        }

        //Weights

        [HttpPost("update-weights/{projectId}")]
        public async Task<IActionResult> UpdateWeights(string projectId, [FromBody] ProductivityWeights weights)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
            {
                return NotFound($"Project with id {projectId} not found.");
            }

            project.Weights = weights;
            await _projectService.UpdateAsync(projectId, project);

            return NoContent();
        }

        [HttpGet("weights/{projectId}")]
        public async Task<ActionResult<ProductivityWeights>> GetWeights(string projectId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
            {
                return NotFound($"Project with id {projectId} not found.");
            }

            return Ok(project.Weights);
        }
    }
}
