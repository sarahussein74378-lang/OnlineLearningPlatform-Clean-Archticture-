using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(int Id, CreateQuestionDto Dto) : IRequest<QuestionDto?>;
