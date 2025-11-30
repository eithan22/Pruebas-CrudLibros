namespace Domian.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;

        // NUEVO CAMPO PARA BORRADO LÓGICO
        public bool Eliminado { get; set; } = false;

    }
}
