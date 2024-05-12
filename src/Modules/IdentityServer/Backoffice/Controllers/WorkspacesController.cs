// using Api.Application.Commands;
// using Astrum.Infrastructure.Shared;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Api.Controllers;
//
// public class WorkspacesController : ApiBaseController
// {
//     [HttpGet]
//     public async Task<IEnumerable<Workspace>> Get()
//     {
//         return await Mediator.Send(new GetWorkspacesQuery());
//     }
//
//     [HttpGet("{id:guid}")]
//     public async Task<Workspace> GetById(Guid id)
//     {
//         return await Mediator.Send(new GetWorkspaceByIdQuery(id));
//     }
//
//     /// <summary>
//     ///     Creates workspace with related projects
//     /// </summary>
//     /// <remarks>
//     ///     {
//     ///     "name": "New Project",
//     ///     "projects": [
//     ///     {
//     ///     "alias": "awesome-project"
//     ///     }
//     ///     ]
//     ///     }
//     /// </remarks>
//     [HttpPost]
//     public async Task Create(CreateWorkspaceCommand command)
//     {
//         await Mediator.Send(command);
//     }
//
//     [HttpDelete("{id:guid}")]
//     public async Task Delete(Guid id)
//     {
//         await Mediator.Send(new DeleteWorkspaceCommand(id));
//     }
//
//     [HttpDelete("{id:guid}/v2")]
//     public async Task DeleteAOP(Guid id)
//     {
//         await Mediator.Send(new DeleteWorkspaceCommandAOP(id));
//     }
//
//     [HttpDelete("")]
//     public async Task DeleteAll([FromServices] ApplicationDbContext db)
//     {
//         await db.Database.ExecuteSqlRawAsync("TRUNCATE public.\"Workspaces\" CASCADE");
//     }
// }