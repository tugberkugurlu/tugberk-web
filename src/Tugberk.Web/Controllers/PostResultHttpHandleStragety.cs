using System;
using Microsoft.AspNetCore.Mvc;
using Tugberk.Domain;
using Tugberk.Persistance.Abstractions;

namespace Tugberk.Web.Controllers
{
    /// <seealso href="https://blogs.msdn.microsoft.com/curth/2008/11/15/c-dynamic-and-multiple-dispatch/" />
    /// <seealso href="https://stackoverflow.com/a/9504523/463785" />
    public class PostResultHttpHandleStragety
    {
        private readonly Controller _controller;

        public PostResultHttpHandleStragety(Controller controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            _controller = controller;
        }

        public IActionResult HandleResult(Result result)
        {
            if (result == null) throw new System.ArgumentNullException(nameof(result));
            return Convert((dynamic)result);
        }

        private IActionResult Convert(Tugberk.Persistance.Abstractions.NotFoundResult result) =>
            new Microsoft.AspNetCore.Mvc.NotFoundResult();

        private IActionResult Convert(NotApprovedResult<Post> result) =>
            new Microsoft.AspNetCore.Mvc.NotFoundResult();

        private IActionResult Convert(FoundByNonDefaultSlugResult<Post> result) =>
            new RedirectToActionResult(
                nameof(PostsController.Index),
                "Posts", 
                new { slug = result.DefaultSlug.Path });

        private IActionResult Convert(FoundResult<Post> result) =>
            _controller.View(result.Model);

        private IActionResult Convert(Result result) =>
            throw new NotSupportedException($"{result.GetType()} is not supported for handling.");
    }
}
