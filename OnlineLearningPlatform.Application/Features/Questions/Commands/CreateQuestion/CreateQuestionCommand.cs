using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;

namespace OnlineLearningPlatform.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(int QuizId, CreateQuestionDto Dto) : IRequest<QuestionDto>;
