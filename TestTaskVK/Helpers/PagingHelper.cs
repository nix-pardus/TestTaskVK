using TestTaskVK.Models;

namespace TestTaskVK.Helpers
{
    public static class PagingHelper
    {
        public class PageInfo
        {
            /// <summary>
            /// Номер текущей страницы
            /// </summary>
            public int PageNumber { get; set; }
            /// <summary>
            /// Количество объектов на странице
            /// </summary>
            public int PageSize { get; set; }
            /// <summary>
            /// Всего объектов
            /// </summary>
            public int TotalItems { get; set; }
            /// <summary>
            /// Всего страниц
            /// </summary>
            public int TotalPages
            {
                get => (int)Math.Ceiling((double)TotalItems / PageSize);
            }
        }

        public class IndexViewModel
        {
            public IEnumerable<User> Users { get; set; }
            public PageInfo PageInfo { get; set; }
        }
    }
}
