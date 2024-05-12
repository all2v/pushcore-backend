using Astrum.Articles.Application.Models.Requests;
using Astrum.Articles.Requests;
using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Articles.Application.Features.Commands.Articles.UpdateArticleCommand
{
    public class UpdateArticleCommand : CommandResult<ArticleView>
    {
        public Guid ArticleId { get; set; }
        public ArticleEditRequest Article { get; set; }
        public UpdateArticleCommand(Guid articleId, ArticleEditRequest article)
        {
            ArticleId = articleId;
            Article = article;
        }
    }
}
