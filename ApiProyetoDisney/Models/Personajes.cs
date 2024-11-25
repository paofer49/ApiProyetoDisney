namespace ApiProyetoDisney.Models
{
    public class Personajes
    {
        public int _id { get; set; }
        public string ?name { get; set; }
        public string ?imageUrl { get; set; }
        public List<string> ?films { get; set; }
        public List<string> ?tvShows { get; set; }
    }
}
