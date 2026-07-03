using Microsoft.AspNetCore.Mvc;
using sprint16_EF_LearningPlatform.DataAccess.Postgres.Repositories;
using sprint16_EF_LearningPlatform.API.DTOs;

namespace sprint16_EF_LearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(StudentsRepository studentsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var students = await studentsRepository.Get();
        return Ok(students);
    }

    [HttpGet("with-courses")]
    public async Task<IActionResult> GetWithCourses()
    {
        var students = await studentsRepository.GetWithCourses();
        return Ok(students);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
    {
        await studentsRepository.Add(request.id, request.userName);
        return Ok();
    }
    
    [HttpPost("{id:guid}/enroll")]
    public async Task<IActionResult> Enroll(Guid id, [FromBody] EnrollStudentRequest request)
    {
        await studentsRepository.Enroll(id, request.courseId);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await studentsRepository.Delete(id);
        return Ok();
    }
}