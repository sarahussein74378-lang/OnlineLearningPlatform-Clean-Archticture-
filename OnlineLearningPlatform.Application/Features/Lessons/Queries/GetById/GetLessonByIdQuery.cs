using MediatR;
using OnlineLearningPlatform.Application.DTOs.Lesson;

namespace OnlineLearningPlatform.Application.Features.Lessons.Queries.GetById;

public record GetLessonByIdQuery(int Id) : IRequest<LessonDto?>;
