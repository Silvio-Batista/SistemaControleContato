using System.ComponentModel.DataAnnotations;

namespace ControleDeContato.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O Campo Nome precisa ser preenchido")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O Campo Email precisa ser preenchido")]
        [EmailAddress(ErrorMessage ="O email informado é inválido")]
        public string Email { get; set;}
        [Required(ErrorMessage = "O Campo Celular precisa ser preenchido")]
        [Phone(ErrorMessage = "O celular é inválido")]
        public string Celular { get; set;}
    }
}
