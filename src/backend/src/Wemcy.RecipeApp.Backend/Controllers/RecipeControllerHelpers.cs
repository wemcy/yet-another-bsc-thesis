using System.Text;

namespace Wemcy.RecipeApp.Backend.Controllers
{
    internal static class RecipeControllerHelpers
    {
        public static readonly byte[] DefaultImageSvg = Encoding.UTF8.GetBytes(
           """
        <svg xmlns="http://www.w3.org/2000/svg" width="400" height="300" viewBox="0 0 400 300">
          <rect width="400" height="300" fill="#e0e0e0"/>
          <text x="50%" y="50%" dominant-baseline="middle" text-anchor="middle"
                font-family="sans-serif" font-size="24" fill="#999">No Image</text>
        </svg>
        """);
    }
}