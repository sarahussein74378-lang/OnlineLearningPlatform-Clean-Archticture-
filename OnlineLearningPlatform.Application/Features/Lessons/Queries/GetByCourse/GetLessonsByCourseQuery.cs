using MediatR;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Lessons.Queries.GetByCourse;

public record GetLessonsByCourseQuery(int CourseId) : IRequest<IEnumerable<LessonDto>>;
