using MediatR;

namespace OnlineLearningPlatform.Application.Features.Questions.Commands.DeleteQuestion;

public record DeleteQuestionCommand(int Id) : IRequest<bool>;
