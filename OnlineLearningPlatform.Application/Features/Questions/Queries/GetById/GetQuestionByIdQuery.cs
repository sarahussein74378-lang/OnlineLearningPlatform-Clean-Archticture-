using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Questions.Queries.GetById;

public record GetQuestionByIdQuery(int Id) : IRequest<QuestionDto?>;
