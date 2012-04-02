using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ru.ocltd.linq.test
{
    public class SampleEntity
    {
        [Key]
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Необходимо указать краткое наименование!")]
        [MinLength(2, ErrorMessage = "Краткое наименование должно содержать не менее 2 символов!")]
        [MaxLength(300, ErrorMessage = "Краткое наименование должно содержать не более 300 символов!")]
        [Display(Name = "Краткое Наименование")]
        public string EntityName { get; set; }

        [Required(ErrorMessage = "Необходимо указать полное описание!")]
        [MinLength(5, ErrorMessage = "Полное описание должно содержать не менее 10 символов!")]
        [MaxLength(1000, ErrorMessage = "Полное описание должно содержать не более 1000 символов!")]
        [Display(Name = "Полное Описание")]
        public string EntityDescription { get; set; }

        [Range(typeof(decimal), "0,01", "100,00", ErrorMessage = "Рейтинг должен иметь значение в диапазоне от 0,01 до 100,00!")]
        [Display(Name = "Рейтинг")]
        public decimal Rank { get; set; }

        [Display(Name = "Действующий")]
        public bool IsObsolete { get; set; }

        [Display(Name = "Дата изменения")]
        public DateTime LastModified { get; set; }
    }
}
