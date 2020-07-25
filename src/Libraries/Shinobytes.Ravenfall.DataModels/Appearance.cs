using Shinobytes.Ravenfall.Data.Entities;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Appearance : Entity<Appearance>
    {
        private Gender gender;
        private int hair;
        private int head;
        private string warPaintColor; 
        private string stubbleColor; 
        private bool helmetVisible; 
        private string eyeColor; 
        private string beardColor; 
        private string hairColor; 
        private string skinColor; 
        private int facialHair; 
        private int eyebrows; 

        public Gender Gender { get => gender; set => Set(ref gender, value); }
        public int Hair { get => hair; set => Set(ref hair, value); }
        public int Head { get => head; set => Set(ref head, value); }
        public int Eyebrows { get => eyebrows; set => Set(ref eyebrows, value); }
        public int FacialHair { get => facialHair; set => Set(ref facialHair, value); }
        public string SkinColor { get => skinColor; set => Set(ref skinColor, value); }
        public string HairColor { get => hairColor; set => Set(ref hairColor, value); }
        public string BeardColor { get => beardColor; set => Set(ref beardColor, value); }
        public string EyeColor { get => eyeColor; set => Set(ref eyeColor, value); }
        public bool HelmetVisible { get => helmetVisible; set => Set(ref helmetVisible, value); }
        public string StubbleColor { get => stubbleColor; set => Set(ref stubbleColor, value); }
        public string WarPaintColor { get => warPaintColor; set => Set(ref warPaintColor, value); }
    }
}