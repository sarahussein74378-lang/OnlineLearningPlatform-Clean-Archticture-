using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.CreateAnswer;

public record CreateAnswerCommand(int QuestionId, CreateAnswerDto Dto) : IRequest<AnswerDto>;
