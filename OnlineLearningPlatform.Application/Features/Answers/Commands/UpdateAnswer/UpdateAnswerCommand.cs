using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.UpdateAnswer;

public record UpdateAnswerCommand(int Id, CreateAnswerDto Dto) : IRequest<AnswerDto?>;
