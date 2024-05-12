using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Astrum.Logging.Entities
{
    public class LogHttp : AbstractLog
    {
        public string StatusCode { get; set; }
        public TypeRequest TypeRequest { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string BodyRequest { get; set; }
        public string RequestResponse { get; set; }
    }

    public enum TypeRequest
    {
        GET, 
        POST, 
        PUT, 
        DELETE
    }

    public enum ModuleAstrum
    {
        [Display(Name = "Пользователи")]
        Account,
        [Display(Name = "Заявки")]
        Appeal,
        [Display(Name = "Статьи")]
        Articles,
        [Display(Name = "Календарь")]
        Calendar,
        [Display(Name = "Инвентаризация")]
        Inventory,
        [Display(Name = "Магазин")]
        Market,
        [Display(Name = "Новости")]
        News,
        [Display(Name = "Проект")]
        Project,
        [Display(Name = "Трекер задач")]
        TrackerProject,
        [Display(Name = "Авторизация")]
        Identity,
        [Display(Name = "Долги")]
        Debts,
        [Display(Name = "Код ревью")]
        CodeRev
    }
}
