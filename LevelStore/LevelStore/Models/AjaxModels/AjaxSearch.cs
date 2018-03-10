namespace LevelStore.Models.AjaxModels
{
    public class AjaxSearch
    {
        public string SearchString { get; set; }
        public int? SubCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int FirstInOrder { get; set; }
    }
}
